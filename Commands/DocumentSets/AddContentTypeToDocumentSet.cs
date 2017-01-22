using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.DocumentSets
{
    [Cmdlet(VerbsCommon.Add,"PnPContentTypeToDocumentSet")]
    [CmdletAlias("Add-SPOContentTypeToDocumentSet")]
    [CmdletHelp("Adds a content type to a document set", 
        Category = CmdletHelpCategory.DocumentSets)]
    [CmdletExample(
        Code = @"PS:> Add-PnPContentTypeToDocumentSet -ContentType ""Test CT"" -DocumentSet ""Test Document Set""",
        Remarks = "This will add the content type called 'Test CT' to the document set called ''Test Document Set'",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> $docset = Get-PnPDocumentSetTemplate -Identity ""Test Document Set""
PS:> $ct = Get-SPOContentType -Identity ""Test CT""
PS:> Add-PnPContentTypeToDocumentSet -ContentType $ct -DocumentSet $docset",
        Remarks = "This will add the content type called 'Test CT' to the document set called ''Test Document Set'",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Add-PnPContentTypeToDocumentSet -ContentType 0x0101001F1CEFF1D4126E4CAD10F00B6137E969 -DocumentSet 0x0120D520005DB65D094035A241BAC9AF083F825F3B",
        Remarks = "This will add the content type called 'Test CT' to the document set called ''Test Document Set'",
        SortOrder = 3)]
    public class AddContentTypeToDocumentSet : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The content type object, name or id to add. Either specify name, an id, or a content type object.")]
        public ContentTypePipeBind[] ContentType;

        [Parameter(Mandatory = true, HelpMessage = "The document set object or id to add the content type to. Either specify a name, a document set template object, an id, or a content type object")]
        public DocumentSetPipeBind DocumentSet;

        protected override void ExecuteCmdlet()
        {
            var docSetTemplate = DocumentSet.GetDocumentSetTemplate(SelectedWeb);

            foreach (var ct in ContentType)
            {
                var contentType = ct.GetContentType(SelectedWeb);

                docSetTemplate.AllowedContentTypes.Add(contentType.Id);
            }
            docSetTemplate.Update(true);
            ClientContext.ExecuteQueryRetry();
        }
    }
}
