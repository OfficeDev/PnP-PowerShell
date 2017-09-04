#if !ONPREMISES
using System.Linq;
using System.Management.Automation;
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using SharePointPnP.PowerShell.Commands.Enums;

namespace SharePointPnP.PowerShell.Commands
{

    [Cmdlet(VerbsCommon.Get, "PnPTenantSite", SupportsShouldProcess = true)]
    [CmdletHelp(@"Uses the tenant API to retrieve site information.", 
        Category = CmdletHelpCategory.TenantAdmin,
        SupportedPlatform = CmdletSupportedPlatform.Online,
        OutputType = typeof(Microsoft.Online.SharePoint.TenantAdministration.SiteProperties),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.online.sharepoint.tenantadministration.siteproperties.aspx")]
    [CmdletExample(Code = @"PS:> Get-PnPTenantSite", Remarks = "Returns all site collections", SortOrder = 1)]
    [CmdletExample(Code = @"PS:> Get-PnPTenantSite -Url http://tenant.sharepoint.com/sites/projects", Remarks = "Returns information about the project site.", SortOrder = 2)]
    [CmdletExample(Code = @"PS:> Get-PnPTenantSite -Detailed", Remarks = "Returns all sites with the full details of these sites", SortOrder = 3)]
    [CmdletExample(Code = @"PS:> Get-PnPTenantSite -IncludeOneDriveSites", Remarks = "Returns all sites including all OneDrive 4 Business sites", SortOrder = 4)]
    [CmdletExample(Code = @"PS:> Get-PnPTenantSite -IncludeOneDriveSites -Filter ""Url -like '-my.sharepoint.com/personal/'""", Remarks = "Returns all OneDrive for Business sites.", SortOrder = 5)]
    [CmdletExample(Code = @"PS:> Get-PnPTenantSite -WebTemplate SITEPAGEPUBLISHING#0", Remarks = "Returns all Communication sites", SortOrder = 6)]
    [CmdletExample(Code = @"PS:> Get-PnPTenantSite -Filter ""Url -like 'sales'"" ", Remarks = "Returns all sites including 'sales' in the url.", SortOrder = 7)]
    public class GetTenantSite : PnPAdminCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "The URL of the site", Position = 0, ValueFromPipeline = true)]
        [Alias("Identity")]
        public string Url;

        [Parameter(Mandatory = false, HelpMessage = "By default, all sites will be return. Specify a template value alike 'STS#0' here to filter on the template")]
        public string Template;

        [Parameter(Mandatory = false, HelpMessage = "By default, not all returned attributes are populated. This switch populates all attributes. It can take several seconds to run. Without this, some attributes will show default values that may not be correct.")]
        public SwitchParameter Detailed;

        [Parameter(Mandatory = false, HelpMessage = "By default, the OneDrives are not returned. This switch includes all OneDrives.")]
        public SwitchParameter IncludeOneDriveSites;

        [Parameter(Mandatory = false, HelpMessage = "When the switch IncludeOneDriveSites is used, this switch ignores the question shown that the command can take a long time to execute")]
        public SwitchParameter Force;

        [Parameter(Mandatory = false, HelpMessage = "Limit results to a specific web template name.")]
        public string WebTemplate;

        [Parameter(Mandatory = false, HelpMessage = "Specifies the script block of the server-side filter to apply. See https://technet.microsoft.com/en-us/library/fp161380.aspx")]
        public string Filter;

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
                    SPOSitePropertiesEnumerableFilter filter = new SPOSitePropertiesEnumerableFilter()
                    {
                        IncludePersonalSite = IncludeOneDriveSites.IsPresent ? PersonalSiteFilter.Include : PersonalSiteFilter.UseServerDefault,
                        StartIndex = null,
                        IncludeDetail = true,
                        Template = WebTemplate,
                        Filter = Filter,
                    };

                    var list = Tenant.GetSitePropertiesFromSharePointByFilters(filter);

                    Tenant.Context.Load(list);
                    Tenant.Context.ExecuteQueryRetry();
                    var siteProperties = list.ToList();

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
