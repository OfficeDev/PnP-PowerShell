using System.Management.Automation;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using Microsoft.SharePoint.Client;

namespace SharePointPnP.PowerShell.Commands.DocumentSets
{
    [Cmdlet(VerbsCommon.Remove, "PnPContentTypeFromDocumentSet")]
    [CmdletAlias("Remove-SPOContentTypeFromDocumentSet")]
    [CmdletHelp("Removes a content type from a document set", 
        Category = CmdletHelpCategory.DocumentSets)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPContentTypeFromDocumentSet -ContentType ""Test CT"" -DocumentSet ""Test Document Set""",
        Remarks = "This will remove the content type called 'Test CT' from the document set called ''Test Document Set'",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPContentTypeFromDocumentSet -ContentType 0x0101001F1CEFF1D4126E4CAD10F00B6137E969 -DocumentSet 0x0120D520005DB65D094035A241BAC9AF083F825F3B",
        Remarks = "This will remove the content type with ID '0x0101001F1CEFF1D4126E4CAD10F00B6137E969' from the document set with ID '0x0120D520005DB65D094035A241BAC9AF083F825F3B'",
        SortOrder = 2)]
    public class RemoveContentTypeFromDocumentSet : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The content type to remove. Either specify name, an id, or a content type object.")]
        public ContentTypePipeBind ContentType;

        [Parameter(Mandatory = true, HelpMessage = "The document set to remove the content type from. Either specify a name, a document set template object, an id, or a content type object")]
        public DocumentSetPipeBind DocumentSet;
        protected override void ExecuteCmdlet()
        {
            var ct = ContentType.GetContentType(SelectedWeb);
            var docSetTemplate = DocumentSet.GetDocumentSetTemplate(SelectedWeb);

            docSetTemplate.AllowedContentTypes.Remove(ct.Id);

            docSetTemplate.Update(true);

            ClientContext.ExecuteQueryRetry();
        }
    }
}
