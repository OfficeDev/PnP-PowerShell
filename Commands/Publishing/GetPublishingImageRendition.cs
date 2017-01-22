using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Publishing;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.Publishing
{
    [Cmdlet(VerbsCommon.Get, "PnPPublishingImageRendition")]
    [CmdletHelp("Returns all image renditions or if Identity is specified a specific one",
        Category = CmdletHelpCategory.Publishing,
        OutputType = typeof(ImageRendition),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.publishing.imagerendition.aspx")]
    [CmdletExample(
        Code = @"PS:> Get-PnPPublishingImageRendition",
        Remarks = @"Returns all Image Renditions",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPPublishingImageRendition -Identity ""Test""",
        Remarks = @"Returns the image rendition named ""Test""",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Get-PnPPublishingImageRendition -Identity 2",
        Remarks = @"Returns the image rendition where its id equals 2",
        SortOrder = 3)]
    public class GetPublishingImageRendition : PnPWebCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "Id or name of an existing image rendition", Position = 0, ValueFromPipeline = true)]
        public ImageRenditionPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            if (MyInvocation.BoundParameters.ContainsKey("Identity"))
            {
                WriteObject(Identity.GetImageRendition(SelectedWeb));
            }
            else
            {
                WriteObject(SelectedWeb.GetPublishingImageRenditions(), true);
            }
        }
    }
}
