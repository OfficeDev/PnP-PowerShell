using System;
using System.Linq.Expressions;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.DocumentSet;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.DocumentSets
{
    [Cmdlet(VerbsCommon.Get,"PnPDocumentSetTemplate")]
    [CmdletAlias("Get-SPODocumentSetTemplate")]
    [CmdletHelp("Retrieves a document set template", 
        Category = CmdletHelpCategory.DocumentSets,
        OutputType=typeof(DocumentSetTemplate),
        OutputTypeLink= "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.documentset.documentsettemplate.aspx")]
    [CmdletExample(
        Code = @"PS:> Get-PnPDocumentSetTemplate -Identity ""Test Document Set""",
        Remarks = @"This will get the document set template with the name ""Test Document Set""",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPDocumentSetTemplate -Identity ""0x0120D520005DB65D094035A241BAC9AF083F825F3B""",
        Remarks = @"This will get the document set template with the id ""0x0120D520005DB65D094035A241BAC9AF083F825F3B""",        
        SortOrder = 2)]
    public class GetDocumentSetTemplate : PnPWebRetrievalsCmdlet<DocumentSetTemplate>
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, HelpMessage = "Either specify a name, an id, a document set template object or a content type object")]
        public DocumentSetPipeBind Identity;

        protected override void ExecuteCmdlet()
        { 
            DefaultRetrievalExpressions = new Expression<Func<DocumentSetTemplate, object>>[] { t => t.AllowedContentTypes, t => t.DefaultDocuments, t => t.SharedFields, t => t.WelcomePageFields };

            var docSetTemplate = Identity.GetDocumentSetTemplate(SelectedWeb);

            ClientContext.Load(docSetTemplate, RetrievalExpressions);

            ClientContext.ExecuteQueryRetry();

            WriteObject(docSetTemplate);
        }
    }
}
