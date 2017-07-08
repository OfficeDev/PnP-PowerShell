using System.Management.Automation;
using OfficeDevPnP.Core.Pages;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
#if !ONPREMISES
namespace SharePointPnP.PowerShell.Commands.Publishing
{
    [Cmdlet(VerbsCommon.Add, "PnPModernPage")]
    [CmdletHelp("Adds a modern page",
        Category = CmdletHelpCategory.Publishing)]
    [CmdletExample(
        Code = @"PS:> Add-PnPModernPage -PageUrl 'modernpage.aspx' ",
        Remarks = "Creates a new modern page 'modernpage.aspx'",
        SortOrder = 1)]
    public class AddModernPage : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The name page")]
        [Alias("PageName")]
        public string NamePage = string.Empty;

        protected override void ExecuteCmdlet()
        {
            var modernPage = new ClientSidePage(ClientContext);
            modernPage.Save(this.NamePage);


        }
    }
}
#endif
