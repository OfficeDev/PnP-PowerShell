#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using System.Management.Automation;
using OfficeDevPnP.Core.Sites;

namespace SharePointPnP.PowerShell.Commands.Admin
{
    [Cmdlet(VerbsCommon.Add, "PnPOffice365GroupToSite")]
    [CmdletHelp("Groupifies a classic team site by creating an Office 365 group for it and connecting the site with the newly created group",
        DetailedDescription = "This command allows you to add an Office 365 Unified group to an existing classic site collection.",
        SupportedPlatform = CmdletSupportedPlatform.Online,
        Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(
        Code = @"PS:> Add-PnPOffice365GroupToSite -Url ""https://contoso.sharepoint.com/sites/FinanceTeamsite"" -Alias ""FinanceTeamsite"" -DisplayName = ""My finance team site group""",
        Remarks = @"This will add a group called MyGroup to the current site collection", SortOrder = 1)]
    public class AddOffice365GroupToSite: PnPAdminCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = @"Url of the site to be connected to an Office 365 Group.")]
        public string Url;

        [Parameter(Mandatory = true, HelpMessage = @"Specifies the alias of the group. Cannot contain spaces.")]
        public string Alias;

        [Parameter(Mandatory = false, HelpMessage = @"The optional description of the group.")]
        public string Description;

        [Parameter(Mandatory = true, HelpMessage = @"The display name of the group.")]
        public string DisplayName;

        [Parameter(Mandatory = false, HelpMessage = @"Specifies the classification of the group.")]
        public string Classification;

        [Parameter(Mandatory = false, HelpMessage = @"Specifies if the group is public. Defaults to false.")]
        public SwitchParameter IsPublic;

        [Parameter(Mandatory = false, HelpMessage = @"Specifies if the current site home page is kept. Defaults to false.")]
        public SwitchParameter KeepOldHomePage;

        protected override void ExecuteCmdlet()
        {
            var groupifyInformation = new TeamSiteCollectionGroupifyInformation()
            {
                Alias = Alias,
                DisplayName = DisplayName,
                Description = Description,
                Classification = Classification,
                IsPublic = IsPublic,
                KeepOldHomePage = KeepOldHomePage
            };

            Tenant.GroupifySite(Url, groupifyInformation);
        }
    }
}
#endif