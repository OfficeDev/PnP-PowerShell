using System.Management.Automation;
using PnP.PowerShell.CmdletHelpAttributes;
using System;
using PnP.PowerShell.Commands.Properties;

namespace PnP.PowerShell.Commands.Base
{
    [Cmdlet(VerbsCommon.Get, "PnPContext")]
    [CmdletHelp("Returns the current context",
        "Returns a Client Side Object Model context",
        Category = CmdletHelpCategory.Base,
        OutputType = typeof(Microsoft.SharePoint.Client.ClientContext),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.clientcontext.aspx")]
    [CmdletExample(
        Code = @"PS:> $ctx = Get-PnPContext",
        Remarks = @"This will put the current context in the $ctx variable.",
        SortOrder = 1)]
            [CmdletExample(
        Code = @"PS:> Connect-PnPOnline -Url $siteAurl -Credentials $credentials
PS:> $ctx = Get-PnPContext
PS:> Get-PnPList # returns the lists from site specified with $siteAurl
PS:> Connect-PnPOnline -Url $siteBurl -Credentials $credentials
PS:> Get-PnPList # returns the lists from the site specified with $siteBurl
PS:> Set-PnPContext -Context $ctx # switch back to site A
PS:> Get-PnPList # returns the lists from site A", SortOrder = 2)]
    public class GetSPOContext : PSCmdlet
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
            WriteObject(PnPConnection.CurrentConnection.Context);
        }
    }
}
