#if !ONPREMISES
using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using SharePointPnP.PowerShell.Commands.Enums;
using Resources = SharePointPnP.PowerShell.Commands.Properties.Resources;

namespace SharePointPnP.PowerShell.Commands
{

    [Cmdlet(VerbsCommon.Get, "PnPTenantSite", SupportsShouldProcess = true)]
    [CmdletAlias("Get-SPOTenantSite")]
    [CmdletHelp(@"Office365 only: Uses the tenant API to retrieve site information.", 
        Category = CmdletHelpCategory.TenantAdmin,
        OutputType = typeof(Microsoft.Online.SharePoint.TenantAdministration.SiteProperties),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.online.sharepoint.tenantadministration.siteproperties.aspx")]
    [CmdletExample(Code = @"PS:> Get-PnPTenantSite", Remarks = "Returns all site collections", SortOrder = 1)]
    [CmdletExample(Code = @"PS:> Get-PnPTenantSite -Url http://tenant.sharepoint.com/sites/projects", Remarks = "Returns information about the project site.",SortOrder = 2)]
    [CmdletExample(Code = @"PS:> Get-PnPTenantSite -Detailed", Remarks = "Returns all sites with the full details of these sites", SortOrder = 3)]
    [CmdletExample(Code = @"PS:> Get-PnPTenantSite -IncludeOneDriveSites", Remarks = "Returns all sites including all OneDrive 4 Business sites", SortOrder = 4)]
    public class GetTenantSite : PnPAdminCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "The URL of the site", Position = 0, ValueFromPipeline = true)]
        [Alias("Identity")]
        public string Url;

        [Parameter(Mandatory = false, HelpMessage = "By default, all sites will be return. Specify a template value alike 'STS#0' here to filter on the template")]
        public string Template;

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
                  
                    Tenant.Context.Load(list);
                    Tenant.Context.ExecuteQueryRetry();
                    var siteProperties = list.ToList();
                    var returnedEntries = list.Count;

                    var startIndex = 0;
                    while (returnedEntries > 299)
                    {
                        startIndex = startIndex + 300;
                        var nextList = Tenant.GetSiteProperties(startIndex, Detailed);
                        Tenant.Context.Load(nextList);
                        Tenant.Context.ExecuteQueryRetry();
                        siteProperties.AddRange(nextList);
                        returnedEntries = nextList.Count;
                    }

                    
                    
                    if (IncludeOneDriveSites)
                    {
                        if (Force || ShouldContinue(Resources.GetTenantSite_ExecuteCmdlet_This_request_can_take_a_long_time_to_execute__Continue_, Resources.Confirm))
                        {
                            var onedriveSites = Tenant.GetOneDriveSiteCollections();

                            var personalUrl = ClientContext.Url.ToLower().Replace("-admin", "-my");
                            foreach (var site in onedriveSites)
                            {
                                var siteprops = Tenant.GetSitePropertiesByUrl($"{personalUrl.TrimEnd('/')}/{site.Url.Trim('/')}", Detailed);
                                ClientContext.Load(siteprops);
                                ClientContext.ExecuteQueryRetry();
                                siteProperties.Add(siteprops);
                            }
                        }
                    }
                    if (Template != null)
                    {
                        WriteObject(siteProperties.Where(t => t.Template == Template).OrderBy(x => x.Url), true);
                    }
                    else
                    {
                        WriteObject(siteProperties.OrderBy(x => x.Url), true);
                    }
                }
            }
        }
    }
}
#endif
