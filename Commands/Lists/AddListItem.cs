using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using OfficeDevPnP.Core.Utilities;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using SharePointPnP.PowerShell.Commands.Taxonomy;

namespace SharePointPnP.PowerShell.Commands.Lists
{
    [Cmdlet(VerbsCommon.Add, "PnPListItem")]
    [CmdletAlias("Add-SPOListItem")]
    [CmdletHelp("Adds an item to a list",
        Category = CmdletHelpCategory.Lists,
        OutputType = typeof(ListItem),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.listitem.aspx")]
    [CmdletExample(
        Code = @"Add-PnPListItem -List ""Demo List"" -Values @{""Title"" = ""Test Title""; ""Category""=""Test Category""}",
        Remarks = @"Adds a new list item to the ""Demo List"", and sets both the Title and Category fields with the specified values. Notice, use the internal names of fields.",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"Add-PnPListItem -List ""Demo List"" -ContentType ""Company"" -Values @{""Title"" = ""Test Title""; ""Category""=""Test Category""}",
        Remarks = @"Adds a new list item to the ""Demo List"", sets the content type to ""Company"" and sets both the Title and Category fields with the specified values. Notice, use the internal names of fields.",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"Add-PnPListItem -List ""Demo List"" -Values @{""MultiUserField""=""user1@domain.com"",""user2@domain.com""}",
        Remarks = @"Adds a new list item to the ""Demo List"" and sets the user field called MultiUserField to 2 users. Separate multiple users with a comma.",
        SortOrder = 3)]
    [CmdletExample(
        Code = @"Add-PnPListItem -List ""Demo List"" -Values @{""Title""=""Sales Report""} -Folder ""projects/europe""",
        Remarks = @"Adds a new list item to the ""Demo List"". It will add the list item to the europe folder which is located in the projects folder. Folders will be created if needed.",
        SortOrder = 3)]
    public class AddListItem : SPOWebCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The ID, Title or Url of the list.")]
        public ListPipeBind List;

        [Parameter(Mandatory = false, HelpMessage = "Specify either the name, ID or an actual content type.")]
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
            "\n\nManaged Metadata (single value with path to term): -Values @{\"MetadataField\" = \"CORPORATE|DEPARTMENTS|FINANCE\"}"+
            "\n\nManaged Metadata (singel value with id of term): -Values @{\"MetadataField\" = \"fe40a95b-2144-4fa2-b82a-0b3d0299d818\"} with Id of term" +
            "\n\nManaged Metadata (multiple values with paths to terms): -Values @{\"MetadataField\" = \"CORPORATE|DEPARTMENTS|FINANCE\",\"CORPORATE|DEPARTMENTS|HR\"}"+ 
            "\n\nManaged Metadata (multiple values with ids of terms): -Values @{\"MetadataField\" = \"fe40a95b-2144-4fa2-b82a-0b3d0299d818\",\"52d88107-c2a8-4bf0-adfa-04bc2305b593\"}" +
            "\n\nHyperlink or Picture: -Values @{\"Hyperlink\" = \"https://github.com/OfficeDev/, OfficePnp\"}")]
        public Hashtable Values;

        [Parameter(Mandatory = false, HelpMessage = @"The list relative URL of a folder. E.g. ""MyFolder"" for a folder located in the root of the list, or ""MyFolder/SubFolder"" for a folder located in the MyFolder folder which is located in the root of the list.")]
        public string Folder;

        protected override void ExecuteCmdlet()
        {
            List list = null;
            if (List != null)
            {
                list = List.GetList(SelectedWeb);
            }
            if (list != null)
            {
                ListItemCreationInformation liCI = new ListItemCreationInformation();
                if (Folder != null)
                {
                    // Create the folder if it doesn't exist
                    var rootFolder = list.EnsureProperty(l => l.RootFolder);
                    var targetFolder =
                        SelectedWeb.EnsureFolder(rootFolder, Folder);

                    liCI.FolderUrl = targetFolder.ServerRelativeUrl;
                }
                var item = list.AddItem(liCI);

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
                    // Load all list fields and their types
                    var fields = ClientContext.LoadQuery(list.Fields.Include(f => f.Id, f => f.InternalName, f => f.Title, f => f.TypeAsString));
                    ClientContext.ExecuteQueryRetry();

                    foreach (var key in Values.Keys)
                    {
                        var field = fields.FirstOrDefault(f => f.InternalName == key as string || f.Title == key as string);
                        if (field != null)
                        {
                            switch (field.TypeAsString)
                            {
                                case "User":
                                case "UserMulti":
                                    {
                                        var userValues = new List<FieldUserValue>();

                                        var value = Values[key];
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
                                                    userValues.Add(new FieldUserValue() { LookupId = user.Id });
                                                }
                                                else
                                                {
                                                    userValues.Add(new FieldUserValue() { LookupId = userId });
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
                                                item[key as string] = new FieldUserValue() { LookupId = user.Id };
                                            }
                                            else
                                            {
                                                item[key as string] = new FieldUserValue() { LookupId = userId };
                                            }
                                        }
#if !ONPREMISES
                                        item.SystemUpdate();
#else
                                        item.Update();
#endif
                                        break;
                                    }
                                case "TaxonomyFieldType":
                                case "TaxonomyFieldTypeMulti":
                                    {
                                        var value = Values[key];
                                        if (value.GetType().IsArray)
                                        {
                                            var taxSession = ClientContext.Site.GetTaxonomySession();
                                            var terms = new List<KeyValuePair<Guid, string>>();
                                            foreach (var arrayItem in value as object[])
                                            {
                                                TaxonomyItem taxonomyItem = null;
                                                Guid termGuid = Guid.Empty;
                                                if (!Guid.TryParse(arrayItem as string, out termGuid))
                                                {
                                                    // Assume it's a TermPath
                                                    taxonomyItem = ClientContext.Site.GetTaxonomyItemByPath(arrayItem as string);
                                                }
                                                else
                                                {
                                                    taxonomyItem = taxSession.GetTerm(termGuid);
                                                    ClientContext.Load(taxonomyItem);
                                                    ClientContext.ExecuteQueryRetry();
                                                }



                                                terms.Add(new KeyValuePair<Guid, string>(taxonomyItem.Id, taxonomyItem.Name));
                                            }

                                            TaxonomyField taxField = ClientContext.CastTo<TaxonomyField>(field);

                                            taxField.EnsureProperty(tf => tf.AllowMultipleValues);

                                            if (taxField.AllowMultipleValues)
                                            {
                                                var termValuesString = String.Empty;
                                                foreach (var term in terms)
                                                {
                                                    termValuesString += "-1;#" + term.Value + "|" + term.Key.ToString("D") + ";#";
                                                }

                                                termValuesString = termValuesString.Substring(0, termValuesString.Length - 2);

                                                var newTaxFieldValue = new TaxonomyFieldValueCollection(ClientContext, termValuesString, taxField);
                                                taxField.SetFieldValueByValueCollection(item, newTaxFieldValue);
#if !ONPREMISES
                                                item.SystemUpdate();
#else
                                                item.Update();
#endif
                                                ClientContext.ExecuteQueryRetry();
                                            }
                                            else
                                            {
                                                WriteWarning($@"You are trying to set multiple values in a single value field. Skipping values for field ""{field.InternalName}""");
                                            }
                                        }
                                        else
                                        {
                                            Guid termGuid = Guid.Empty;
                                            if (!Guid.TryParse(value as string, out termGuid))
                                            {
                                                // Assume it's a TermPath
                                                var taxonomyItem = ClientContext.Site.GetTaxonomyItemByPath(value as string);
                                                termGuid = taxonomyItem.Id;
                                            }
                                            item[key as string] = termGuid.ToString();
                                        }
#if !ONPREMISES
                                        item.SystemUpdate();
#else
                                        item.Update();
#endif
                                        break;
                                    }
                                default:
                                    {
                                        item[key as string] = Values[key];
#if !ONPREMISES
                                        item.SystemUpdate();
#else
                                        item.Update();
#endif
                                        break;
                                    }
                            }
                        }
                        else
                        {
                            throw new Exception("Field not present in list");
                        }
                    }
                }

                item.Update();
                ClientContext.Load(item);
                ClientContext.ExecuteQueryRetry();
                WriteObject(item);
            }
        }
    }

}
