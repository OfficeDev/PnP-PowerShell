using System.Collections.Generic;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Get, "PnPAppInstance")]
    [CmdletAlias("Get-SPOAppInstance")]
    [CmdletHelp("Returns a SharePoint AddIn Instance in the site", 
        Category = CmdletHelpCategory.Apps,
        OutputType= typeof(List<AppInstance>),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.appinstance.aspx")]
    [CmdletExample(Code = @"PS:> Get-PnPAppInstance", Remarks = @"This will return all addin instances in the site.", SortOrder = 1)]
    [CmdletExample(Code = @"PS:> Get-PnPAppInstance -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe", Remarks = @"This will return an addin instance with the specified id.", SortOrder = 2)]
    public class GetAppInstance : SPOWebCmdlet
    {
        [Parameter(Mandatory = false, Position=0, ValueFromPipeline = true, HelpMessage = "Specifies the Id of the App Instance")]
        public GuidPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            
            if (Identity != null)
            {
                var instance = SelectedWeb.GetAppInstanceById(Identity.Id);
                ClientContext.Load(instance);
                ClientContext.ExecuteQueryRetry();
                WriteObject(instance);
            }
            else
            {
                var instances = SelectedWeb.GetAppInstances();
                if (instances.Count > 1)
                {
                    WriteObject(instances,true);
                }
                else if (instances.Count == 1)
                {
                    WriteObject(instances[0]);
                }
            }
        }
    }
}
