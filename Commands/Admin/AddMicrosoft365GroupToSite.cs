#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using System.Management.Automation;
using OfficeDevPnP.Core.Sites;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.Admin
{
    [Cmdlet(VerbsCommon.Add, "PnPMicrosoft365GroupToSite")]
    [CmdletHelp("Groupifies a classic team site by creating a Microsoft 365 group for it and connecting the site with the newly created group",
        DetailedDescription = "This command allows you to add a Microsoft 365 Unified group to an existing classic site collection, also known as groupifying.",
        SupportedPlatform = CmdletSupportedPlatform.Online,
        Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(
        Code = @"PS:> Add-PnPOffice365GroupToSite -Url ""https://contoso.sharepoint.com/sites/FinanceTeamsite"" -Alias ""FinanceTeamsite"" -DisplayName = ""My finance team site group""",
        Remarks = @"This will groupify the FinanceTeamsite", SortOrder = 1)]
    [Alias("Add-PnPOffice365GroupToSite")]
    public class AddMicrosoft365GroupToSite: PnPAdminCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = @"Url of the site to be connected to an Microsoft 365 Group")]
        public string Url;

        [Parameter(Mandatory = true, HelpMessage = @"Specifies the alias of the group. Cannot contain spaces.")]
        public string Alias;

        [Parameter(Mandatory = false, HelpMessage = @"The optional description of the group")]
        public string Description;

        [Parameter(Mandatory = true, HelpMessage = @"The display name of the group")]
        public string DisplayName;

        [Parameter(Mandatory = false, HelpMessage = @"Specifies the classification of the group")]
        public string Classification;

        [Parameter(Mandatory = false, HelpMessage = @"Specifies if the group is public. Defaults to false.")]
        public SwitchParameter IsPublic;

        [Parameter(Mandatory = false, HelpMessage = @"Specifies if the current site home page is kept. Defaults to false.")]
        public SwitchParameter KeepOldHomePage;

        [Parameter(Mandatory = false, HelpMessage = "If specified the site will be associated to the hubsite as identified by this id")]
        public GuidPipeBind HubSiteId;

        [Parameter(Mandatory = false, HelpMessage = "The array UPN values of the group's owners")]
        public string[] Owners;
        protected override void ExecuteCmdlet()
        {            
            var groupifyInformation = new TeamSiteCollectionGroupifyInformation()
            {
                Alias = Alias,
                DisplayName = DisplayName,
                Description = Description,
                Classification = Classification,
                IsPublic = IsPublic,
                KeepOldHomePage = KeepOldHomePage,
                Owners = Owners
            };

            if (ParameterSpecified(nameof(HubSiteId)))
            {
                groupifyInformation.HubSiteId = HubSiteId.Id;
            }

            Tenant.GroupifySite(Url, groupifyInformation);
        }
    }
}
#endif