using Microsoft.SharePoint.Client;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Utilities
{
    /// <summary>
    /// Utilitity class to aid in converting model classes to PSObject classes suitable to be output in PowerShell
    /// </summary>
    public static class PSObjectConverter
    {
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
    }
}
