#if !ONPREMISES
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Admin
{
    [Cmdlet(VerbsCommon.Set, "PnPHubSite")]
    [CmdletHelp(@"Sets hub site properties", "Allows configuring a hub site",
        Category = CmdletHelpCategory.TenantAdmin,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = @"PS:> Set-PnPHubSite -Identity https://tenant.sharepoint.com/sites/myhubsite -Title ""My New Title""",
        Remarks = "Sets the title of the hub site",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Set-PnPHubSite -Identity https://tenant.sharepoint.com/sites/myhubsite -Description ""My updated description""",
        Remarks = "Sets the description of the hub site",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Set-PnPHubSite -Identity https://tenant.sharepoint.com/sites/myhubsite -SiteDesignId df8a3ef1-9603-44c4-abd9-541aea2fa745",
        Remarks = "Sets the site design which should be applied to sites joining the the hub site",
        SortOrder = 3)]
    [CmdletExample(
        Code = @"PS:> Set-PnPHubSite -Identity https://tenant.sharepoint.com/sites/myhubsite -LogoUrl ""https://tenant.sharepoint.com/SiteAssets/Logo.png""",
        Remarks = "Sets the logo of the hub site",
        SortOrder = 4)]
    public class SetHubSite : PnPAdminCmdlet
    {
        [Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true, HelpMessage = "The Id or Url of a hub site to configure")]
        [Alias("HubSite")]
        public HubSitePipeBind Identity { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "The title to set on the hub which will be shown in the hub navigation bar")]
        public string Title { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Full url to the image to use for the hub site logo. Can either be a logo hosted on SharePoint or outside of SharePoint and must be an absolute URL to the image.")]
        public string LogoUrl { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "The description of the hub site")]
        public string Description { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "GUID of the SharePoint Site Design which should be applied when a site joins the hub site")]
        public GuidPipeBind SiteDesignId;

        [Parameter(Mandatory = false)]
        public SwitchParameter HideNameInNavigation;

        [Parameter(Mandatory = false)]
        public SwitchParameter RequiresJoinApproval;

        protected override void ExecuteCmdlet()
        {
            var hubSiteProperties = Identity.GetHubSite(Tenant);
            ClientContext.Load(hubSiteProperties);
            if (ParameterSpecified(nameof(Title)))
            {
                hubSiteProperties.Title = Title;
            }
            if (ParameterSpecified(nameof(LogoUrl)))
            {
                hubSiteProperties.LogoUrl = LogoUrl;
            }
            if (ParameterSpecified(nameof(Description)))
            {
                hubSiteProperties.Description = Description;
            }
            if (ParameterSpecified(nameof(SiteDesignId)))
            {
                hubSiteProperties.SiteDesignId = SiteDesignId.Id;
            }
            if (ParameterSpecified(nameof(HideNameInNavigation)))
            {
                hubSiteProperties.HideNameInNavigation = HideNameInNavigation.ToBool();
            }
            if (ParameterSpecified(nameof(RequiresJoinApproval)))
            {
                hubSiteProperties.RequiresJoinApproval = RequiresJoinApproval.ToBool();
            }
            hubSiteProperties.Update();
            ClientContext.ExecuteQueryRetry();
        }
    }
}
#endif