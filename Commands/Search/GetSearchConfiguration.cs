using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Search.Administration;
using Microsoft.SharePoint.Client.Search.Portability;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Enums;
using Resources = SharePointPnP.PowerShell.Commands.Properties.Resources;

namespace SharePointPnP.PowerShell.Commands.Search
{
    [Cmdlet(VerbsCommon.Get, "PnPSearchConfiguration")]
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
    public class GetSearchConfiguration : SPOWebCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "Scope to use. Either Web, Site, or Subscription. Defaults to Web")]
        public SearchConfigurationScope Scope = SearchConfigurationScope.Web;

        [Parameter(Mandatory = false, HelpMessage = "Local path where the search configuration will be saved")]
        public string Path;

        protected override void ExecuteCmdlet()
        {
            string configoutput = string.Empty;

            switch (Scope)
            {
                case SearchConfigurationScope.Web:
                    {
                        configoutput = SelectedWeb.GetSearchConfiguration();
                        break;
                    }
                case SearchConfigurationScope.Site:
                    {
                        configoutput = ClientContext.Site.GetSearchConfiguration();
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

                        configoutput = configuration.Value;
                    }
                    break;
            }

            if (Path != null)
            {
                if (!System.IO.Path.IsPathRooted(Path))
                {
                    Path = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Path);
                }
                System.IO.File.WriteAllText(Path, configoutput);
            }
            else
            {
                WriteObject(configoutput);
            }
        }
    }
}
