using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Entities;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System.Collections;
using Newtonsoft.Json;

namespace SharePointPnP.PowerShell.Commands.Fields
{
    [Cmdlet(VerbsCommon.Add, "PnPField", DefaultParameterSetName = "ListPara")]
    [CmdletHelp("Adds a field to a list or as a site column",
        Category = CmdletHelpCategory.Fields,
        OutputType = typeof(Field),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.field.aspx")]
    [CmdletExample(
     Code = @"PS:> Add-PnPField -List ""Demo list"" -DisplayName ""Location"" -InternalName ""SPSLocation"" -Type Choice -Group ""Demo Group"" -AddToDefaultView -Choices ""Stockholm"",""Helsinki"",""Oslo""",
     Remarks = @"This will add a field of type Choice to the list ""Demo List"".", SortOrder = 1)]
    [CmdletExample(
     Code = @"PS:>Add-PnPField -List ""Demo list"" -DisplayName ""Speakers"" -InternalName ""SPSSpeakers"" -Type MultiChoice -Group ""Demo Group"" -AddToDefaultView -Choices ""Obiwan Kenobi"",""Darth Vader"", ""Anakin Skywalker""",
Remarks = @"This will add a field of type Multiple Choice to the list ""Demo List"". (you can pick several choices for the same item)", SortOrder = 2)]
    [CmdletAdditionalParameter(ParameterType = typeof(string[]),ParameterName = "Choices", HelpMessage = "Specify choices, only valid if the field type is Choice", ParameterSetName = "ListPara")]
    [CmdletAdditionalParameter(ParameterType = typeof(string[]), ParameterName = "Choices", HelpMessage = "Specify choices, only valid if the field type is Choice", ParameterSetName = "WebPara")]
    public class AddField : PnPWebCmdlet, IDynamicParameters
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true, ParameterSetName = "ListPara", HelpMessage = "The name of the list, its ID or an actual list object where this field needs to be added")]
        [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = "FieldRef", HelpMessage = "The name of the list, its ID or an actual list object where this field needs to be added")]
        public ListPipeBind List;

        [Parameter(Mandatory = true, ParameterSetName = "FieldRef", HelpMessage = "The name of the field, its ID or an actual field object that needs to be added")]
        public FieldPipeBind Field;

        [Parameter(Mandatory = true, ParameterSetName = "ListPara", HelpMessage = "The display name of the field")]
        [Parameter(Mandatory = true, ParameterSetName = "WebPara", HelpMessage = "The display name of the field")]
        public string DisplayName;

        [Parameter(Mandatory = true, ParameterSetName = "ListPara", HelpMessage = "The internal name of the field")]
        [Parameter(Mandatory = true, ParameterSetName = "WebPara", HelpMessage = "The internal name of the field")]
        public string InternalName;

        [Parameter(Mandatory = true, ParameterSetName = "ListPara", HelpMessage = "The type of the field like Choice, Note, MultiChoice")]
        [Parameter(Mandatory = true, ParameterSetName = "WebPara", HelpMessage = "The type of the field like Choice, Note, MultiChoice")]
        public FieldType Type;

        [Parameter(Mandatory = false, ParameterSetName = "ListPara", HelpMessage = "The ID of the field, must be unique")]
        [Parameter(Mandatory = false, ParameterSetName = "WebPara", HelpMessage = "The ID of the field, must be unique")]
        public GuidPipeBind Id = new GuidPipeBind();

        [Parameter(Mandatory = false, ParameterSetName = "ListPara", HelpMessage = "Switch Parameter if this field must be added to the default view")]
        [Parameter(Mandatory = false, ParameterSetName = "ListXML", HelpMessage = "Switch Parameter if this field must be added to the default view")]
        public SwitchParameter AddToDefaultView;

        [Parameter(Mandatory = false, ParameterSetName = "ListPara", HelpMessage = "Switch Parameter if the field is a required field")]
        [Parameter(Mandatory = false, ParameterSetName = "ListXML", HelpMessage = "Switch Parameter if the field is a required field")]
        public SwitchParameter Required;

        [Parameter(Mandatory = false, ParameterSetName = "ListPara", HelpMessage = "The group name to where this field belongs to")]
        [Parameter(Mandatory = false, ParameterSetName = "ListXML", HelpMessage = "The group name to where this field belongs to")]
        public string Group;

#if !ONPREMISES
        [Parameter(Mandatory = false, ParameterSetName = "ListPara", HelpMessage = "The Client Side Component Id to set to the field")]
        [Parameter(Mandatory = false, ParameterSetName = "WebPara", HelpMessage = "The Client Side Component Id to set to the field")]
        public GuidPipeBind ClientSideComponentId;

        [Parameter(Mandatory = false, ParameterSetName = "ListPara", HelpMessage = "The Client Side Component Properties to set to the field")]
        [Parameter(Mandatory = false, ParameterSetName = "WebPara", HelpMessage = "The Client Side Component Properties to set to the field")]
        public string ClientSideComponentProperties;
#endif

        [Parameter(Mandatory = false)]
        [Obsolete("Not in use")]
        public AddFieldOptions FieldOptions = AddFieldOptions.DefaultValue;

        public object GetDynamicParameters()
        {
            if (Type == FieldType.Choice || Type == FieldType.MultiChoice)
            {
                _context = new ChoiceFieldDynamicParameters();
                return _context;
            }
            return null;
        }

        private ChoiceFieldDynamicParameters _context;

        protected override void ExecuteCmdlet()
        {

            if (Id.Id == Guid.Empty)
            {
                Id = new GuidPipeBind(Guid.NewGuid());
            }

            if (List != null)
            {
                var list = List.GetList(SelectedWeb);
                Field f;
                if (ParameterSetName != "FieldRef")
                {
                    var fieldCI = new FieldCreationInformation(Type)
                    {
                        Id = Id.Id,
                        InternalName = InternalName,
                        DisplayName = DisplayName,
                        Group = Group,
                        AddToDefaultView = AddToDefaultView
                    };

                    if (Type == FieldType.Choice || Type == FieldType.MultiChoice)
                    {
                        f = list.CreateField<FieldChoice>(fieldCI);
                        ((FieldChoice)f).Choices = _context.Choices;
                        f.Update();
                        ClientContext.ExecuteQueryRetry();
                    }
                    else
                    {
                        f = list.CreateField(fieldCI);

                    }
                    if (Required)
                    {
                        f.Required = true;
                        f.Update();
                        ClientContext.Load(f);
                        ClientContext.ExecuteQueryRetry();
                    }
                    WriteObject(f);
                }
                else
                {
                    Field field = Field.Field;
                    if (field == null)
                    {
                        if (Field.Id != Guid.Empty)
                        {
                            field = SelectedWeb.Fields.GetById(Field.Id);
                            ClientContext.Load(field);
                            ClientContext.ExecuteQueryRetry();
                        }
                        else if (!string.IsNullOrEmpty(Field.Name))
                        {
                            try
                            {
                                field = SelectedWeb.Fields.GetByInternalNameOrTitle(Field.Name);
                                ClientContext.Load(field);
                                ClientContext.ExecuteQueryRetry();
                            }
                            catch
                            {
                                // Field might be sitecolumn, swallow exception
                            }
                            if (field != null)
                            {
                                var rootWeb = ClientContext.Site.RootWeb;
                                field = rootWeb.Fields.GetByInternalNameOrTitle(Field.Name);
                                ClientContext.Load(field);
                                ClientContext.ExecuteQueryRetry();
                            }
                        }
                    }
                    if (field != null)
                    {
                        list.Fields.Add(field);
                        list.Update();
                        ClientContext.ExecuteQueryRetry();
                    }
                }
            }
            else
            {
                Field f;

                var fieldCI = new FieldCreationInformation(Type)
                {
                    Id = Id.Id,
                    InternalName = InternalName,
                    DisplayName = DisplayName,
                    Group = Group,
                    AddToDefaultView = AddToDefaultView
                };



                if (Type == FieldType.Choice || Type == FieldType.MultiChoice)
                {
                    f = SelectedWeb.CreateField<FieldChoice>(fieldCI);
                    ((FieldChoice)f).Choices = _context.Choices;
                    f.Update();
                    ClientContext.ExecuteQueryRetry();
                }
                else
                {
                    f = SelectedWeb.CreateField(fieldCI);
                }

                if (Required)
                {
                    f.Required = true;
                    f.Update();
                    ClientContext.Load(f);
                    ClientContext.ExecuteQueryRetry();
                }
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

        public class ChoiceFieldDynamicParameters
        {
            [Parameter(Mandatory = false)]
            public string[] Choices
            {
                get { return _choices; }
                set { _choices = value; }
            }
            private string[] _choices;
        }

    }

}
