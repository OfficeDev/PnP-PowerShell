using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.ContentTypes
{

    [Cmdlet(VerbsCommon.Set, "PnPDefaultContentTypeToList")]
    [CmdletHelp("Sets the default content type for a list", 
        Category = CmdletHelpCategory.ContentTypes)]
    [CmdletExample(
        Code = @"PS:> Set-PnPDefaultContentTypeToList -List ""Project Documents"" -ContentType ""Project""",
        Remarks = @"This will set the Project content type (which has already been added to a list) as the default content type", 
        SortOrder = 1)]
    public class SetDefaultContentTypeToList : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The name of a list, an ID or the actual list object to update")]
        public ListPipeBind List;

        [Parameter(Mandatory = true, HelpMessage = "The content type object that needs to be set as the default content type on the list. Content Type needs to be present on the list.")]
        public ContentTypePipeBind ContentType;

        protected override void ExecuteCmdlet()
        {
            ContentType ct = null;
            List list = List.GetList(SelectedWeb);

            if (ContentType.ContentType == null)
            {
                if (ContentType.Id != null)
                {
                    ct = list.GetContentTypeById(ContentType.Id);
                }
                else if (ContentType.Name != null)
                {
                    ct = list.GetContentTypeByName(ContentType.Name);
                }
            }
            else
            {
                ct = ContentType.ContentType;
                if(!list.ContentTypeExistsById(ct.Id.ToString()))
                {
                    WriteError(new ErrorRecord(new System.Exception("Content type does not exist on list. Use Add-PnPContentTypeToList to add the content type first."), "CONTENTTYPENOTADDEDTOLIST", ErrorCategory.ResourceUnavailable, ct));
                }
            }
            if (ct != null)
            {
                list.SetDefaultContentType(ct.Id);
            }
        }

    }
}
