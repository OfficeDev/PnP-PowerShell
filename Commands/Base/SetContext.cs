using System.Management.Automation;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using Microsoft.SharePoint.Client;

namespace SharePointPnP.PowerShell.Commands.Base
{
    [Cmdlet(VerbsCommon.Set, "PnPContext")]
    [CmdletAlias("Set-SPOContext")]
    [CmdletHelp("Sets the Client Context to use by the cmdlets",
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
            SPOnlineConnection.CurrentConnection.Context = Context;
        }
    }
}
