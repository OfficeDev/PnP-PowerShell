using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using OfficeDevPnP.Core.Entities;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.Fields
{
    [Cmdlet(VerbsCommon.Add, "PnPTaxonomyField")]
    [CmdletAlias("Add-SPOTaxonomyField")]
    [CmdletHelp("Adds a taxonomy field to a list or as a site column.",
        Category = CmdletHelpCategory.Fields,
        OutputType = typeof(Field),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.field.aspx")]
    [CmdletExample(
        Code = @"PS:> Add-PnPTaxonomyField -DisplayName ""Test"" -InternalName ""Test"" -TermSetPath ""TestTermGroup|TestTermSet""",
        Remarks = @"Adds a new taxonomy field called ""Test"" that points to the TestTermSet which is located in the TestTermGroup",
        SortOrder = 1)]
    public class AddTaxonomyField : PnPWebCmdlet
    {
        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets, HelpMessage = "The list object or name where this field needs to be added")]
        public ListPipeBind List;

        [Parameter(Mandatory = true, ParameterSetName = ParameterAttribute.AllParameterSets, HelpMessage = "The display name of the field")]
        public string DisplayName;

        [Parameter(Mandatory = true, ParameterSetName = ParameterAttribute.AllParameterSets, HelpMessage = "The internal name of the field")]
        public string InternalName;

        [Parameter(Mandatory = true, ParameterSetName = "Path", HelpMessage = "The path to the term that this needs be be bound")]
        public string TermSetPath;

        [Parameter(Mandatory = false, ParameterSetName = "Id", HelpMessage = "The ID of the Taxonomy item")]
        public GuidPipeBind TaxonomyItemId;

        [Parameter(Mandatory = false, ParameterSetName = "Path", HelpMessage = "The path delimiter to be used, by default this is '|'")]
        public string TermPathDelimiter = "|";

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets, HelpMessage = "The group name to where this field belongs to")]
        public string Group;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets, HelpMessage = "The ID for the field, must be unique")]
        public GuidPipeBind Id = new GuidPipeBind();

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets, HelpMessage = "Switch Parameter if this field must be added to the default view")]
        public SwitchParameter AddToDefaultView;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets, HelpMessage = "Switch Parameter if this Taxonomy field can hold multiple values")]
        public SwitchParameter MultiValue;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets, HelpMessage = "Switch Parameter if the field is a required field")]
        public SwitchParameter Required;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets, HelpMessage = "Specifies the control settings while adding a field. See https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.addfieldoptions.aspx for details")]
        public AddFieldOptions FieldOptions = AddFieldOptions.DefaultValue;


        protected override void ExecuteCmdlet()
        {
            TaxonomyItem taxItem;
            Field field;
            if (ParameterSetName == "Path")
            {
                taxItem = ClientContext.Site.GetTaxonomyItemByPath(TermSetPath, TermPathDelimiter);
            }
            else
            {
                var taxSession = ClientContext.Site.GetTaxonomySession();
                var termStore = taxSession.GetDefaultKeywordsTermStore();
                try
                {
                    taxItem = termStore.GetTermSet(TaxonomyItemId.Id);
                }
                catch
                {
                    try
                    {
                        taxItem = termStore.GetTerm(TaxonomyItemId.Id);
                    }
                    catch
                    {
                        throw new Exception($"Taxonomy Item with Id {TaxonomyItemId.Id} not found");
                    }
                }
                taxItem.EnsureProperty(t => t.Id);
            }

            Guid id = Id.Id;
            if (id == Guid.Empty)
            {
                id = Guid.NewGuid();
            }

            var fieldCI = new TaxonomyFieldCreationInformation()
            {
                Id = id,
                InternalName = InternalName,
                DisplayName = DisplayName,
                Group = Group,
                TaxonomyItem = taxItem,
                MultiValue = MultiValue,
                Required = Required,
                AddToDefaultView = AddToDefaultView
            };

            if (List != null)
            {
                var list = List.GetList(SelectedWeb);
                field = list.CreateTaxonomyField(fieldCI);
            }
            else
            {
                field = SelectedWeb.CreateTaxonomyField(fieldCI);
            }
            WriteObject(ClientContext.CastTo<TaxonomyField>(field));
        }

    }

}
