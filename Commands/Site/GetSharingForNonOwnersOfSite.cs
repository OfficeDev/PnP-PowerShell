#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantManagement;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Site
{
    [Cmdlet(VerbsCommon.Get, "PnPSharingForNonOwnersOfSite")]
    [CmdletHelp("Returns $false if sharing of the site and items in the site is restricted only to owners or $true if members and owners are allowed to share",
        DetailedDescription = "Returns $false if sharing of the site and items in the site is restricted only to owners or $true if members and owners are allowed to share. You can disable sharing by non owners by using Disable-PnPSharingForNonOwnersOfSite. At this point there is no interface available yet to enable sharing by owners and members again through script. You will have to do so through the user interface of SharePoint.",
        Category = CmdletHelpCategory.Sites,
        SupportedPlatform = CmdletSupportedPlatform.Online,
        OutputType = typeof(bool))]
    [CmdletExample(
        Code = @"PS:> Get-PnPSharingForNonOwnersOfSite",
        Remarks = "Returns $false if sharing of the site and items in the site is restricted only to owners or $true if members and owners are allowed to share",
        SortOrder = 1)]

    public class GetSharingForNonOwnersOfSite : PnPSharePointCmdlet
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true)]
        [Alias("Url")]
        public SitePipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            var context = ClientContext;
            var site = ClientContext.Site;
            var siteUrl = ClientContext.Url;

            if (ParameterSpecified(nameof(Identity)))
            {
                context = ClientContext.Clone(Identity.Url);
                site = context.Site;
                siteUrl = context.Url;
            }

            Office365Tenant office365Tenant = new Office365Tenant(context);
            context.Load(office365Tenant);
            var isSharingDisabledForNonOwnersOfSite = office365Tenant.IsSharingDisabledForNonOwnersOfSite(siteUrl);
            context.ExecuteQueryRetry();

            // Inverting the outcome here on purpose as the wording of the cmdlet indicates that a true means sharing for owners and members is allowed and false would mean only sharing for owners would be allowed
            WriteObject(!isSharingDisabledForNonOwnersOfSite.Value);
        }
    }
}
#endif
