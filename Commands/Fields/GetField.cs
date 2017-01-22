using System;
using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.Fields
{
    [Cmdlet(VerbsCommon.Get, "PnPField")]
    [CmdletAlias("Get-SPOField")]
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

                    WriteObject(fieldCollection, true);
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
                    ClientContext.Load(SelectedWeb.Fields, fc => fc.IncludeWithDefaultProperties(RetrievalExpressions));
                    ClientContext.ExecuteQueryRetry();
                    WriteObject(SelectedWeb.Fields, true);
                }
                else
                {
                    Field field = null;
                    if (Identity.Id != Guid.Empty)
                    {
                        field = SelectedWeb.Fields.GetById(Identity.Id);
                    }
                    else if (!string.IsNullOrEmpty(Identity.Name))
                    {
                        field = SelectedWeb.Fields.GetByInternalNameOrTitle(Identity.Name);
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
