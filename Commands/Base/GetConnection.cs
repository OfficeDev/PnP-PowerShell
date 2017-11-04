using System.Management.Automation;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using System;
using SharePointPnP.PowerShell.Commands.Properties;

namespace SharePointPnP.PowerShell.Commands.Base
{
    [Cmdlet(VerbsCommon.Get, "PnPConnection")]
    [CmdletHelp("Returns the current context",
        "Returns a PnP PowerShell Connection for use with the -Connection parameter on other cmdlets.",
        Category = CmdletHelpCategory.Base)]
    [CmdletExample(
        Code = @"PS:> $ctx = Get-PnPConnection",
        Remarks = @"This will put the current connection for use with the -Connection parameter on other cmdlets.",
        SortOrder = 1)]
    public class GetPnPConnection : PSCmdlet
    {

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            if (SPOnlineConnection.CurrentConnection == null)
            {
                throw new InvalidOperationException(Resources.NoConnection);
            }
            if (SPOnlineConnection.CurrentConnection.Context == null)
            {
                throw new InvalidOperationException(Resources.NoConnection);
            }
        }

        protected override void ProcessRecord()
        {
            WriteObject(SPOnlineConnection.CurrentConnection);
        }
    }
}
