using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.DocumentSet;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.DocumentSets
{
    [Cmdlet(VerbsCommon.Add, "PnPDocumentSet")]
    [CmdletAlias("Add-SPODocumentSet")]
    [CmdletHelp("Creates a new document set in a library.",
      Category = CmdletHelpCategory.DocumentSets,
        OutputType=typeof(string))]
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

        [Parameter(Mandatory = true, HelpMessage = "The name of the content type, its ID or an actual content object referencing to the document set.")] 
        public ContentTypePipeBind ContentType;
        protected override void ExecuteCmdlet()
        {
            var list = List.GetList(SelectedWeb);
            list.EnsureProperty(l => l.RootFolder);

            var result = DocumentSet.Create(ClientContext, list.RootFolder, Name, ContentType.GetContentType(SelectedWeb).Id);

            ClientContext.ExecuteQueryRetry();

            WriteObject(result.Value);
        }
    }
}
