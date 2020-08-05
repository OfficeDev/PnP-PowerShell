using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;

namespace PnP.PowerShell.Commands.Publishing
{
    [Cmdlet(VerbsCommon.Add, "PnPPublishingImageRendition")]
    [CmdletHelp("Adds an Image Rendition if the Name of the Image Rendition does not already exist. This prevents creating two Image Renditions that share the same name.", Category = CmdletHelpCategory.Publishing)]
    [CmdletExample(
        Code = @"PS:> Add-PnPPublishingImageRendition -Name ""MyImageRendition"" -Width 800 -Height 600",
        SortOrder = 1)]
    public class AddPublishingImageRendition : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The display name of the Image Rendition.")]
        public string Name = string.Empty;

        [Parameter(Mandatory = true, HelpMessage = "The width of the Image Rendition.")]
        public int Width = 0;
        
        [Parameter(Mandatory = true, HelpMessage = "The height of the Image Rendition.")]
        public int Height = 0;

        protected override void ExecuteCmdlet()
        {
            SelectedWeb.CreatePublishingImageRendition(Name, Width, Height);
        }
    }
}
