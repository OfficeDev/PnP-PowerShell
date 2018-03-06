using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.ContentTypes
{
    [Cmdlet(VerbsCommon.Add, "PnPContentTypeToList")]
    [CmdletHelp("Adds a new content type to a list", 
        Category = CmdletHelpCategory.ContentTypes)]
    [CmdletExample(
        Code = @"PS:> Add-PnPContentTypeToList -List ""Documents"" -ContentType ""Project Document"" -DefaultContentType",
        Remarks = @"This will add an existing content type to a list and sets it as the default content type", 
        SortOrder = 1)]
    public class AddContentTypeToList : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Specifies the list to which the content type needs to be added")]
        public ListPipeBind List;

        [Parameter(Mandatory = true, HelpMessage = "Specifies the content type that needs to be added to the list")]
        public ContentTypePipeBind ContentType;

        [Parameter(Mandatory = false, HelpMessage = "Specify if the content type needs to be the default content type or not")]
        public SwitchParameter DefaultContentType;

        protected override void ExecuteCmdlet()
        {
            ContentType ct = null;
            List list = List.GetList(SelectedWeb);

            if (ContentType.ContentType == null)
            {
                if (ContentType.Id != null)
                {
                    ct = SelectedWeb.GetContentTypeById(ContentType.Id, true);
                }
                else if (ContentType.Name != null)
                {
                    ct = SelectedWeb.GetContentTypeByName(ContentType.Name, true);
                }
            }
            else
            {
                ct = ContentType.ContentType;
            }
            if (ct != null)
            {
                SelectedWeb.AddContentTypeToList(list.Title, ct, DefaultContentType);
            }
        }

    }
}
