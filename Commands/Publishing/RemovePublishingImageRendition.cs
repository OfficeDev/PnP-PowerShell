using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using Resources = PnP.PowerShell.Commands.Properties.Resources;


namespace PnP.PowerShell.Commands.Publishing
{
    [Cmdlet(VerbsCommon.Remove, "PnPPublishingImageRendition")]
    [CmdletHelp("Removes an existing image rendition", Category = CmdletHelpCategory.Publishing)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPPublishingImageRendition -Name ""MyImageRendition"" -Width 800 -Height 600",
        SortOrder = 1)]
    public class RemovePublishingImageRendition : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The display name or id of the Image Rendition.", Position = 0, ValueFromPipeline = true)]
        public ImageRenditionPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "If provided, no confirmation will be asked to remove the Image Rendition.")]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            var rendition = Identity.GetImageRendition(SelectedWeb);

            if (rendition != null)
            {
                if (Force ||
                    ShouldContinue(string.Format(Resources.RemoveImageRenditionWithName0, rendition.Name), Resources.Confirm))
                {
                    SelectedWeb.RemovePublishingImageRendition(rendition.Name);
                }
            }
        }
    }
}
