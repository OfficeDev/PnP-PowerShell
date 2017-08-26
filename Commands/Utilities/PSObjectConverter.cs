using Microsoft.SharePoint.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Utilities
{
    /// <summary>
    /// Utilitity class to aid in converting model classes to PSObject classes suitable to be output in PowerShell
    /// </summary>
    public static class PSObjectConverter
    {
        /// <summary>
        /// Takes a ListItemCollection and converts the properties of all ListItems contained within to a PSObject
        /// </summary>
        /// <param name="listItemCollection">ListItemCollection to take its properties from its ListItems</param>
        /// <returns>PSObject which can be used to output the properties</returns>
        public static IList<PSObject> ConvertListItems(ListItemCollection listItemCollection)
        {
            var records = new List<PSObject>();
            foreach(var listItem in listItemCollection)
            {
                records.Add(ConvertListItem(listItem));
            }
            return records;
        }

        /// <summary>
        /// Takes a ListItem and converts its properties to a PSObject
        /// </summary>
        /// <param name="listItem">ListItem to take its properties from</param>
        /// <returns>PSObject which can be used to output the properties</returns>
        public static PSObject ConvertListItem(ListItem listItem)
        {
            var record = new PSObject();
            foreach (var field in listItem.FieldValues)
            {
                switch (field.Value?.GetType().ToString())
                {
                    // User picker
                    case "Microsoft.SharePoint.Client.FieldUserValue":
                        var user = (FieldUserValue)field.Value;
                        record.Properties.Add(new PSVariableProperty(new PSVariable(field.Key, $"{user.LookupId};#{user.LookupValue} {user.Email}")));
                        break;

                    // Lookup field
                    case "Microsoft.SharePoint.Client.FieldLookupValue":
                        var lookup = (FieldLookupValue)field.Value;
                        record.Properties.Add(new PSVariableProperty(new PSVariable(field.Key, $"{lookup.LookupId};#{lookup.LookupValue}")));
                        break;

                    // Any other field
                    default:
                        record.Properties.Add(new PSVariableProperty(new PSVariable(field.Key, field.Value)));
                        break;
                }
            }

            return record;
        }

        /// <summary>
        /// Takes an IEnumerable ClientObject collection of object and converts all of their properties to a PSObject IENumerable
        /// </summary>
        /// <param name="collection">Collection of ClientObjects to take their properties from</param>
        /// <param name="cmdLet">The cmdlet for which this command is executed</param>
        /// <returns>PSObject IEnumerable which can be used to output the properties</returns>
        public static IEnumerable<PSObject> ConvertGenericObjects(IEnumerable<ClientObject> collection, Cmdlet cmdLet, string[] defaultProperties = null)
        {
            var records = new List<PSObject>();
            foreach(var item in collection)
            {
                records.Add(ConvertGenericObject(item, cmdLet, defaultProperties));
            }
            return records;
        }

        /// <summary>
        /// Takes a ClientObject and converts its properties to a PSObject
        /// </summary>
        /// <param name="clientObject">Instance of an object to take its properties from</param>
        /// <param name="cmdLet">The cmdlet for which this command is executed</param>
        /// <returns>PSObject which can be used to output the properties</returns>
        public static PSObject ConvertGenericObject(ClientObject clientObject, Cmdlet cmdLet, string[] defaultProperties = null)
        {
            var record = new PSObject();
            var properties = clientObject.GetType().GetProperties();
            foreach (var property in properties)
            {
                try
                {
                    if (clientObject.IsPropertyAvailable(property.Name))
                    {
                        record.Properties.Add(new PSVariableProperty(new PSVariable(property.Name, property.GetValue(clientObject, null)?.ToString())));
                    }
                    else
                    {
                        cmdLet.WriteVerbose($"Property '{property.Name}' has not been loaded. Will be skipped in output.");
                    }
                }
                catch (Exception)
                {
                    // Swallow exceptions thay may occur when using reflection to get properties
                }
            }

            // Check if the default properties must be set or if all available properties should be returned. If delimiting the default properties to return, the other properties not included in the defaults can be requested by piping the output to Select -Property *.
            if (defaultProperties?.Length > 0)
            {
                cmdLet.WriteVerbose($"Setting default properties to '{defaultProperties.Aggregate((a, b) => a + ", " + b)}'. Use Select -Property * to display all available properties on this object.");
                record.Members.Add(new PSMemberSet("PSStandardMembers", new PSMemberInfo[] { new PSPropertySet("DefaultDisplayPropertySet", defaultProperties) }));
            }
            return record;
        }


        /// <summary>
        /// Takes an IEnumerable IDictionary<string, object> collection which is returned by Search Queryies and converts all of their properties to a PSObject IENumerable
        /// </summary>
        /// <param name="searchResultRows">Collection of IDictionary<string, object> to take their properties from</param>
        /// <param name="cmdLet">The cmdlet for which this command is executed</param>
        /// <returns>PSObject IEnumerable which can be used to output the properties</returns>
        public static IEnumerable<PSObject> ConvertSearchResultRows(IEnumerable<IDictionary<string, object>> searchResultRows, Cmdlet cmdLet, string[] defaultProperties = null)
        {
            var records = new List<PSObject>();
            foreach (var searchResultRow in searchResultRows)
            {
                records.Add(ConvertSearchResultRow(searchResultRow, cmdLet, defaultProperties));
            }
            return records;
        }

        /// <summary>
        /// Takes an IDictionary<string, object> item which is returned by Search Queryies and converts all of their properties to a PSObject IENumerable
        /// </summary>
        /// <param name="searchResultRow">Instance of an IDictionary<string, object> to take its properties from</param>
        /// <param name="cmdLet">The cmdlet for which this command is executed</param>
        /// <returns>PSObject which can be used to output the properties</returns>
        public static PSObject ConvertSearchResultRow(IDictionary<string, object> searchResultRow, Cmdlet cmdLet, string[] defaultProperties = null)
        {
            var record = new PSObject();
            foreach(KeyValuePair<string, object> searchResult in searchResultRow)
            {
                record.Properties.Add(new PSVariableProperty(new PSVariable(searchResult.Key, searchResult.Value?.ToString())));
            }

            // Check if the default properties must be set or if all available properties should be returned. If delimiting the default properties to return, the other properties not included in the defaults can be requested by piping the output to Select -Property *.
            if (defaultProperties?.Length > 0)
            {
                cmdLet.WriteVerbose($"Setting default properties to '{defaultProperties.Aggregate((a, b) => a + ", " + b)}'. Use Select -Property * to display all available properties on this object.");
                record.Members.Add(new PSMemberSet("PSStandardMembers", new PSMemberInfo[] { new PSPropertySet("DefaultDisplayPropertySet", defaultProperties) }));
            }
            return record;
        }
    }
}
