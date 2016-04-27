#if !CLIENTSDKV15
using System.ComponentModel;
using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.PowerShell.CmdletHelpAttributes;
using OfficeDevPnP.PowerShell.Commands.Base;
using OfficeDevPnP.PowerShell.Commands.Enums;
using Resources = OfficeDevPnP.PowerShell.Commands.Properties.Resources;

namespace OfficeDevPnP.PowerShell.Commands
{

    [Cmdlet(VerbsCommon.Get, "SPOTenantSite", SupportsShouldProcess = true)]
    [CmdletHelp(@"Office365 only: Uses the tenant API to retrieve site information.", Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(Code = @"PS:> Get-SPOTenantSite", Remarks = "Returns all site collections", SortOrder = 1)]
    [CmdletExample(Code = @"PS:> Get-SPOTenantSite -Url http://tenant.sharepoint.com/sites/projects", Remarks = "Returns information about the project site.",SortOrder = 2)]
    [CmdletExample(Code = @"PS:> Get-SPOTenantSite -Detailed", Remarks = "Returns all sites iwith the full details of these sites", SortOrder = 3)]
    [CmdletExample(Code = @"PS:> Get-SPOTenantSite -IncludeOneDriveSites", Remarks = "Returns all sites including all OneDrive 4 Business sites", SortOrder = 4)]
    public class GetTenantSite : SPOAdminCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "The URL of the site", Position = 0, ValueFromPipeline = true)]
        [Alias("Identity")]
        public string Url;

        [Parameter(Mandatory = false, HelpMessage = "By default, not all returned attributes are populated. This switch populates all attributes. It can take several seconds to run. Without this, some attributes will show default values that may not be correct.")]
        public SwitchParameter Detailed;

        [Parameter(Mandatory = false, HelpMessage = "By default, the OneDrives are not returned. This switch includes all OneDrives. This can take some extra time to run")]
        public SwitchParameter IncludeOneDriveSites;

        [Parameter(Mandatory = false, HelpMessage = "When the switch IncludeOneDriveSites is used, this switch ignores the question shown that the command can take a long time to execute")]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            if (SPOnlineConnection.CurrentConnection.ConnectionType == ConnectionType.OnPrem)
            {
                WriteObject(ClientContext.Site);
            }
            else
            {
                if (!string.IsNullOrEmpty(Url))
                {
                    var list = Tenant.GetSitePropertiesByUrl(Url, Detailed);
                    list.Context.Load(list);
                    list.Context.ExecuteQueryRetry();
                    WriteObject(list, true);
                }
                else
                {
                    var list = Tenant.GetSiteProperties(0, Detailed);
                    list.Context.Load(list);
                    list.Context.ExecuteQueryRetry();
                    var siteProperties = list.ToList();
                    if (IncludeOneDriveSites)
                    {
                        if (Force || ShouldContinue(Resources.GetTenantSite_ExecuteCmdlet_This_request_can_take_a_long_time_to_execute__Continue_, Resources.Confirm))
                        {
                            var onedriveSites = Tenant.GetOneDriveSiteCollections();

                            var personalUrl = ClientContext.Url.ToLower().Replace("-admin", "-my");
                            foreach (var site in onedriveSites)
                            {
                                var siteprops = Tenant.GetSitePropertiesByUrl(string.Format("{0}/{1}", personalUrl.TrimEnd('/'), site.Url.Trim('/')), Detailed);
                                ClientContext.Load(siteprops);
                                ClientContext.ExecuteQueryRetry();
                                siteProperties.Add(siteprops);
                            }
                        }
                    }
                    WriteObject(siteProperties.OrderBy(x => x.Url), true);
                }
            }
        }
    }

}
#endif
