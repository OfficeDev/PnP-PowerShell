using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.DocumentSet;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System.Linq;

namespace PnP.PowerShell.Commands.DocumentSets
{
    [Cmdlet(VerbsCommon.Add, "PnPDocumentSet")]
    [CmdletHelp("Creates a new document set in a library.",
      Category = CmdletHelpCategory.DocumentSets,
        OutputType = typeof(string))]
    [CmdletExample(
      Code = @"PS:> Add-PnPDocumentSet -List ""Documents"" -ContentType ""Test Document Set"" -Name ""Test""",
      Remarks = "This will add a new document set based upon the 'Test Document Set' content type to a list called 'Documents'. The document set will be named 'Test'",
      SortOrder = 1)]
    public class AddDocumentSet : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The name of the list, its ID or an actual list object from where the document set needs to be added")]
        public ListPipeBind List;

        [Parameter(Mandatory = true, HelpMessage = "The name of the document set")]
        public string Name;

        [Parameter(Mandatory = true, HelpMessage = "The name of the content type, its ID or an actual content object referencing to the document set")]
        public ContentTypePipeBind ContentType;

        protected override void ExecuteCmdlet()
        {
            var list = List.GetList(SelectedWeb);
            if (list == null)
                throw new PSArgumentException($"No list found with id, title or url '{List}'", "List");
            list.EnsureProperties(l => l.RootFolder, l => l.ContentTypes);

            // Try getting the content type from the Web
            var contentType = ContentType.GetContentType(SelectedWeb);

            // If content type could not be found but a content type ID has been passed in, try looking for the content type ID on the list instead
            if (contentType == null && !string.IsNullOrEmpty(ContentType.Id))
            {
                contentType = list.ContentTypes.FirstOrDefault(ct => ct.StringId.Equals(ContentType.Id));
            }
            else
            {
                // Content type has been found on the web, check if it also exists on the list
                if (list.ContentTypes.All(ct => !ct.StringId.Equals(contentType.Id.StringValue, System.StringComparison.InvariantCultureIgnoreCase)))
                {
                    contentType = list.ContentTypes.FirstOrDefault(ct => ct.Name.Equals(ContentType.Name));
                }
            }

            if (contentType == null)
            {
                throw new PSArgumentException("The provided contenttype has not been found", "ContentType");
            }

            // Create the document set
            var result = DocumentSet.Create(ClientContext, list.RootFolder, Name, contentType.Id);
            ClientContext.ExecuteQueryRetry();

            WriteObject(result.Value);
        }
    }
}