using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.Apps
{
#if !ONPREMISES
    [Obsolete("Use Get-PnPApp instead")]
#endif
    [Cmdlet(VerbsCommon.Get, "PnPAppInstance")]
    [CmdletHelp("Returns a SharePoint AddIn Instance",
        "Returns a SharePoint App/Addin that has been installed in the current site",
        Category = CmdletHelpCategory.Apps,
        OutputType = typeof(List<AppInstance>),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.appinstance.aspx")]
    [CmdletExample(Code = @"PS:> Get-PnPAppInstance", Remarks = @"This will return all addin instances in the site.", SortOrder = 1)]
    [CmdletExample(Code = @"PS:> Get-PnPAppInstance -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe", Remarks = @"This will return an addin instance with the specified id.", SortOrder = 2)]
    public class GetAppInstance : PnPWebRetrievalsCmdlet<AppInstance>
    {
        [Parameter(Mandatory = false, Position=0, ValueFromPipeline = true, HelpMessage = "Specifies the Id of the App Instance")]
        public AppPipeBind Identity;

        protected override void ExecuteCmdlet()
        {

            if (Identity != null)
            {
                var instance = Identity.GetAppInstance(SelectedWeb);
                WriteObject(instance);
            }
            else
            {
                var instances = SelectedWeb.GetAppInstances();
                if (instances.Count > 1)
                {
                    WriteObject(instances, true);
                }
                else if (instances.Count == 1)
                {
                    WriteObject(instances[0]);
                }
            }
        }
    }
}
