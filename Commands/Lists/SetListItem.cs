using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

// IMPORTANT: If you make changes to this cmdlet, also make the similar/same changes to the Set-PnPListItem Cmdlet

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
    public class SetListItem : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The ID, Title or Url of the list.")]
        public ListPipeBind List;

        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "The ID of the listitem, or actual ListItem object")]
        public ListItemPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "Specify either the name, ID or an actual content type")]
        public ContentTypePipeBind ContentType;

        [Parameter(Mandatory = false, HelpMessage = "Use the internal names of the fields when specifying field names." +
                                                    "\n\nSingle line of text: -Values @{\"TextField\" = \"Title New\"}" +
                                                    "\n\nMultiple lines of text: -Values @{\"MultiTextField\" = \"New text\\n\\nMore text\"}" +
                                                    "\n\nRich text: -Values @{\"MultiTextField\" = \"<strong>New</strong> text\"}" +
            "\n\nChoice: -Values @{\"ChoiceField\" = \"Value 1\"}" +
            "\n\nNumber: -Values @{\"NumberField\" = \"10\"}" +
            "\n\nCurrency: -Values @{\"NumberField\" = \"10\"}" +
            "\n\nCurrency: -Values @{\"CurrencyField\" = \"10\"}" +
            "\n\nDate and Time: -Values @{\"DateAndTimeField\" = \"03/10/2015 14:16\"}" +
            "\n\nLookup (id of lookup value): -Values @{\"LookupField\" = \"2\"}" +
            "\n\nMulti value lookup (id of lookup values as array 1): -Values @{\"MultiLookupField\" = \"1\",\"2\"}" +
            "\n\nMulti value lookup (id of lookup values as array 2): -Values @{\"MultiLookupField\" = 1,2}" +
            "\n\nMulti value lookup (id of lookup values as string): -Values @{\"MultiLookupField\" = \"1,2\"}" +
            "\n\nYes/No: -Values @{\"YesNoField\" = \"No\"}" +
            "\n\nPerson/Group (id of user/group in Site User Info List or email of the user, seperate multiple values with a comma): -Values @{\"PersonField\" = \"user1@domain.com\",\"21\"}" +
            "\n\nManaged Metadata (single value with path to term): -Values @{\"MetadataField\" = \"CORPORATE|DEPARTMENTS|FINANCE\"}" +
            "\n\nManaged Metadata (single value with id of term): -Values @{\"MetadataField\" = \"fe40a95b-2144-4fa2-b82a-0b3d0299d818\"} with Id of term" +
            "\n\nManaged Metadata (multiple values with paths to terms): -Values @{\"MetadataField\" = \"CORPORATE|DEPARTMENTS|FINANCE\",\"CORPORATE|DEPARTMENTS|HR\"}" +
            "\n\nManaged Metadata (multiple values with ids of terms): -Values @{\"MetadataField\" = \"fe40a95b-2144-4fa2-b82a-0b3d0299d818\",\"52d88107-c2a8-4bf0-adfa-04bc2305b593\"}" +
            "\n\nHyperlink or Picture: -Values @{\"HyperlinkField\" = \"https://github.com/OfficeDev/, OfficePnp\"}")]
        public Hashtable Values;

#if !ONPREMISES
        [Parameter(Mandatory = false, HelpMessage = "Updating item without updating the modified and modified by fields")]
        public SwitchParameter SystemUpdate;
#endif

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
                            f => f.TypeAsString));
                    ClientContext.ExecuteQueryRetry();

                    Hashtable values = Values ?? new Hashtable();

                    foreach (var key in values.Keys)
                    {
                        var field =
                            fields.FirstOrDefault(f => f.InternalName == key as string || f.Title == key as string);
                        if (field != null)
                        {
                            switch (field.TypeAsString)
                            {
                                case "User":
                                case "UserMulti":
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
                                case "Lookup":
                                case "LookupMulti":
                                    {
                                        int[] multiValue;
                                        if (values[key] is Array)
                                        {
                                            var arr = (object[])values[key];
                                            multiValue = new int[arr.Length];
                                            for (int i = 0; i < arr.Length; i++)
                                            {
                                                multiValue[i] = int.Parse(arr[i].ToString());
                                            }
                                        }
                                        else
                                        {
                                            string valStr = values[key].ToString();
                                            multiValue = valStr.Split(',', ';').Select(int.Parse).ToArray();
                                        }

                                        var newVals = multiValue.Select(id => new FieldLookupValue { LookupId = id }).ToArray();

                                        FieldLookup lookupField = ClientContext.CastTo<FieldLookup>(field);
                                        lookupField.EnsureProperty(lf => lf.AllowMultipleValues);
                                        if (!lookupField.AllowMultipleValues && newVals.Length > 1)
                                        {
                                            throw new Exception("Field " + field.InternalName + " does not support multiple values");
                                        }

                                        item[key as string] = newVals;
#if !ONPREMISES
                                        item.SystemUpdate();
#else
                                        item.Update();
#endif
                                        break;
                                    }
                                default:
                                    {
                                        item[key as string] = values[key];
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

#if !ONPREMISES
                    if (SystemUpdate)
                    {
                        item.SystemUpdate();
                    }
                    else
                    {
                        item.Update();
                    }
#else
                    item.Update();
#endif
                    ClientContext.Load(item);
                    ClientContext.ExecuteQueryRetry();
                }
                WriteObject(item);
            }
        }
    }
}

