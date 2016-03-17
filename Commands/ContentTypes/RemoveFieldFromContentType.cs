using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.PowerShell.CmdletHelpAttributes;
using OfficeDevPnP.PowerShell.Commands.Base.PipeBinds;
using System.Linq;

namespace OfficeDevPnP.PowerShell.Commands
{

    [Cmdlet(VerbsCommon.Remove, "SPOFieldFromContentType")]
    [CmdletHelp("Removes a site column from a content type", 
        Category = CmdletHelpCategory.ContentTypes)]
    [CmdletExample(
     Code = @"PS:> Remove-SPOFieldFromContentType -Field ""Project_Name"" -ContentType ""Project Document""",
     Remarks = @"This will remove the site column with an internal name of ""Project_Name"" from a content type called ""Project Document""", SortOrder = 1)]
    [CmdletExample(
     Code = @"PS:> Remove-SPOFieldFromContentType -Field ""Project_Name"" -ContentType ""Project Document"" -DoNotUpdateChildren",
     Remarks = @"This will remove the site column with an internal name of ""Project_Name"" from a content type called ""Project Document"". It will not update content types that inherit from the ""Project Document"" content type.", SortOrder = 1)]
    public class RemoveFieldFromContentType : SPOWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The field to remove.")]
        public FieldPipeBind Field;

        [Parameter(Mandatory = true, HelpMessage = "The content type where the field is to be removed from.")]
        public ContentTypePipeBind ContentType;

        [Parameter(Mandatory = false, HelpMessage = "If specified, inherited content types will not be updated.")]
        public SwitchParameter DoNotUpdateChildren;

        protected override void ExecuteCmdlet()
        {
            Field field = Field.Field;
            if (field == null)
            {
                if (Field.Id != Guid.Empty)
                {
                    field = SelectedWeb.Fields.GetById(Field.Id);
                }
                else if (!string.IsNullOrEmpty(Field.Name))
                {
                    field = SelectedWeb.Fields.GetByInternalNameOrTitle(Field.Name);
                }
                ClientContext.Load(field);
                ClientContext.ExecuteQueryRetry();
            }
            if (field != null)
            {
                if (ContentType.ContentType != null)
                {
                    ContentType.ContentType.EnsureProperty(c => c.FieldLinks);
                    var fieldLink = ContentType.ContentType.FieldLinks.FirstOrDefault(f => f.Id == field.Id);
                    fieldLink.DeleteObject();
                    ContentType.ContentType.Update(!DoNotUpdateChildren);
                    ClientContext.ExecuteQueryRetry();
                }
                else
                {
                    ContentType ct;
                    if (!string.IsNullOrEmpty(ContentType.Id))
                    {
                        ct = SelectedWeb.GetContentTypeById(ContentType.Id,true);
                      
                    }
                    else
                    {
                        ct = SelectedWeb.GetContentTypeByName(ContentType.Name,true);
                    }
                    if (ct != null)
                    {
                        ct.EnsureProperty(c => c.FieldLinks);
                        var fieldLink = ct.FieldLinks.FirstOrDefault(f => f.Id == field.Id);
                        fieldLink.DeleteObject();
                        ct.Update(!DoNotUpdateChildren);
                        ClientContext.ExecuteQueryRetry();
                    }
                }
            }
            else
            {
                throw new Exception("Field not found");
            }
        }


    }
}
