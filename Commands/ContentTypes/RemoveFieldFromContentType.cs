using System;
using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.ContentTypes
{
    [Cmdlet(VerbsCommon.Remove, "PnPFieldFromContentType")]
    [CmdletHelp("Removes a site column from a content type",
        Category = CmdletHelpCategory.ContentTypes)]
    [CmdletExample(
     Code = @"PS:> Remove-PnPFieldFromContentType -Field ""Project_Name"" -ContentType ""Project Document""",
     Remarks = @"This will remove the site column with an internal name of ""Project_Name"" from a content type called ""Project Document""", SortOrder = 1)]
    [CmdletExample(
     Code = @"PS:> Remove-PnPFieldFromContentType -Field ""Project_Name"" -ContentType ""Project Document"" -DoNotUpdateChildren",
     Remarks = @"This will remove the site column with an internal name of ""Project_Name"" from a content type called ""Project Document"". It will not update content types that inherit from the ""Project Document"" content type.", SortOrder = 1)]
    public class RemoveFieldFromContentType : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The field to remove")]
        public FieldPipeBind Field;

        [Parameter(Mandatory = true, HelpMessage = "The content type where the field is to be removed from")]
        public ContentTypePipeBind ContentType;

        [Parameter(Mandatory = false, HelpMessage = "If specified, inherited content types will not be updated")]
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
                    if (fieldLink != null)
                    {
                        fieldLink.DeleteObject();
                        ContentType.ContentType.Update(!DoNotUpdateChildren);
                        ClientContext.ExecuteQueryRetry();
                    }
                    else
                    {
                        ThrowTerminatingError(new ErrorRecord(new Exception("Cannot find field reference in content type"), "FieldRefNotFound", ErrorCategory.ObjectNotFound, ContentType));
                    }

                }
                else
                {
                    ContentType ct;
                    if (!string.IsNullOrEmpty(ContentType.Id))
                    {
                        ct = SelectedWeb.GetContentTypeById(ContentType.Id, true);

                    }
                    else
                    {
                        ct = SelectedWeb.GetContentTypeByName(ContentType.Name, true);
                    }
                    if (ct != null)
                    {
                        ct.EnsureProperty(c => c.FieldLinks);
                        var fieldLink = ct.FieldLinks.FirstOrDefault(f => f.Id == field.Id);
                        if (fieldLink != null)
                        {
                            fieldLink.DeleteObject();
                            ct.Update(!DoNotUpdateChildren);
                            ClientContext.ExecuteQueryRetry();
                        }
                        else
                        {
                            ThrowTerminatingError(new ErrorRecord(new Exception("Cannot find field reference in content type"), "FieldRefNotFound", ErrorCategory.ObjectNotFound, ContentType));
                        }
                    }
                }
            }
            else
            {
                ThrowTerminatingError(new ErrorRecord(new Exception("Field not found"), "FieldNotFound", ErrorCategory.ObjectNotFound, this));
            }
        }


    }
}
