#if !SP2013 && !SP2016
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using OfficeDevPnP.Core.ALM;
using System;
using SharePointPnP.PowerShell.Commands.Enums;
using OfficeDevPnP.Core.Enums;
using Microsoft.SharePoint.Client;

namespace SharePointPnP.PowerShell.Commands.Apps
{
    [Cmdlet(VerbsCommon.Get, "PnPApp")]
    [CmdletHelp("Returns the available apps from the app catalog",
        Category = CmdletHelpCategory.Apps,
        OutputType = typeof(List<AppMetadata>), SupportedPlatform = CmdletSupportedPlatform.Online | CmdletSupportedPlatform.SP2019)]
    [CmdletExample(
        Code = @"PS:> Get-PnPApp", 
        Remarks = @"This will return all available apps from the tenant app catalog. It will list the installed version in the current site.", 
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPApp -Scope Site", 
        Remarks = @"This will return all available apps from the site collection scoped app catalog. It will list the installed version in the current site.", 
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Get-PnPApp -Identity 2646ccc3-6a2b-46ef-9273-81411cbbb60f", 
        Remarks = @"This will retrieve the specific app from the app catalog.", 
        SortOrder = 3)]
    public class GetApp : PnPCmdlet
    {
        [Parameter(Mandatory = false, Position = 0, ValueFromPipeline = true, HelpMessage = "Specifies the Id of an app which is available in the app catalog")]
        public AppMetadataPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "Defines which app catalog to use. Defaults to Tenant")]
        public AppCatalogScope Scope = AppCatalogScope.Tenant;

        protected override void ExecuteCmdlet()
        {
            var additionalHeaders = GetAdditionalHeaders();
            var manager = new AppManager(ClientContext, additionalHeaders);

            if (MyInvocation.BoundParameters.ContainsKey("Identity"))
            {
                var app = Identity.GetAppMetadata(ClientContext, Scope, additionalHeaders);
                if (app != null)
                {
                    WriteObject(app);
                } else
                {
                    throw new Exception("Cannot find app");
                }
            }
            else
            {
                var apps = manager.GetAvailable(Scope);
                WriteObject(apps,true);
            }
        }
    }
}
#endif