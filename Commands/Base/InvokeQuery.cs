using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands.Base
{
    
    [Cmdlet(VerbsLifecycle.Invoke,"PnPQuery")]
    [Alias("Execute-PnPQuery")]
    [CmdletHelp("Executes the currently queued actions", 
        "Executes any queued actions / changes on the SharePoint Client Side Object Model Context",
        Category = CmdletHelpCategory.Base)]
    [CmdletExample(
      Code = @"PS:> Invoke-PnPQuery -RetryCount 5",
      Remarks = @"This will execute any queued actions / changes on the SharePoint Client Side Object Model Context and will retry 5 times in case of throttling.", SortOrder = 1)]
    [CmdletExample(
      Code = @"PS:> Invoke-PnPQuery -RetryWait 10",
      Remarks = @"This will execute any queued actions / changes on the SharePoint Client Side Object Model Context and delay the execution for 10 seconds before it retries the execution.", SortOrder = 2)]

    public class InvokeQuery : PnPCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "Number of times to retry in case of throttling. Defaults to 10.")]
        public int RetryCount = 10;

        [Parameter(Mandatory = false, HelpMessage = "Delay in seconds. Defaults to 1.")]
        public int RetryWait = 1;

        protected override void ProcessRecord()
        {
            if(MyInvocation.InvocationName.ToLower() == "execute-pnpquery")
            {
                WriteWarning("Execute-PnPQuery has been deprecated. Use Invoke-PnPQuery instead.");
            }
            ClientContext.ExecuteQueryRetry(RetryCount, RetryWait);
        }
    }
}
