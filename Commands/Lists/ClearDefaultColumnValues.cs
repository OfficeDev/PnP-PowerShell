using System;
using System.Collections.Generic;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Entities;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.Lists
{
    //TODO: Create Test

    [Cmdlet(VerbsCommon.Clear, "PnPDefaultColumnValues")]
    [CmdletHelp("Clear default column values for a document library",
        DetailedDescription = "Clear default column values for a document library, per folder, or for the root folder if the folder parameter has not been specified.",
        Category = CmdletHelpCategory.Lists)]
    [CmdletExample(
        Code = "PS:> Clear-PnPDefaultColumnValues -List Documents -Field MyField",
        SortOrder = 1,
        Remarks = "Clears the default value for the field MyField on a library")]
    [CmdletExample(
        Code = "PS:> Clear-PnPDefaultColumnValues -List Documents -Field MyField -Folder A",
        SortOrder = 2,
        Remarks = "Clears the default value for the field MyField on the folder A on a library")]
    public class ClearDefaultColumnValues : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The ID, Name or Url of the list.")]
        public ListPipeBind List;

        [Parameter(Mandatory = true, HelpMessage = "The internal name, id or a reference to a field")]
        public FieldPipeBind Field;

        [Parameter(Mandatory = false, HelpMessage = "A library relative folder path, if not specified it will set the default column values on the root folder of the library ('/')")]
        public string Folder = "/";

        protected override void ExecuteCmdlet()
        {
            List list = null;
            if (List != null)
            {
                list = List.GetList(SelectedWeb);
            }
            if (list != null)
            {
                if (list.BaseTemplate == (int)ListTemplateType.DocumentLibrary || list.BaseTemplate == (int)ListTemplateType.WebPageLibrary || list.BaseTemplate == (int)ListTemplateType.PictureLibrary)
                {
                    Field field = null;
                    // Get the field
                    if (Field.Field != null)
                    {
                        field = Field.Field;

                        ClientContext.Load(field);
                        ClientContext.ExecuteQueryRetry();

                        field.EnsureProperties(f => f.TypeAsString, f => f.InternalName);
                    }
                    else if (Field.Id != Guid.Empty)
                    {
                        field = list.Fields.GetById(Field.Id);
                        ClientContext.Load(field, f => f.InternalName, f => f.TypeAsString);
                        ClientContext.ExecuteQueryRetry();
                    }
                    else if (!string.IsNullOrEmpty(Field.Name))
                    {
                        field = list.Fields.GetByInternalNameOrTitle(Field.Name);
                        ClientContext.Load(field, f => f.InternalName, f => f.TypeAsString);
                        ClientContext.ExecuteQueryRetry();
                    }
                    if (field != null)
                    {
                        IDefaultColumnValue defaultColumnValue = field.GetDefaultColumnValueFromField(ClientContext, Folder, new string[0]);
                        list.ClearDefaultColumnValues(new List<IDefaultColumnValue>() { defaultColumnValue });
                    }
                }
                else
                {
                    WriteWarning("List is not a document library");
                }

            }
        }
    }

}
