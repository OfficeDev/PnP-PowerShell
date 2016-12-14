using System.Management.Automation;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using System;
using SharePointPnP.PowerShell.Commands.Properties;

namespace SharePointPnP.PowerShell.Commands.Base
{
    [Cmdlet(VerbsCommon.Get, "PnPContext")]
    [CmdletAlias("Get-SPOContext")]
    [CmdletHelp("Returns a Client Side Object Model context",
        Category = CmdletHelpCategory.Base,
        OutputType = typeof(Microsoft.SharePoint.Client.ClientContext),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.clientcontext.aspx")]
    [CmdletExample(
        Code = @"PS:> $ctx = Get-PnPContext",
        Remarks = @"This will put the current context in the $ctx variable.",
        SortOrder = 1)]        
    public class GetSPOContext : PSCmdlet
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
            WriteObject(SPOnlineConnection.CurrentConnection.Context);
        }
    }
}
