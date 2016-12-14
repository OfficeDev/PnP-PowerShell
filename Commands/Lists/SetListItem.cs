using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.Lists
{
    [Cmdlet(VerbsCommon.Set, "PnPListItem")]
    [CmdletAlias("Set-SPOListItem")]
    [CmdletHelp("Updates a list item",
        Category = CmdletHelpCategory.Lists,
        OutputType = typeof(ListItem),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.listitem.aspx")]
    [CmdletExample(
        Code = @"Set-PnPListItem -List ""Demo List"" -Identity 1 -Values @{""Title"" = ""Test Title""; ""Category""=""Test Category""}",
        Remarks = @"Sets fields value in the list item with ID 1 in the ""Demo List"". It sets both the Title and Category fields with the specified values. Notice, use the internal names of fields.",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"Set-PnPListItem -List ""Demo List"" -Identity 1 -ContentType ""Company"" -Values @{""Title"" = ""Test Title""; ""Category""=""Test Category""}",
        Remarks = @"Sets fields value in the list item with ID 1 in the ""Demo List"". It sets the content type of the item to ""Company"" and it sets both the Title and Category fields with the specified values. Notice, use the internal names of fields.",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"Set-PnPListItem -List ""Demo List"" -Identity $item -Values @{""Title"" = ""Test Title""; ""Category""=""Test Category""}",
        Remarks = @"Sets fields value in the list item which has been retrieved by for instance Get-PnPListItem. It sets the content type of the item to ""Company"" and it sets both the Title and Category fields with the specified values. Notice, use the internal names of fields.",
        SortOrder = 3)]
    public class SetListItem : SPOWebCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The ID, Title or Url of the list.")]
        public ListPipeBind List;

        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "The ID of the listitem, or actual ListItem object")]
        public ListItemPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "Specify either the name, ID or an actual content type")]
        public ContentTypePipeBind ContentType;

        [Parameter(Mandatory = false, HelpMessage = "Use the internal names of the fields when specifying field names." +
                                                    "\n\nSingle line of text: -Values @{\"Title\" = \"Title New\"}" +
                                                    "\n\nMultiple lines of text: -Values @{\"MultiText\" = \"New text\\n\\nMore text\"}" +
                                                    "\n\nRich text: -Values @{\"MultiText\" = \"<strong>New</strong> text\"}" +
            "\n\nChoice: -Values @{\"Choice\" = \"Value 1\"}" +
            "\n\nNumber: -Values @{\"Number\" = \"10\"}" +
            "\n\nCurrency: -Values @{\"Number\" = \"10\"}" +
            "\n\nCurrency: -Values @{\"Currency\" = \"10\"}" +
            "\n\nDate and Time: -Values @{\"DateAndTime\" = \"03/10/2015 14:16\"}" +
            "\n\nLookup (id of lookup value): -Values @{\"Lookup\" = \"2\"}" +
            "\n\nYes/No: -Values @{\"YesNo\" = \"No\"}" +
           "\n\nPerson/Group (id of user/group in Site User Info List or email of the user, seperate multiple values with a comma): -Values @{\"Person\" = \"user1@domain.com\",\"21\"}" +
            "\n\nHyperlink or Picture: -Values @{\"Hyperlink\" = \"https://github.com/OfficeDev/, OfficePnp\"}")]
        public Hashtable Values;

        protected override void ExecuteCmdlet()
        {
            List list = null;
            if (List != null)
            {
                list = List.GetList(SelectedWeb);
            }
            if (list != null)
            {
                var item = Identity.GetListItem(list);

                if (ContentType != null)
                {
                    ContentType ct = null;
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
                        ct.EnsureProperty(w => w.StringId);

                        item["ContentTypeId"] = ct.StringId;
                        item.Update();
                        ClientContext.ExecuteQueryRetry();
                    }
                }
                if (Values != null)
                {
                    var fields =
                        ClientContext.LoadQuery(list.Fields.Include(f => f.InternalName, f => f.Title,
                            f => f.FieldTypeKind));
                    ClientContext.ExecuteQueryRetry();

                    Hashtable values = Values ?? new Hashtable();

                    foreach (var key in values.Keys)
                    {
                        var field =
                            fields.FirstOrDefault(f => f.InternalName == key as string || f.Title == key as string);
                        if (field != null)
                        {
                            switch (field.FieldTypeKind)
                            {
                                case FieldType.User:
                                {
                                    List<FieldUserValue> userValues = new List<FieldUserValue>();

                                    var value = values[key];
                                    if (value.GetType().IsArray)
                                    {
                                        foreach (var arrayItem in value as object[])
                                        {
                                            int userId;
                                            if (!int.TryParse(arrayItem as string, out userId))
                                            {
                                                var user = SelectedWeb.EnsureUser(arrayItem as string);
                                                ClientContext.Load(user);
                                                ClientContext.ExecuteQueryRetry();
                                                userValues.Add(new FieldUserValue() {LookupId = user.Id});
                                            }
                                            else
                                            {
                                                userValues.Add(new FieldUserValue() {LookupId = userId});
                                            }
                                        }
                                        item[key as string] = userValues.ToArray();
                                    }
                                    else
                                    {
                                        int userId;
                                        if (!int.TryParse(value as string, out userId))
                                        {
                                            var user = SelectedWeb.EnsureUser(value as string);
                                            ClientContext.Load(user);
                                            ClientContext.ExecuteQueryRetry();
                                            item[key as string] = new FieldUserValue() {LookupId = user.Id};
                                        }
                                        else
                                        {
                                            item[key as string] = new FieldUserValue() {LookupId = userId};
                                        }
                                    }
                                    break;
                                }
                                default:
                                {
                                    item[key as string] = values[key];
                                    break;
                                }
                            }
                        }
                        else
                        {
                            throw new Exception("Field not present in list");
                        }
                    }


                    item.Update();
                    ClientContext.Load(item);
                    ClientContext.ExecuteQueryRetry();
                }
                WriteObject(item);
            }
        }
    }
}

