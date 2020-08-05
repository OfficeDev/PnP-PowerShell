#if !ONPREMISES
using OfficeDevPnP.Core.Sites;
using PnP.PowerShell.CmdletHelpAttributes;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Site
{
    [Cmdlet(VerbsCommon.Set, "PnPTeamifyPromptHidden")]
    [CmdletHelp("Hides the teamify prompt on a modern Team site.",
        DetailedDescription = "This command allows to hide the teamify prompt which shows in the navigation of a newly created Microsoft 365 group connected team site.",
        SupportedPlatform = CmdletSupportedPlatform.Online,
        Category = CmdletHelpCategory.Sites)]
    [CmdletExample(
        Code = @"PS:> Set-PnPTeamifyPromptHidden",
        Remarks = @"This hides the teamify prompt for the current site.", SortOrder = 1)]
    public class SetTeamifyPromptHidden : PnPSharePointCmdlet
    {

        protected override void ExecuteCmdlet()
        {
            var hidden = SiteCollection.IsTeamifyPromptHiddenAsync(ClientContext).GetAwaiter().GetResult();
            if (!hidden)
            {
                SiteCollection.HideTeamifyPromptAsync(ClientContext).GetAwaiter().GetResult();

            } else
            {
                WriteWarning("Teamify prompt was already hidden");  
            }
            
        }
    }
}
#endif