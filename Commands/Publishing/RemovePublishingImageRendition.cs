using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using Resources = SharePointPnP.PowerShell.Commands.Properties.Resources;


namespace SharePointPnP.PowerShell.Commands.Publishing
{
    [Cmdlet(VerbsCommon.Remove, "PnPPublishingImageRendition")]
    [CmdletHelp("Removes an existing image rendition", Category = CmdletHelpCategory.Publishing)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPPublishingImageRendition -Name ""MyImageRendition"" -Width 800 -Height 600",
        SortOrder = 1)]
    public class RemovePublishingImageRendition : SPOWebCmdlet
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
