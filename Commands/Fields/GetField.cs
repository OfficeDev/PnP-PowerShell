using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
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
    public class GetField : SPOWebCmdlet
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true, HelpMessage = "The list object or name where to get the field from")]
        public ListPipeBind List;

        [Parameter(Mandatory = false, Position=0, ValueFromPipeline=true, HelpMessage = "The field object or name to get")]
        public FieldPipeBind Identity = new FieldPipeBind();

        protected override void ExecuteCmdlet()
        {
            if (List != null)
            {
                var list = List.GetList(SelectedWeb);

                Field f = null;
                FieldCollection c = null;
                if (list != null)
                {
                    if (Identity.Id != Guid.Empty)
                    {
                        f = list.Fields.GetById(Identity.Id);
                    }
                    else if (!string.IsNullOrEmpty(Identity.Name))
                    {
                        f = list.Fields.GetByInternalNameOrTitle(Identity.Name);
                    }
                    else
                    {
                        c = list.Fields;
                        ClientContext.Load(c);
                        ClientContext.ExecuteQueryRetry();
                    }
                }
                if (f != null)
                {
                    ClientContext.Load(f);
                    ClientContext.ExecuteQueryRetry();
                    WriteObject(f);
                }
                else if (c != null)
                {

                    WriteObject(c, true);
                }
                else
                {
                    WriteObject(null);
                }
            }
            else
            {

                // Get a site column
                if (Identity.Id == Guid.Empty && string.IsNullOrEmpty(Identity.Name))
                {
                    // Get all columns
                    ClientContext.Load(SelectedWeb.Fields);
                    ClientContext.ExecuteQueryRetry();
                    WriteObject(SelectedWeb.Fields, true);
                }
                else
                {
                    Field f = null;
                    if (Identity.Id != Guid.Empty)
                    {
                        f = SelectedWeb.Fields.GetById(Identity.Id);
                    }
                    else if (!string.IsNullOrEmpty(Identity.Name))
                    {
                        f = SelectedWeb.Fields.GetByInternalNameOrTitle(Identity.Name);
                    }
                    ClientContext.Load(f);
                    ClientContext.ExecuteQueryRetry();
                    switch (f.FieldTypeKind)
                    {
                        case FieldType.DateTime:
                            {
                                WriteObject(ClientContext.CastTo<FieldDateTime>(f));
                                break;
                            }
                        case FieldType.Choice:
                            {
                                WriteObject(ClientContext.CastTo<FieldChoice>(f));
                                break;
                            }
                        case FieldType.Calculated:
                            {
                                WriteObject(ClientContext.CastTo<FieldCalculated>(f));
                                break;
                            }
                        case FieldType.Computed:
                            {
                                WriteObject(ClientContext.CastTo<FieldComputed>(f));
                                break;
                            }
                        case FieldType.Geolocation:
                            {
                                WriteObject(ClientContext.CastTo<FieldGeolocation>(f));
                                break;

                            }
                        case FieldType.User:
                            {
                                WriteObject(ClientContext.CastTo<FieldUser>(f));
                                break;
                            }
                        case FieldType.Currency:
                            {
                                WriteObject(ClientContext.CastTo<FieldCurrency>(f));
                                break;
                            }
                        case FieldType.Guid:
                            {
                                WriteObject(ClientContext.CastTo<FieldGuid>(f));
                                break;
                            }
                        case FieldType.URL:
                            {
                                WriteObject(ClientContext.CastTo<FieldUrl>(f));
                                break;
                            }
                        case FieldType.Lookup:
                            {
                                WriteObject(ClientContext.CastTo<FieldLookup>(f));
                                break;
                            }
                        case FieldType.MultiChoice:
                            {
                                WriteObject(ClientContext.CastTo<FieldMultiChoice>(f));
                                break;
                            }
                        case FieldType.Number:
                            {
                                WriteObject(ClientContext.CastTo<FieldNumber>(f));
                                break;
                            }
                        default:
                            {
                                WriteObject(f);
                                break;
                            }
                    }
                }
            }

        }
    }

}
