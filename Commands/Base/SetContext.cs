using System.Management.Automation;
using PnP.PowerShell.CmdletHelpAttributes;
using Microsoft.SharePoint.Client;

namespace PnP.PowerShell.Commands.Base
{
    [Cmdlet(VerbsCommon.Set, "PnPContext")]
    [CmdletHelp("Set the ClientContext",
        "Sets the Client Context to use by the cmdlets, which allows easy context switching. See examples for details.",
        Category = CmdletHelpCategory.Base)]
    [CmdletExample(
        Code = @"PS:> Connect-PnPOnline -Url $siteAurl -Credentials $credentials
PS:> $ctx = Get-PnPContext
PS:> Get-PnPList # returns the lists from site specified with $siteAurl
PS:> Connect-PnPOnline -Url $siteBurl -Credentials $credentials
PS:> Get-PnPList # returns the lists from the site specified with $siteBurl
PS:> Set-PnPContext -Context $ctx # switch back to site A
PS:> Get-PnPList # returns the lists from site A", SortOrder = 1)]
    public class SetContext : PSCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 1, HelpMessage = "The ClientContext to set")]
        public ClientContext Context;

        protected override void ProcessRecord()
        {
            PnPConnection.CurrentConnection.Context = Context;
        }
    }
}
