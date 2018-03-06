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
    [Cmdlet(VerbsCommon.Add, "PnPField", DefaultParameterSetName = "Add field to list")]
    [CmdletHelp("Add a field",
        "Adds a field to a list or as a site column",
        Category = CmdletHelpCategory.Fields,
        OutputType = typeof(Field),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.field.aspx")]
    [CmdletExample(
     Code = @"PS:> Add-PnPField -List ""Demo list"" -DisplayName ""Location"" -InternalName ""SPSLocation"" -Type Choice -Group ""Demo Group"" -AddToDefaultView -Choices ""Stockholm"",""Helsinki"",""Oslo""",
     Remarks = @"This will add a field of type Choice to the list ""Demo List"".", SortOrder = 1)]
    [CmdletExample(
     Code = @"PS:>Add-PnPField -List ""Demo list"" -DisplayName ""Speakers"" -InternalName ""SPSSpeakers"" -Type MultiChoice -Group ""Demo Group"" -AddToDefaultView -Choices ""Obiwan Kenobi"",""Darth Vader"", ""Anakin Skywalker""",
Remarks = @"This will add a field of type Multiple Choice to the list ""Demo List"". (you can pick several choices for the same item)", SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Add-PnPField -Type Calculated -InternalName ""C1"" -DisplayName ""C1"" -Formula =""[Title]""",
        Remarks = @"Adds a new calculated site column with the formula specified")]
    [CmdletAdditionalParameter(ParameterType = typeof(string[]), ParameterName = "Choices", HelpMessage = "Specify choices, only valid if the field type is Choice", ParameterSetName = ParameterSet_ADDFIELDTOLIST)]
    [CmdletAdditionalParameter(ParameterType = typeof(string[]), ParameterName = "Choices", HelpMessage = "Specify choices, only valid if the field type is Choice", ParameterSetName = ParameterSet_ADDFIELDTOWEB)]
    [CmdletAdditionalParameter(ParameterType = typeof(string), ParameterName = "Formula", HelpMessage = "Specify the formula. Only available if the field type is Calculated", ParameterSetName = ParameterSet_ADDFIELDTOLIST)]
    [CmdletAdditionalParameter(ParameterType = typeof(string), ParameterName = "Formula", HelpMessage = "Specify the formula. Only avialable if the field type is Calculated", ParameterSetName = ParameterSet_ADDFIELDTOWEB)]
    public class AddField : PnPWebCmdlet, IDynamicParameters
    {
        const string ParameterSet_ADDFIELDTOLIST = "Add field to list";
        const string ParameterSet_ADDFIELDREFERENCETOLIST = "Add field reference to list";
        const string ParameterSet_ADDFIELDTOWEB = "Add field to web";
        const string ParameterSet_ADDFIELDBYXMLTOLIST = "Add field by XML to list";

        [Parameter(Mandatory = false, ValueFromPipeline = true, ParameterSetName = ParameterSet_ADDFIELDTOLIST, HelpMessage = "The name of the list, its ID or an actual list object where this field needs to be added")]
        [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = ParameterSet_ADDFIELDREFERENCETOLIST, HelpMessage = "The name of the list, its ID or an actual list object where this field needs to be added")]
        public ListPipeBind List;

        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_ADDFIELDREFERENCETOLIST, HelpMessage = "The name of the field, its ID or an actual field object that needs to be added")]
        public FieldPipeBind Field;

        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_ADDFIELDTOLIST, HelpMessage = "The display name of the field")]
        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_ADDFIELDTOWEB, HelpMessage = "The display name of the field")]
        public string DisplayName;

        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_ADDFIELDTOLIST, HelpMessage = "The internal name of the field")]
        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_ADDFIELDTOWEB, HelpMessage = "The internal name of the field")]
        public string InternalName;

        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_ADDFIELDTOLIST, HelpMessage = "The type of the field like Choice, Note, MultiChoice")]
        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_ADDFIELDTOWEB, HelpMessage = "The type of the field like Choice, Note, MultiChoice")]
        public FieldType Type;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ADDFIELDTOLIST, HelpMessage = "The ID of the field, must be unique")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ADDFIELDTOWEB, HelpMessage = "The ID of the field, must be unique")]
        public GuidPipeBind Id = new GuidPipeBind();

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ADDFIELDTOLIST, HelpMessage = "Switch Parameter if this field must be added to the default view")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ADDFIELDBYXMLTOLIST, HelpMessage = "Switch Parameter if this field must be added to the default view")]
        public SwitchParameter AddToDefaultView;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ADDFIELDTOLIST, HelpMessage = "Switch Parameter if the field is a required field")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ADDFIELDBYXMLTOLIST, HelpMessage = "Switch Parameter if the field is a required field")]
        public SwitchParameter Required;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ADDFIELDTOLIST, HelpMessage = "The group name to where this field belongs to")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ADDFIELDBYXMLTOLIST, HelpMessage = "The group name to where this field belongs to")]
        public string Group;

#if !ONPREMISES
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ADDFIELDTOLIST, HelpMessage = "The Client Side Component Id to set to the field")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ADDFIELDTOWEB, HelpMessage = "The Client Side Component Id to set to the field")]
        public GuidPipeBind ClientSideComponentId;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ADDFIELDTOLIST, HelpMessage = "The Client Side Component Properties to set to the field")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ADDFIELDTOWEB, HelpMessage = "The Client Side Component Properties to set to the field")]
        public string ClientSideComponentProperties;
#endif

        [Parameter(Mandatory = false)]
        [Obsolete("Not in use")]
        public AddFieldOptions FieldOptions = AddFieldOptions.DefaultValue;

        public object GetDynamicParameters()
        {
            if (Type == FieldType.Choice || Type == FieldType.MultiChoice)
            {
                choiceFieldParameters = new ChoiceFieldDynamicParameters();
                return choiceFieldParameters;
            }
            if (Type == FieldType.Calculated)
            {
                calculatedFieldParameters = new CalculatedFieldDynamicParameters();
                return calculatedFieldParameters;
            }
            return null;
        }

        private ChoiceFieldDynamicParameters choiceFieldParameters;
        private CalculatedFieldDynamicParameters calculatedFieldParameters;

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
                if (ParameterSetName != ParameterSet_ADDFIELDREFERENCETOLIST)
                {
                    var fieldCI = new FieldCreationInformation(Type)
                    {
                        Id = Id.Id,
                        InternalName = InternalName,
                        DisplayName = DisplayName,
                        Group = Group,
                        AddToDefaultView = AddToDefaultView
                    };
#if !ONPREMISES
                    if (ClientSideComponentId != null)
                    {
                        fieldCI.ClientSideComponentId = ClientSideComponentId.Id;
                    }
                    if (!string.IsNullOrEmpty(ClientSideComponentProperties))
                    {
                        fieldCI.ClientSideComponentProperties = ClientSideComponentProperties;
                    }
#endif
                    if (Type == FieldType.Choice || Type == FieldType.MultiChoice)
                    {
                        f = list.CreateField<FieldChoice>(fieldCI);
                        ((FieldChoice)f).Choices = choiceFieldParameters.Choices;
                        f.Update();
                        ClientContext.ExecuteQueryRetry();
                    }
                    else if (Type == FieldType.Calculated)
                    {
                        f = list.CreateField<FieldCalculated>(fieldCI);
                        ((FieldCalculated)f).Formula = calculatedFieldParameters.Formula;
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

#if !ONPREMISES
                if (ClientSideComponentId != null)
                {
                    fieldCI.ClientSideComponentId = ClientSideComponentId.Id;
                }
                if (!string.IsNullOrEmpty(ClientSideComponentProperties))
                {
                    fieldCI.ClientSideComponentProperties = ClientSideComponentProperties;
                }
#endif

                if (Type == FieldType.Choice || Type == FieldType.MultiChoice)
                {
                    f = SelectedWeb.CreateField<FieldChoice>(fieldCI);
                    ((FieldChoice)f).Choices = choiceFieldParameters.Choices;
                    f.Update();
                    ClientContext.ExecuteQueryRetry();
                }
                else if (Type == FieldType.Calculated)
                {
                    f = SelectedWeb.CreateField<FieldCalculated>(fieldCI);
                    ((FieldCalculated)f).Formula = calculatedFieldParameters.Formula;
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
                            var calculatedField = ClientContext.CastTo<FieldCalculated>(f);
                            calculatedField.EnsureProperty(fc => fc.Formula);
                            WriteObject(calculatedField);
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

        public class CalculatedFieldDynamicParameters
        {
            [Parameter(Mandatory = true)]
            public string Formula;
        }

    }

}
