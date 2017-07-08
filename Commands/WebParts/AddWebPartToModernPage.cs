using System.Linq;
using System.Management.Automation;
using OfficeDevPnP.Core.Pages;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
#if !ONPREMISES
namespace SharePointPnP.PowerShell.Commands.WebParts
{
    [Cmdlet(VerbsCommon.Add, "PnPWebPartToModernPage")]
    [CmdletHelp("Adds a webpart to a Modern Page in a specified zone",
        Category = CmdletHelpCategory.WebParts)]
    [CmdletExample(
        Code =
            @"PS:> Add-PnPWebPartToModernPage -ServerRelativePageUrl ""/sites/demo/sitepages/home.aspx"" -NameWebPart ""HelloWorld""  -ZoneIndex 1 ",
        SortOrder = 1)]
    public class AddWebPartToModernPage : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Server Relative Url of the page to add the webpart to.")]
        [Alias("PageUrl")] public string ServerRelativePageUrl = string.Empty;

        [Parameter(Mandatory = true, HelpMessage = "A name for the webpart.")] [Alias("WebPart")]
        public string NameWebPart = string.Empty;

        [Parameter(Mandatory = true)] public int ZoneIndex;

        protected override void ExecuteCmdlet()
        {
            var page = ClientSidePage.Load(ClientContext, ServerRelativePageUrl);

            var components = page.AvailableClientSideComponents();
            var myWebPart = components.FirstOrDefault(s => s.ComponentType == 1 && s.Name == NameWebPart);
            if (myWebPart !=null)
            {
                var webPart= new ClientSideWebPart(myWebPart) {Order = ZoneIndex};
                page.AddControl(webPart);
            }
            page.Save();
        }

    }
}
#endif