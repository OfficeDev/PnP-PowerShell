using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.ContentTypes
{
    [Cmdlet(VerbsCommon.Add, "PnPFieldToContentType")]
    [CmdletAlias("Add-SPOFieldToContentType")]
    [CmdletHelp("Adds an existing site column to a content type", 
        Category = CmdletHelpCategory.ContentTypes)]
    [CmdletExample(
        Code = @"PS:> Add-PnPFieldToContentType -Field ""Project_Name"" -ContentType ""Project Document""",
        Remarks = @"This will add an existing site column with an internal name of ""Project_Name"" to a content type called ""Project Document""", 
        SortOrder = 1)]
    public class AddFieldToContentType : SPOWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Specifies the field that needs to be added to the content type")]
        public FieldPipeBind Field;

        [Parameter(Mandatory = true, HelpMessage = "Specifies which content type a field needs to be added to")]
        public ContentTypePipeBind ContentType;

        [Parameter(Mandatory = false, HelpMessage = "Specifies whether the field is required or not")]
        public SwitchParameter Required;

        [Parameter(Mandatory = false, HelpMessage = "Specifies whether the field should be hidden or not")]
        public SwitchParameter Hidden;

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
                    SelectedWeb.AddFieldToContentType(ContentType.ContentType, field, Required, Hidden);
                }
                else
                {
                    ContentType ct;
                    if (!string.IsNullOrEmpty(ContentType.Id))
                    {
                        ct = SelectedWeb.GetContentTypeById(ContentType.Id);
                      
                    }
                    else
                    {
                        ct = SelectedWeb.GetContentTypeByName(ContentType.Name);
                    }
                    if (ct != null)
                    {
                        SelectedWeb.AddFieldToContentType(ct, field, Required, Hidden);
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
