using System.Management.Automation;
using PnP.PowerShell.CmdletHelpAttributes;
using System;
using PnP.PowerShell.Commands.Properties;

namespace PnP.PowerShell.Commands.Base
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

            if (PnPConnection.CurrentConnection == null)
            {
                throw new InvalidOperationException(Resources.NoSharePointConnection);
            }
            if (PnPConnection.CurrentConnection.Context == null)
            {
                throw new InvalidOperationException(Resources.NoSharePointConnection);
            }
        }

        protected override void ProcessRecord()
        {
            WriteObject(PnPConnection.CurrentConnection);
        }
    }
}
