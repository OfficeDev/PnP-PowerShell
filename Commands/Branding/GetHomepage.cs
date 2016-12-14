using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands.Branding
{
    [Cmdlet(VerbsCommon.Get, "PnPHomePage")]
    [CmdletAlias("Get-SPOHomePage")]
    [CmdletHelp("Returns the URL to the home page", 
        Category = CmdletHelpCategory.Branding,
        OutputType = typeof(string))]
    [CmdletExample(Code = @"PS:> Get-PnPHomePage",
        Remarks = "Will return the URL of the home page of the web.",
        SortOrder = 1)]
    public class GetHomePage : SPOWebCmdlet
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
