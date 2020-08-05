#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantManagement;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Site
{
    [Cmdlet(VerbsLifecycle.Disable, "PnPSharingForNonOwnersOfSite")]
    [CmdletHelp("Configures the site to only allow sharing of the site and items in the site by owners",
        DetailedDescription = "Configures the site to only allow sharing of the site and items in the site by owners. At this point there is no interface available yet to undo this action through script. You will have to do so through the user interface of SharePoint.",
        Category = CmdletHelpCategory.Sites,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = @"PS:> Disable-PnPSharingForNonOwnersOfSite",
        Remarks = "Restricts sharing of the site and items in the site only to owners",
        SortOrder = 1)]

    public class DisableSharingForNonOwnersOfSite : PnPSharePointCmdlet
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true)]
        [Alias("Url")]
        public SitePipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            var context = ClientContext;
            var site = ClientContext.Site;
            var siteUrl = ClientContext.Url;

            if(ParameterSpecified(nameof(Identity)))
            {
                context = ClientContext.Clone(Identity.Url);
                site = context.Site;
                siteUrl = context.Url;
            }

            Office365Tenant office365Tenant = new Office365Tenant(context);
            context.Load(office365Tenant);
            office365Tenant.DisableSharingForNonOwnersOfSite(siteUrl);
            context.ExecuteQueryRetry();
        }
    }
}
#endif
