using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;

namespace PnP.PowerShell.Commands.InformationManagement
{
    [Cmdlet(VerbsCommon.Set, "PnPSitePolicy")]
    [CmdletHelp("Sets a site policy", 
        Category = CmdletHelpCategory.InformationManagement)]
    [CmdletExample(
      Code = @"PS:> Set-PnPSitePolicy -Name ""Contoso HBI""",
      Remarks = @"This applies a site policy with the name ""Contoso HBI"" to the current site. The policy needs to be available in the site.", SortOrder = 1)]
    public class ApplySitePolicy : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The name of the site policy to apply")]
        public string Name;

       
        protected override void ExecuteCmdlet()
        {
            SelectedWeb.ApplySitePolicy(Name);
        }
    }
}


