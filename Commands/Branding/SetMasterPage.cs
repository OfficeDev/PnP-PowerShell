using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Utilities;
using PnP.PowerShell.CmdletHelpAttributes;

namespace PnP.PowerShell.Commands.Branding
{
    [Cmdlet(VerbsCommon.Set, "PnPMasterPage")]
    [CmdletHelp("Set the masterpage",
        "Sets the default master page of the current web.",
        Category = CmdletHelpCategory.Branding)]
    [CmdletExample(
        Code = @"PS:> Set-PnPMasterPage -MasterPageServerRelativeUrl /sites/projects/_catalogs/masterpage/oslo.master",
        Remarks = "Sets the master page based on a server relative URL",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Set-PnPMasterPage -MasterPageServerRelativeUrl /sites/projects/_catalogs/masterpage/oslo.master -CustomMasterPageServerRelativeUrl /sites/projects/_catalogs/masterpage/oslo.master",
        Remarks = "Sets the master page and custom master page based on a server relative URL",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Set-PnPMasterPage -MasterPageSiteRelativeUrl _catalogs/masterpage/oslo.master",
        Remarks = "Sets the master page based on a site relative URL",
        SortOrder = 3)]
    [CmdletExample(
        Code = @"PS:> Set-PnPMasterPage -MasterPageSiteRelativeUrl _catalogs/masterpage/oslo.master -CustomMasterPageSiteRelativeUrl _catalogs/masterpage/oslo.master",
        Remarks = "Sets the master page and custom master page based on a site relative URL",
        SortOrder = 4)]
    public class SetMasterPage : PnPWebCmdlet
    {
        private const string ParameterSet_SERVER = "Server Relative";
        private const string ParameterSet_SITE = "Site Relative";

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_SERVER, HelpMessage = "Specifies the Master page URL based on the server relative URL")]
        [Alias("MasterPageUrl")]
        public string MasterPageServerRelativeUrl = null;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_SERVER, HelpMessage = "Specifies the custom Master page URL based on the server relative URL")]
        [Alias("CustomMasterPageUrl")]
        public string CustomMasterPageServerRelativeUrl = null;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_SITE, HelpMessage = "Specifies the Master page URL based on the site relative URL")]
        public string MasterPageSiteRelativeUrl = null;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_SITE, HelpMessage = "Specifies the custom Master page URL based on the site relative URL")]
        public string CustomMasterPageSiteRelativeUrl = null;

        protected override void ExecuteCmdlet()
        {
            if (SelectedWeb.IsNoScriptSite())
            {
                ThrowTerminatingError(new ErrorRecord(new Exception("Site has NoScript enabled, and setting custom master pages is not supported."), "NoScriptEnabled", ErrorCategory.InvalidOperation, this));
                return;
            }

            if (ParameterSetName == ParameterSet_SERVER)
            {
                if (!string.IsNullOrEmpty(MasterPageServerRelativeUrl))
                    SelectedWeb.SetMasterPageByUrl(MasterPageServerRelativeUrl);

                if (!string.IsNullOrEmpty(CustomMasterPageServerRelativeUrl))
                    SelectedWeb.SetCustomMasterPageByUrl(CustomMasterPageServerRelativeUrl);
            }
            else
            {
                if (!string.IsNullOrEmpty(MasterPageSiteRelativeUrl))
                {
                    SelectedWeb.SetMasterPageByUrl(GetServerRelativeUrl(MasterPageSiteRelativeUrl));
                }
                if (!string.IsNullOrEmpty(CustomMasterPageSiteRelativeUrl))
                {
                    SelectedWeb.SetCustomMasterPageByUrl(GetServerRelativeUrl(CustomMasterPageSiteRelativeUrl));
                }
            }
        }

        private string GetServerRelativeUrl(string url)
        {
            var serverRelativeUrl = SelectedWeb.EnsureProperty(w => w.ServerRelativeUrl);
            return UrlUtility.Combine(serverRelativeUrl, url);
        }
    }
}
