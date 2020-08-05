using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;

namespace PnP.PowerShell.Commands.Branding
{
    [Cmdlet(VerbsCommon.Get, "PnPHomePage")]
    [CmdletHelp("Return the homepage",
        "Returns the URL to the page set as home page", 
        Category = CmdletHelpCategory.Branding,
        OutputType = typeof(string))]
    [CmdletExample(Code = @"PS:> Get-PnPHomePage",
        Remarks = "Will return the URL of the home page of the web.",
        SortOrder = 1)]
    public class GetHomePage : PnPWebCmdlet
    {
        protected override void ExecuteCmdlet()
        {
            var folder = SelectedWeb.RootFolder;

            ClientContext.Load(folder, f => f.WelcomePage);

            ClientContext.ExecuteQueryRetry();

            if (string.IsNullOrEmpty(folder.WelcomePage))
            {
                WriteObject("default.aspx");
            }
            else
            {
                WriteObject(folder.WelcomePage);
            }
        }
    }
}
