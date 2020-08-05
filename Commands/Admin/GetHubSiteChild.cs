#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.Management.Automation;
using Resources = PnP.PowerShell.Commands.Properties.Resources;

namespace PnP.PowerShell.Commands.Admin
{
    [Cmdlet(VerbsCommon.Get, "PnPHubSiteChild")]
    [CmdletHelp(@"Retrieves all sites linked to a specific hub site",
        "Retrieves all sites linked to a specific hub site",
        Category = CmdletHelpCategory.TenantAdmin,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(Code = @"PS:> Get-PnPHubSiteChild -Identity https://contoso.sharepoint.com/sites/myhubsite", Remarks = "Returns the sites having configured the provided hub site as their hub site", SortOrder = 1)]
    public class GetHubSiteChild : PnPAdminCmdlet
    {
        [Parameter(ValueFromPipeline = true, Mandatory = true, HelpMessage = "The URL of the hubsite for which to receive the sites refering to it")]
        public HubSitePipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            HubSiteProperties hubSiteProperties;
            try
            {
                hubSiteProperties = Identity.GetHubSite(Tenant);
                ClientContext.Load(hubSiteProperties, h => h.ID);
                ClientContext.ExecuteQueryRetry();
            }
            catch (ServerException ex)
            {
                if (ex.ServerErrorTypeName.Equals("System.IO.FileNotFoundException"))
                {
                    throw new ArgumentException(Resources.SiteNotFound, nameof(Identity));
                }

                throw ex;
            }

            // Get the ID of the hubsite for which we need to find child sites
            var hubSiteId = hubSiteProperties.ID;

            WriteObject(Tenant.GetHubSiteChildUrls(hubSiteId), true);
        }
    }
}
#endif
