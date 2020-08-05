using System;
using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.Fields
{
    [Cmdlet(VerbsCommon.Get, "PnPField")]
    [CmdletHelp("Returns a field from a list or site",
        Category = CmdletHelpCategory.Fields,
        OutputType = typeof(Field),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.field.aspx")]
    [CmdletExample(
        Code = @"PS:> Get-PnPField",
        Remarks = @"Gets all the fields from the current site",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPField -List ""Demo list"" -Identity ""Speakers""",
        Remarks = @"Gets the speakers field from the list Demo list",
        SortOrder = 2)]
    public class GetField : PnPWebRetrievalsCmdlet<Field>
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true, HelpMessage = "The list object or name where to get the field from")]
        public ListPipeBind List;

        [Parameter(Mandatory = false, Position = 0, ValueFromPipeline = true, HelpMessage = "The field object or name to get")]
        public FieldPipeBind Identity = new FieldPipeBind();

        [Parameter(Mandatory = false, HelpMessage = "Filter to the specified group")]
        public string Group;

        [Parameter(Mandatory = false, ValueFromPipeline = false, HelpMessage = "Search site hierarchy for fields")]
        public SwitchParameter InSiteHierarchy;

        protected override void ExecuteCmdlet()
        {
            if (List != null)
            {
                var list = List.GetList(SelectedWeb);

                Field field = null;
                FieldCollection fieldCollection = null;
                if (list != null)
                {
                    if (Identity.Id != Guid.Empty)
                    {
                        field = list.Fields.GetById(Identity.Id);
                    }
                    else if (!string.IsNullOrEmpty(Identity.Name))
                    {
                        field = list.Fields.GetByInternalNameOrTitle(Identity.Name);
                    }
                    else
                    {
                        fieldCollection = list.Fields;
                        ClientContext.Load(fieldCollection, fc => fc.IncludeWithDefaultProperties(RetrievalExpressions));
                        ClientContext.ExecuteQueryRetry();
                    }
                }
                if (field != null)
                {
                    ClientContext.Load(field, RetrievalExpressions);
                    ClientContext.ExecuteQueryRetry();
                    WriteObject(field);
                }
                else if (fieldCollection != null)
                {
                    if (!string.IsNullOrEmpty(Group))
                    {
                        WriteObject(fieldCollection.Where(f => f.Group.Equals(Group, StringComparison.InvariantCultureIgnoreCase)), true);
                    }
                    else
                    {
                        WriteObject(fieldCollection, true);
                    }
                }
                else
                {
                    WriteObject(null);
                }
            }
            else
            {
                if (Identity.Id == Guid.Empty && string.IsNullOrEmpty(Identity.Name))
                {
                    if (InSiteHierarchy.IsPresent)
                    {
                        ClientContext.Load(SelectedWeb.AvailableFields, fc => fc.IncludeWithDefaultProperties(RetrievalExpressions));
                    }
                    else
                    {
                        ClientContext.Load(SelectedWeb.Fields, fc => fc.IncludeWithDefaultProperties(RetrievalExpressions));
                    }
                    ClientContext.ExecuteQueryRetry();
                    if (!string.IsNullOrEmpty(Group))
                    {
                        if (InSiteHierarchy.IsPresent)
                        {
                            WriteObject(SelectedWeb.AvailableFields.Where(f => f.Group.Equals(Group, StringComparison.InvariantCultureIgnoreCase)).OrderBy(f => f.Title), true);
                        }
                        else
                        {
                            WriteObject(SelectedWeb.Fields.Where(f => f.Group.Equals(Group, StringComparison.InvariantCultureIgnoreCase)).OrderBy(f => f.Title), true);
                        }
                    }
                    else
                    {
                        if (InSiteHierarchy.IsPresent)
                        {
                            WriteObject(SelectedWeb.AvailableFields.OrderBy(f => f.Title), true);
                        }
                        else
                        {
                            WriteObject(SelectedWeb.Fields.OrderBy(f => f.Title), true);
                        }
                    }
                }
                else
                {
                    Field field = null;
                    if (Identity.Id != Guid.Empty)
                    {
                        if (InSiteHierarchy.IsPresent)
                        {
                            field = SelectedWeb.AvailableFields.GetById(Identity.Id);
                        }
                        else
                        {
                            field = SelectedWeb.Fields.GetById(Identity.Id);
                        }
                    }
                    else if (!string.IsNullOrEmpty(Identity.Name))
                    {
                        if (InSiteHierarchy.IsPresent)
                        {
                            field = SelectedWeb.AvailableFields.GetByInternalNameOrTitle(Identity.Name);
                        }
                        else
                        {
                            field = SelectedWeb.Fields.GetByInternalNameOrTitle(Identity.Name);
                        }
                    }
                    ClientContext.Load(field, RetrievalExpressions);
                    ClientContext.ExecuteQueryRetry();
                    
                    switch (field.FieldTypeKind)
                    {
                        case FieldType.DateTime:
                            {
                                WriteObject(ClientContext.CastTo<FieldDateTime>(field));
                                break;
                            }
                        case FieldType.Choice:
                            {
                                WriteObject(ClientContext.CastTo<FieldChoice>(field));
                                break;
                            }
                        case FieldType.Calculated:
                            {
                                WriteObject(ClientContext.CastTo<FieldCalculated>(field));
                                break;
                            }
                        case FieldType.Computed:
                            {
                                WriteObject(ClientContext.CastTo<FieldComputed>(field));
                                break;
                            }
                        case FieldType.Geolocation:
                            {
                                WriteObject(ClientContext.CastTo<FieldGeolocation>(field));
                                break;

                            }
                        case FieldType.User:
                            {
                                WriteObject(ClientContext.CastTo<FieldUser>(field));
                                break;
                            }
                        case FieldType.Currency:
                            {
                                WriteObject(ClientContext.CastTo<FieldCurrency>(field));
                                break;
                            }
                        case FieldType.Guid:
                            {
                                WriteObject(ClientContext.CastTo<FieldGuid>(field));
                                break;
                            }
                        case FieldType.URL:
                            {
                                WriteObject(ClientContext.CastTo<FieldUrl>(field));
                                break;
                            }
                        case FieldType.Lookup:
                            {
                                WriteObject(ClientContext.CastTo<FieldLookup>(field));
                                break;
                            }
                        case FieldType.MultiChoice:
                            {
                                WriteObject(ClientContext.CastTo<FieldMultiChoice>(field));
                                break;
                            }
                        case FieldType.Number:
                            {
                                WriteObject(ClientContext.CastTo<FieldNumber>(field));
                                break;
                            }
                        case FieldType.Invalid:
                            {
                                if (field.TypeAsString.StartsWith("TaxonomyFieldType"))
                                {
                                    WriteObject(ClientContext.CastTo<TaxonomyField>(field));
                                    break;
                                }
                                goto default;
                            }
                        default:
                            {
                                WriteObject(field);
                                break;
                            }
                    }
                }
            }

        }
    }

}
