using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Xml.Linq;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Search.Administration;
using Microsoft.SharePoint.Client.Search.Portability;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Enums;
using Resources = SharePointPnP.PowerShell.Commands.Properties.Resources;

namespace SharePointPnP.PowerShell.Commands.Search
{
    public enum OutputFormat
    {
        CompleteXml = 0,
        ManagedPropertyMappings = 1
    }

    [Cmdlet(VerbsCommon.Get, "PnPSearchConfiguration", DefaultParameterSetName = "Xml")]
    [CmdletAlias("Get-SPOSearchConfiguration")]
    [CmdletHelp("Returns the search configuration",
        Category = CmdletHelpCategory.Search,
        OutputType = typeof(string),
        OutputTypeDescription = "Does not return a string when the -Path parameter has been specified.")]
    [CmdletExample(
        Code = @"PS:> Get-PnPSearchConfiguration",
        Remarks = "Returns the search configuration for the current web",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPSearchConfiguration -Scope Site",
        Remarks = "Returns the search configuration for the current site collection",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Get-PnPSearchConfiguration -Scope Subscription",
        Remarks = "Returns the search configuration for the current tenant",
        SortOrder = 3)]
    [CmdletExample(
        Code = @"PS:> Get-PnPSearchConfiguration -Path searchconfig.xml -Scope Subscription",
        Remarks = "Returns the search configuration for the current tenant and saves it to the specified file",
        SortOrder = 4)]
    [CmdletExample(
        Code = @"PS:> Get-PnPSearchConfiguration -Scope Site -OutputFormat ManagedPropertyMappings",
        Remarks = "Returns all custom managed properties and crawled property mapping at the current site collection",
        SortOrder = 5)]
    public class GetSearchConfiguration : PnPWebCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "Scope to use. Either Web, Site, or Subscription. Defaults to Web", ParameterSetName = ParameterAttribute.AllParameterSets)]
        public SearchConfigurationScope Scope = SearchConfigurationScope.Web;

        [Parameter(Mandatory = false, HelpMessage = "Local path where the search configuration will be saved", ParameterSetName = "Xml")]
        public string Path;

        [Parameter(Mandatory = false, HelpMessage = "Output format for of the configuration. Defaults to complete XML",
            ParameterSetName = "OutputFormat")]
        public OutputFormat OutputFormat = OutputFormat.CompleteXml;

        protected override void ExecuteCmdlet()
        {
            string configOutput = string.Empty;

            switch (Scope)
            {
                case SearchConfigurationScope.Web:
                    {
                        configOutput = SelectedWeb.GetSearchConfiguration();
                        break;
                    }
                case SearchConfigurationScope.Site:
                    {
                        configOutput = ClientContext.Site.GetSearchConfiguration();
                        break;
                    }
                case SearchConfigurationScope.Subscription:
                    {
                        if (!ClientContext.Url.ToLower().Contains("-admin"))
                        {
                            throw new InvalidOperationException(Resources.CurrentSiteIsNoTenantAdminSite);
                        }

                        SearchObjectOwner owningScope = new SearchObjectOwner(ClientContext, SearchObjectLevel.SPSiteSubscription);
                        var config = new SearchConfigurationPortability(ClientContext);
                        ClientResult<string> configuration = config.ExportSearchConfiguration(owningScope);
                        ClientContext.ExecuteQueryRetry(10, 60 * 5 * 1000);

                        configOutput = configuration.Value;
                    }
                    break;
            }

            if (Path != null)
            {
                if (!System.IO.Path.IsPathRooted(Path))
                {
                    Path = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Path);
                }
                System.IO.File.WriteAllText(Path, configOutput);
            }
            else
            {
                if (OutputFormat == OutputFormat.CompleteXml)
                {
                    WriteObject(configOutput);
                }
                else if (OutputFormat == OutputFormat.ManagedPropertyMappings)
                {
                    StringReader sr = new StringReader(configOutput);
                    var doc = XDocument.Load(sr);
                    var mps = GetCustomManagedProperties(doc);

                    foreach (var mp in mps)
                    {
                        mp.Aliases = new List<string>();
                        mp.Mappings = new List<string>();

                        var mappings = GetCpMappingsFromPid(doc, mp.Pid);
                        mp.Mappings = mappings;
                        var aliases = GetAliasesFromPid(doc, mp.Pid);
                        mp.Aliases = aliases;
                    }
                    WriteObject(mps);
                }
            }
        }

        #region Helper functions

        internal class ManagedProperty
        {
            public string Name { get; set; }
            public List<string> Aliases { get; set; }

            public List<string> Mappings { get; set; }

            public string Type { get; set; }

            internal string Pid { get; set; }
        }

        private static string PidToName(string pid)
        {
            /*
    RefinableString00 1000000000
    Int00             1000000100
    Date00            1000000200
    Decimal00         1000000300
    Double00          1000000400
    RefinableInt00    1000000500
    RefinableDate00   1000000600
    RefinableDateSingle00 1000000650
    RefinableDateInvariant00  1000000660
    RefinableDecimal00    1000000700
    RefinableDouble00     1000000800
    RefinableString100  1000000900                
 */
            int p;
            if (!int.TryParse(pid, out p)) return pid;

            var autoMpNum = pid.Substring(pid.Length - 2);
            var mpName = pid;

            if (p < 1000000100) mpName = "RefinableString";
            else if (p < 1000000200) mpName = "Int";
            else if (p < 1000000300) mpName = "Date";
            else if (p < 1000000400) mpName = "Decimal";
            else if (p < 1000000500) mpName = "Double";
            else if (p < 1000000600) mpName = "RefinableInt";
            else if (p < 1000000650) mpName = "RefinableDate";
            else if (p < 1000000660) mpName = "RefinableDateSingle";
            else if (p < 1000000700) mpName = "RefinableDateInvariant";
            else if (p < 1000000800) mpName = "RefinableDecimal";
            else if (p < 1000000900) mpName = "RefinableDouble";
            else if (p < 1000001000) mpName = "RefinableString1";
            return mpName + autoMpNum;
        }

        private static List<ManagedProperty> GetCustomManagedProperties(XDocument doc)
        {
            var mpList = new List<ManagedProperty>();
            var mps =
                doc.Descendants()
                    .Where(n => n.Name.LocalName.StartsWith("KeyValueOfstringManagedPropertyInfo"));
            foreach (var mpNode in mps)
            {
                var name = mpNode.Descendants().Single(n => n.Name.LocalName == "Name").Value;
                var pid = mpNode.Descendants().Single(n => n.Name.LocalName == "Pid").Value;
                var type = mpNode.Descendants().Single(n => n.Name.LocalName == "ManagedType").Value;
                var mp = new ManagedProperty
                {
                    Name = PidToName(name),
                    Pid = pid,
                    Type = type
                };
                mpList.Add(mp);
            }

            var overrides = doc.Descendants()
                .Where(n => n.Name.LocalName.StartsWith("KeyValueOfstringOverrideInfo"));
            foreach (var o in overrides)
            {
                var name = o.Descendants().Single(n => n.Name.LocalName == "Name").Value;
                var pid = o.Descendants().Single(n => n.Name.LocalName == "ManagedPid").Value;
                var mp = new ManagedProperty
                {
                    Name = PidToName(name),
                    Pid = pid
                };
                if (mp.Name.Contains("String")) mp.Type = "Text";
                else if (mp.Name.Contains("Date")) mp.Type = "Date";
                else if (mp.Name.Contains("Int")) mp.Type = "Integer";
                else if (mp.Name.Contains("Double")) mp.Type = "Double";
                else if (mp.Name.Contains("Decimal")) mp.Type = "Decimal";
                mpList.Add(mp);
            }
            return mpList;
        }

        private static List<string> GetAliasesFromPid(XDocument doc, string pid)
        {
            var aliasList = new List<string>();
            var aliases = doc.Descendants().Where(n => n.Name.LocalName.StartsWith("KeyValueOfstringAliasInfo"));
            foreach (var alias in aliases)
                if (alias.Descendants().Single(n => n.Name.LocalName == "ManagedPid").Value == pid)
                {
                    var aliasName = alias.Descendants().Single(n => n.Name.LocalName == "Name").Value;
                    aliasList.Add(aliasName);
                }
            return aliasList;
        }

        private static List<string> GetCpMappingsFromPid(XDocument doc, string pid)
        {
            var mappingList = new List<string>();
            var cps = doc.Descendants().Where(n => n.Name.LocalName.StartsWith("KeyValueOfstringMappingInfo"));
            foreach (var cp in cps)
                if (cp.Descendants().Single(n => n.Name.LocalName == "ManagedPid").Value == pid)
                {
                    var cpName = cp.Descendants().Single(n => n.Name.LocalName == "CrawledPropertyName").Value;
                    mappingList.Add(cpName);
                }
            return mappingList;
        }
        #endregion
    }
}
