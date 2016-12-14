using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands.Base
{
    [Cmdlet("Execute", "PnPQuery")]
    [CmdletAlias("Execute-SPOQuery")]
    [CmdletHelp("Executes any queued actions / changes on the SharePoint Client Side Object Model Context",
        Category = CmdletHelpCategory.Base)]
    [CmdletExample(
      Code = @"PS:> Execute-PnPQuery -RetryCount 5",
      Remarks = @"This will execute any queued actions / changes on the SharePoint Client Side Object Model Context and will retry 5 times in case of throttling.", SortOrder = 1)]
    [CmdletExample(
      Code = @"PS:> Execute-PnPQuery -RetryWait 10",
      Remarks = @"This will execute any queued actions / changes on the SharePoint Client Side Object Model Context and delay the execution for 10 seconds before it retries the execution.", SortOrder = 2)]

    public class ExecuteSPOQuery : SPOCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "Number of times to retry in case of throttling. Defaults to 10.")]
        public int RetryCount = 10;

        [Parameter(Mandatory = false, HelpMessage = "Delay in seconds. Defaults to 1.")]
        public int RetryWait = 1;

        protected override void ProcessRecord()
        {
            ClientContext.ExecuteQueryRetry(RetryCount,RetryWait);
        }
    }
}
