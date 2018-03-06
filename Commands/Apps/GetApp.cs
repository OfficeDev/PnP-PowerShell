#if !ONPREMISES
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using OfficeDevPnP.Core.ALM;
using System;

namespace SharePointPnP.PowerShell.Commands.Apps
{
    [Cmdlet(VerbsCommon.Get, "PnPApp")]
    [CmdletHelp("Returns the available apps from the app catalog",
        Category = CmdletHelpCategory.Apps,
        OutputType = typeof(List<AppMetadata>), SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(Code = @"PS:> Get-PnPApp", Remarks = @"This will return all available app metadata from the tenant app catalog. It will list the installed version in the current site.", SortOrder = 1)]
    [CmdletExample(Code = @"PS:> Get-PnPApp -Identity 2646ccc3-6a2b-46ef-9273-81411cbbb60f", Remarks = @"This will the specific app metadata from the app catalog.", SortOrder = 2)]
    public class GetApp : PnPCmdlet
    {
        [Parameter(Mandatory = false, Position = 0, ValueFromPipeline = true, HelpMessage = "Specifies the Id of an app which is available in the app catalog")]
        public AppMetadataPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            var manager = new AppManager(ClientContext);

            if (MyInvocation.BoundParameters.ContainsKey("Identity"))
            {
                AppMetadata app = null;
                if (Identity.Id != Guid.Empty)
                {
                    app = manager.GetAvailable(Identity.Id);
                } else if(!string.IsNullOrEmpty(Identity.Title))
                {
                    app = manager.GetAvailable(Identity.Title);
                }
                if (app != null)
                {
                    WriteObject(app);
                }
                else
                {
                    throw new System.Exception("App not found");
                }
            }
            else
            {
                var apps = manager.GetAvailable();
                WriteObject(apps,true);
            }
        }
    }
}
#endif