using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using OfficeDevPnP.Core.Entities;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.Lists
{
    //TODO: Create Test

    [Cmdlet(VerbsCommon.Set, "PnPDefaultColumnValues")]
    [CmdletHelp("Sets default column values for a document library",
        DetailedDescription = "Sets default column values for a document library, per folder, or for the root folder if the folder parameter has not been specified. Supports both text and taxonomy fields.",
        Category = CmdletHelpCategory.Lists)]
    [CmdletExample(
        Code = "PS:> Set-PnPDefaultColumnValues -List Documents -Field TaxKeyword -Value \"Company|Locations|Stockholm\"",
        SortOrder = 1,
        Remarks = "Sets a default value for the enterprise keywords field on a library to a term called \"Stockholm\", located in the \"Locations\" term set, which is part of the \"Company\" term group")]
    [CmdletExample(
        Code = "PS:> Set-PnPDefaultColumnValues -List Documents -Field TaxKeyword -Value \"15c4c4e4-4b67-4894-a1d8-de5ff811c791\"",
        SortOrder = 2,
        Remarks = "Sets a default value for the enterprise keywords field on a library to a term with the id \"15c4c4e4-4b67-4894-a1d8-de5ff811c791\". You need to ensure the term is valid for the field.")]
    [CmdletExample(
        Code = "PS:> Set-PnPDefaultColumnValues -List Documents -Field MyTextField -Value \"DefaultValue\" -Folder \"My folder\"",
        SortOrder = 3,
        Remarks = "Sets a default value for the MyTextField text field on the folder \"My folder\" in a library to a value of \"DefaultValue\"")]
    [CmdletExample(
        Code = "PS:> Set-PnPDefaultColumnValues -List Documents -Field MyPeopleField -Value \"1;#Foo Bar\"",
        SortOrder = 4,
        Remarks = "Sets a default value for the MyPeopleField people field on a library to a value of \"Foo Bar\" using the id from the user information list.")]
    [CmdletExample(
        Code = "PS:> $user = New-PnPUser -LoginName foobar@contoso.com\nPS:> Set-PnPDefaultColumnValues -List Documents -Field MyPeopleField -Value \"$($user.Id);#$($user.LoginName)\"",
        SortOrder = 5,
        Remarks = "Sets a default value for the MyPeopleField people field on a library to a value of \"Foo Bar\" using the id from the user information list.")]
    [CmdletExample(
        Code = "PS:> $user1 = New-PnPUser -LoginName user1@contoso.com\nPS:> $user2 = New-PnPUser -LoginName user2@contoso.com\nPS:> Set-PnPDefaultColumnValues -List Documents -Field MyMultiPeopleField -Value \"$($user1.Id);#$($user1.LoginName)\",\"$($user2.Id);#$($user2.LoginName)\"",
        SortOrder = 6,
        Remarks = "Sets a default value for the MyMultiPeopleField people field on a library to a value of \"User 1\" and \"User 2\" using the id from the user information list.")]
    public class SetDefaultColumnValues : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The ID, Name or Url of the list.")]
        public ListPipeBind List;

        [Parameter(Mandatory = true, HelpMessage = "The internal name, id or a reference to a field")]
        public FieldPipeBind Field;

        [Parameter(Mandatory = true, HelpMessage = "A list of values. In case of a text field the values will be concatenated, separated by a semi-colon. In case of a taxonomy field multiple values will added. In case of people field multiple values will be added.")]
        public string[] Value;

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
                        IDefaultColumnValue defaultColumnValue = field.GetDefaultColumnValueFromField(ClientContext, Folder, Value);
                        list.SetDefaultColumnValues(new List<IDefaultColumnValue>() { defaultColumnValue });
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
