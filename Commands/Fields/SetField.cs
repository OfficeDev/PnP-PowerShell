using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System.Collections;

namespace PnP.PowerShell.Commands.Fields
{
    [Cmdlet(VerbsCommon.Set, "PnPField")]
    [CmdletHelp("Changes one or more properties of a field in a specific list or for the whole web",
        Category = CmdletHelpCategory.Fields,
        OutputType = typeof(Field),
        OutputTypeLink = "https://docs.microsoft.com/previous-versions/office/sharepoint-server/ee545882(v=office.15)")]
    [CmdletExample(
        Code = @"PS:> Set-PnPField -Identity AssignedTo -Values @{JSLink=""customrendering.js"";Group=""My fields""}",
        Remarks = @"Updates the AssignedTo field on the current web to use customrendering.js for the JSLink and sets the group name the field is categorized in to ""My Fields"". Lists that are already using the AssignedTo field will not be updated.",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Set-PnPField -Identity AssignedTo -Values @{JSLink=""customrendering.js"";Group=""My fields""} -UpdateExistingLists",
        Remarks = @"Updates the AssignedTo field on the current web to use customrendering.js for the JSLink and sets the group name the field is categorized in to ""My Fields"". Lists that are already using the AssignedTo field will also be updated.",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Set-PnPField -List ""Tasks"" -Identity ""AssignedTo"" -Values @{JSLink=""customrendering.js""}",
        Remarks = @"Updates the AssignedTo field on the Tasks list to use customrendering.js for the JSLink",
        SortOrder = 3)]
    public class SetField : PnPWebCmdlet
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true, HelpMessage = "The list object, name or id where to update the field. If omitted the field will be updated on the web.")]
        public ListPipeBind List;

        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, HelpMessage = "The field object, internal field name (case sensitive) or field id to update")]
        public FieldPipeBind Identity = new FieldPipeBind();

        [Parameter(Mandatory = true, HelpMessage = "Hashtable of properties to update on the field. Use the syntax @{property1=\"value\";property2=\"value\"}.")]
        public Hashtable Values;

        [Parameter(Mandatory = false, HelpMessage = "If provided, the field will be updated on existing lists that use it as well. If not provided or set to $false, existing lists using the field will remain unchanged but new lists will get the updated field.")]
        public SwitchParameter UpdateExistingLists;

        protected override void ExecuteCmdlet()
        {
            const string allowDeletionPropertyKey = "AllowDeletion";
            Field field = null;
            if (List != null)
            {
                var list = List.GetList(SelectedWeb);

                if (list == null)
                {
                    throw new ArgumentException("Unable to retrieve the list specified using the List parameter", "List");
                }

                if (Identity.Id != Guid.Empty)
                {
                    field = list.Fields.GetById(Identity.Id);
                }
                else if (!string.IsNullOrEmpty(Identity.Name))
                {
                    field = list.Fields.GetByInternalNameOrTitle(Identity.Name);
                }
                if (field == null)
                {
                    throw new ArgumentException("Unable to retrieve field with id, name or the field instance provided through Identity on the specified List", "Identity");
                }
            }
            else
            {
                if (Identity.Id != Guid.Empty)
                {
                    field = ClientContext.Web.Fields.GetById(Identity.Id);
                }
                else if (!string.IsNullOrEmpty(Identity.Name))
                {
                    field = ClientContext.Web.Fields.GetByInternalNameOrTitle(Identity.Name);
                }
                else if (Identity.Field != null)
                {
                    field = Identity.Field;
                }

                if (field == null)
                {
                    throw new ArgumentException("Unable to retrieve field with id, name or the field instance provided through Identity on the current web", "Identity");
                }
            }

            if (Values.ContainsKey(allowDeletionPropertyKey))
            {
                ClientContext.Load(field, f => f.SchemaXmlWithResourceTokens);
            }
            else
            {
                ClientContext.Load(field);
            }
            ClientContext.ExecuteQueryRetry();

            // Get a reference to the type-specific object to allow setting type-specific properties, i.e. LookupList and LookupField for Microsoft.SharePoint.Client.FieldLookup
            var typeSpecificField = field.TypedObject;

            foreach (string key in Values.Keys)
            {
                var value = Values[key];

                var property = typeSpecificField.GetType().GetProperty(key);

                bool isAllowDeletionProperty = string.Equals(key, allowDeletionPropertyKey, StringComparison.Ordinal);

                if (property == null && !isAllowDeletionProperty)
                {
                    WriteWarning($"No property '{key}' found on this field. Value will be ignored.");
                }
                else
                {
                    try
                    {
                        if (isAllowDeletionProperty)
                        {
                            field.SetAllowDeletion(value as bool?);
                        }
                        else
                        {
                            property.SetValue(typeSpecificField, value);
                        }
                    }
                    catch (Exception e)
                    {
                        WriteWarning($"Setting property '{key}' to '{value}' failed with exception '{e.Message}'. Value will be ignored.");
                    }
                }
            }
            field.UpdateAndPushChanges(UpdateExistingLists);
            ClientContext.ExecuteQueryRetry();
        }
    }
}