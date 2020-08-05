using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Search.Administration;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Enums;
using Resources = PnP.PowerShell.Commands.Properties.Resources;

namespace PnP.PowerShell.Commands.Search
{
    [Cmdlet(VerbsCommon.Set, "PnPSearchConfiguration")]
    [CmdletHelp("Sets the search configuration",
        Category = CmdletHelpCategory.Search)]
    [CmdletExample(
        Code = @"PS:> Set-PnPSearchConfiguration -Configuration $config",
        Remarks = "Sets the search configuration for the current web",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Set-PnPSearchConfiguration -Configuration $config -Scope Site",
        Remarks = "Sets the search configuration for the current site collection",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Set-PnPSearchConfiguration -Configuration $config -Scope Subscription",
        Remarks = "Sets the search configuration for the current tenant",
        SortOrder = 3)]
    [CmdletExample(
        Code = @"PS:> Set-PnPSearchConfiguration -Path searchconfig.xml -Scope Subscription",
        Remarks = "Reads the search configuration from the specified XML file and sets it for the current tenant",
        SortOrder = 4)]

    public class SetSearchConfiguration : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, ParameterSetName = "Config", HelpMessage = "Search configuration string")]
        public string Configuration;

        [Parameter(Mandatory = true, ParameterSetName = "Path", HelpMessage = "Path to a search configuration")]
        public string Path;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets)]
        public SearchConfigurationScope Scope = SearchConfigurationScope.Web;

        protected override void ExecuteCmdlet()
        {
            if (ParameterSetName == "Path")
            {
                if (!System.IO.Path.IsPathRooted(Path))
                {
                    Path = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Path);
                }
                Configuration = System.IO.File.ReadAllText(Path);
            }
            switch (Scope)
            {
                case SearchConfigurationScope.Web:
                    {
                        SelectedWeb.SetSearchConfiguration(Configuration);
                        break;
                    }
                case SearchConfigurationScope.Site:
                    {
                        ClientContext.Site.SetSearchConfiguration(Configuration);
                        break;
                    }
                case SearchConfigurationScope.Subscription:
                    {
                        if (!ClientContext.Url.ToLower().Contains("-admin"))
                        {
                            throw new InvalidOperationException(Resources.CurrentSiteIsNoTenantAdminSite);
                        }

                        ClientContext.ImportSearchSettingsConfiguration(Configuration, SearchObjectLevel.SPSiteSubscription);
                        break;
                    }
            }
        }
    }
}
