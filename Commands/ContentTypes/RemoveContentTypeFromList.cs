using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.ContentTypes
{

    [Cmdlet(VerbsCommon.Remove, "PnPContentTypeFromList")]
    [CmdletAlias("Remove-SPOContentTypeFromList")]
    [CmdletHelp("Removes a content type from a list", 
        Category = CmdletHelpCategory.ContentTypes)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPContentTypeFromList -List ""Documents"" -ContentType ""Project Document""",
        Remarks = @"This will remove a content type called ""Project Document"" from the ""Documents"" list",
        SortOrder = 1)]
    public class RemoveContentTypeFromList : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The name of the list, its ID or an actual list object from where the content type needs to be removed from")]
        public ListPipeBind List;

        [Parameter(Mandatory = true, HelpMessage = "The name of a content type, its ID or an actual content type object that needs to be removed from the specified list.")]
        public ContentTypePipeBind ContentType;

        protected override void ExecuteCmdlet()
        {
            ContentType ct = null;
            List list = List.GetList(SelectedWeb);

            if (ContentType.ContentType == null)
            {
                if (ContentType.Id != null)
                {
                    ct = SelectedWeb.GetContentTypeById(ContentType.Id,true);
                }
                else if (ContentType.Name != null)
                {
                    ct = SelectedWeb.GetContentTypeByName(ContentType.Name,true);
                }
            }
            else
            {
                ct = ContentType.ContentType;
            }
            if (ct != null)
            {
                SelectedWeb.RemoveContentTypeFromList(list, ct);
            }
        }

    }
}
