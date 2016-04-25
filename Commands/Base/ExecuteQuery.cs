using System.Management.Automation;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.PowerShell.CmdletHelpAttributes;

namespace OfficeDevPnP.PowerShell.Commands.Base
{
    [Cmdlet("Execute", "SPOQuery")]
    [CmdletHelp("Executes any queued actions / changes on the SharePoint Client Side Object Model Context",
        Category = CmdletHelpCategory.Base)]

    public class ExecuteSPOQuery : SPOCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "Number to times to retry in case of throttling. Defaults to 10.")]
        public int RetryCount = 10;

        [Parameter(Mandatory = false, HelpMessage = "Delay in seconds. Defaults to 1.")]
        public int RetryWait = 1;

        protected override void ProcessRecord()
        {
            ClientContext.ExecuteQueryRetry(RetryCount,RetryWait);
        }
    }
}
