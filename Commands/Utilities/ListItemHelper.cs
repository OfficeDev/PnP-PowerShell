using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using PnP.PowerShell.Commands.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PnP.PowerShell.Commands.Utilities
{
    public static class ListItemHelper
    {
        private class FieldUpdateValue
        {
            public string Key { get; set; }
            public object Value { get; set; }
            public string FieldTypeString { get; set; }

            public FieldUpdateValue(string key, object value)
            {
                Key = key;
                Value = value;
            }
            public FieldUpdateValue(string key, object value, string fieldTypeString)
            {
                Key = key;
                Value = value;
                FieldTypeString = fieldTypeString;
            }
        }

        public static ListItem UpdateListItem(ListItem item, Hashtable valuesToSet, ListItemUpdateType updateType, Action<string> warningCallback, Action<string, string> terminatingError)
        {
            var itemValues = new List<FieldUpdateValue>();

            var context = item.Context as ClientContext;
            var list = item.ParentList;
            context.Web.EnsureProperty(w => w.Url);

            var clonedContext = context.Clone(context.Web.Url);
            var web = clonedContext.Web;

            var fields =
                     context.LoadQuery(list.Fields.Include(f => f.InternalName, f => f.Title,
                         f => f.TypeAsString));
            context.ExecuteQueryRetry();

            Hashtable values = valuesToSet ?? new Hashtable();

            foreach (var key in values.Keys)
            {
                var field = fields.FirstOrDefault(f => f.InternalName == key as string || f.Title == key as string);
                if (field != null)
                {
                    switch (field.TypeAsString)
                    {
                        case "User":
                        case "UserMulti":
                            {
                                List<FieldUserValue> userValues = new List<FieldUserValue>();

                                var value = values[key];
                                if (value == null) goto default;
                                if (value is string && string.IsNullOrWhiteSpace(value+"")) goto default;
                                if (value.GetType().IsArray)
                                {
                                    foreach (var arrayItem in (value as IEnumerable))
                                    {
                                        int userId;
                                        if (!int.TryParse(arrayItem.ToString(), out userId))
                                        {
                                            var user = web.EnsureUser(arrayItem as string);
                                            clonedContext.Load(user);
                                            clonedContext.ExecuteQueryRetry();
                                            userValues.Add(new FieldUserValue() { LookupId = user.Id });
                                        }
                                        else
                                        {
                                            userValues.Add(new FieldUserValue() { LookupId = userId });
                                        }
                                    }
                                    itemValues.Add(new FieldUpdateValue(key as string, userValues.ToArray(), null));
                                }
                                else
                                {
                                    int userId;
                                    if (!int.TryParse(value as string, out userId))
                                    {
                                        var user = web.EnsureUser(value as string);
                                        clonedContext.Load(user);
                                        clonedContext.ExecuteQueryRetry();
                                        itemValues.Add(new FieldUpdateValue(key as string, new FieldUserValue() { LookupId = user.Id }));
                                    }
                                    else
                                    {
                                        itemValues.Add(new FieldUpdateValue(key as string, new FieldUserValue() { LookupId = userId }));
                                    }
                                }
                                break;
                            }
                        case "TaxonomyFieldType":
                        case "TaxonomyFieldTypeMulti":
                            {
                                var value = values[key];
                                if (value != null && value.GetType().IsArray)
                                {
                                    var taxSession = clonedContext.Site.GetTaxonomySession();
                                    var terms = new List<KeyValuePair<Guid, string>>();
                                    foreach (var arrayItem in value as object[])
                                    {
                                        TaxonomyItem taxonomyItem;
                                        Guid termGuid;
                                        if (!Guid.TryParse(arrayItem as string, out termGuid))
                                        {
                                            // Assume it's a TermPath
                                            taxonomyItem = clonedContext.Site.GetTaxonomyItemByPath(arrayItem as string);
                                        }
                                        else
                                        {
                                            taxonomyItem = taxSession.GetTerm(termGuid);
                                            clonedContext.Load(taxonomyItem);
                                            clonedContext.ExecuteQueryRetry();
                                        }
                                        terms.Add(new KeyValuePair<Guid, string>(taxonomyItem.Id, taxonomyItem.Name));
                                    }

                                    TaxonomyField taxField = context.CastTo<TaxonomyField>(field);
                                    taxField.EnsureProperty(tf => tf.AllowMultipleValues);
                                    if (taxField.AllowMultipleValues)
                                    {
                                        var termValuesString = String.Empty;
                                        foreach (var term in terms)
                                        {
                                            termValuesString += "-1;#" + term.Value + "|" + term.Key.ToString("D") + ";#";
                                        }

                                        termValuesString = termValuesString.Substring(0, termValuesString.Length - 2);

                                        var newTaxFieldValue = new TaxonomyFieldValueCollection(context, termValuesString, taxField);
                                        itemValues.Add(new FieldUpdateValue(key as string, newTaxFieldValue, field.TypeAsString));
                                    }
                                    else
                                    {
                                        warningCallback?.Invoke($@"You are trying to set multiple values in a single value field. Skipping values for field ""{field.InternalName}""");
                                    }
                                }
                                else
                                {
                                    Guid termGuid = Guid.Empty;

                                    var taxSession = clonedContext.Site.GetTaxonomySession();
                                    TaxonomyItem taxonomyItem = null;
                                    if (value != null && !Guid.TryParse(value as string, out termGuid))
                                    {
                                        // Assume it's a TermPath
                                        taxonomyItem = clonedContext.Site.GetTaxonomyItemByPath(value as string);
                                    }
                                    else
                                    {
                                        if (value != null)
                                        {
                                            taxonomyItem = taxSession.GetTerm(termGuid);
                                            clonedContext.Load(taxonomyItem);
                                            clonedContext.ExecuteQueryRetry();
                                        }
                                    }

                                    TaxonomyField taxField = context.CastTo<TaxonomyField>(field);
                                    TaxonomyFieldValue taxValue = new TaxonomyFieldValue();
                                    if (taxonomyItem != null)
                                    {
                                        taxValue.TermGuid = taxonomyItem.Id.ToString();
                                        taxValue.Label = taxonomyItem.Name;
                                        itemValues.Add(new FieldUpdateValue(key as string, taxValue, field.TypeAsString));
                                    }
                                    else
                                    {
                                        taxField.ValidateSetValue(item, null);
                                    }
                                }
                                break;
                            }
                        case "Lookup":
                        case "LookupMulti":
                            {
                                var value = values[key];
                                if (value == null) goto default;
                                int[] multiValue;
                                if (value is Array)
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

                                FieldLookup lookupField = context.CastTo<FieldLookup>(field);
                                lookupField.EnsureProperty(lf => lf.AllowMultipleValues);
                                if (!lookupField.AllowMultipleValues && newVals.Length > 1)
                                {
                                    throw new Exception("Field " + field.InternalName + " does not support multiple values");
                                }
                                itemValues.Add(new FieldUpdateValue(key as string, newVals));
                                break;
                            }
                        default:
                            {
                                itemValues.Add(new FieldUpdateValue(key as string, values[key]));
                                break;
                            }
                    }
                }
                else
                {
                    terminatingError?.Invoke($"Field {key} not present in list.", "FIELDNOTINLIST");
                }
            }
            foreach (var itemValue in itemValues)
            {
                if (string.IsNullOrEmpty(itemValue.FieldTypeString))
                {
                    item[itemValue.Key] = itemValue.Value;
                }
                else
                {
                    switch (itemValue.FieldTypeString)
                    {
                        case "TaxonomyFieldTypeMulti":
                            {
                                var field = fields.FirstOrDefault(f => f.InternalName == itemValue.Key as string || f.Title == itemValue.Key as string);
                                var taxField = context.CastTo<TaxonomyField>(field);
                                if (itemValue.Value is TaxonomyFieldValueCollection)
                                {
                                    taxField.SetFieldValueByValueCollection(item, itemValue.Value as TaxonomyFieldValueCollection);
                                } else
                                {
                                    taxField.SetFieldValueByValue(item, itemValue.Value as TaxonomyFieldValue);
                                }
                                break;
                            }
                        case "TaxonomyFieldType":
                            {
                                var field = fields.FirstOrDefault(f => f.InternalName == itemValue.Key as string || f.Title == itemValue.Key as string);
                                var taxField = context.CastTo<TaxonomyField>(field);
                                taxField.SetFieldValueByValue(item, itemValue.Value as TaxonomyFieldValue);
                                break;
                            }
                    }
                }
            }
#if !ONPREMISES
            switch(updateType)
            {
                case ListItemUpdateType.Update:
                    {
                        item.Update();
                        break;
                    }
                case ListItemUpdateType.SystemUpdate:
                    {
                        item.SystemUpdate();
                        break;
                    }
                case ListItemUpdateType.UpdateOverwriteVersion:
                    {
                        item.UpdateOverwriteVersion();
                        break;
                    }
            }
#else
            item.Update();
#endif
            context.Load(item);
            context.ExecuteQueryRetry();
            return item;
        }
    }
}
