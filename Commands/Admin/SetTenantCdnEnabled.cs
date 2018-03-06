#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using System.Management.Automation;
using OfficeDevPnP.Core.Sites;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System;
using SharePointPnP.PowerShell.Commands.Enums;
using System.Collections.Generic;

namespace SharePointPnP.PowerShell.Commands.Admin
{
    [Cmdlet(VerbsCommon.Set, "PnPTenantCdnEnabled")]
    [CmdletHelp("Enables or disabled the public or private Office 365 Content Delivery Network (CDN).",
        DetailedDescription = @"Enables or disabled the public or private Office 365 Content Delivery Network (CDN).",
        SupportedPlatform = CmdletSupportedPlatform.Online,
        Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(
        Code = @"PS:> Set-PnPTenantCdnEnabled -CdnType Public -Enable $true",
        Remarks = @"This example sets the Public CDN enabled.", SortOrder = 1)]
    public class SetTenantCdnEnabled : PnPAdminCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Specify to enable or disable")]
        public bool Enable;

        [Parameter(Mandatory = true, HelpMessage = "The type of cdn to enable or disable")]
        public CdnType CdnType;

        [Parameter(Mandatory = false)]
        public SwitchParameter NoDefaultOrigins { get; set; }

        protected override void ExecuteCmdlet()
        {
            bool privateFlag = CdnType == CdnType.Both || CdnType == CdnType.Private;
            bool publicFlag= CdnType == CdnType.Both || CdnType == CdnType.Public;

            if (privateFlag)
            {
                Tenant.SetTenantCdnEnabled(SPOTenantCdnType.Private, Enable);
            }
            if(publicFlag)
            {
                Tenant.SetTenantCdnEnabled(SPOTenantCdnType.Public, Enable);
            }
            if (this.Enable && !this.NoDefaultOrigins)
            {
                if (privateFlag)
                {
                    Tenant.CreateTenantCdnDefaultOrigins(SPOTenantCdnType.Private);
                }
                if (publicFlag)
                {
                    Tenant.CreateTenantCdnDefaultOrigins(SPOTenantCdnType.Public);
                }
            }
            ClientContext.ExecuteQueryRetry();
        }
    }
}
#endif