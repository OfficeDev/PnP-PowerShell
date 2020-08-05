using System.Management.Automation;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Entities;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Enums;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.Branding
{
    [Cmdlet(VerbsCommon.Add, "PnPCustomAction")]
    [CmdletHelp("Adds a custom action",
        "Adds a user custom action to a web or sitecollection.",
        Category = CmdletHelpCategory.Branding)]
    [CmdletExample(Code = @"$cUIExtn = ""<CommandUIExtension><CommandUIDefinitions><CommandUIDefinition Location=""""Ribbon.List.Share.Controls._children""""><Button Id=""""Ribbon.List.Share.GetItemsCountButton"""" Alt=""""Get list items count"""" Sequence=""""11"""" Command=""""Invoke_GetItemsCountButtonRequest"""" LabelText=""""Get Items Count"""" TemplateAlias=""""o1"""" Image32by32=""""_layouts/15/images/placeholder32x32.png"""" Image16by16=""""_layouts/15/images/placeholder16x16.png"""" /></CommandUIDefinition></CommandUIDefinitions><CommandUIHandlers><CommandUIHandler Command=""""Invoke_GetItemsCountButtonRequest"""" CommandAction=""""javascript: alert('Total items in this list: '+ ctx.TotalListItems);"""" EnabledScript=""""javascript: function checkEnable() { return (true);} checkEnable();""""/></CommandUIHandlers></CommandUIExtension>""

Add-PnPCustomAction -Name 'GetItemsCount' -Title 'Invoke GetItemsCount Action' -Description 'Adds custom action to custom list ribbon' -Group 'SiteActions' -Location 'CommandUI.Ribbon' -CommandUIExtension $cUIExtn",
    Remarks = @"Adds a new custom action to the custom list template, and sets the Title, Name and other fields with the specified values. On click it shows the number of items in that list. Notice: escape quotes in CommandUIExtension.",
    SortOrder = 1)]
    [CmdletExample(Code = @"Add-PnPCustomAction -Title ""CollabFooter"" -Name ""CollabFooter"" -Location ""ClientSideExtension.ApplicationCustomizer"" -ClientSideComponentId c0ab3b94-8609-40cf-861e-2a1759170b43 -ClientSideComponentProperties ""{`""sourceTermSet`"":`""PnP-CollabFooter-SharedLinks`"",`""personalItemsStorageProperty`"":`""PnP-CollabFooter-MyLinks`""}",
    Remarks = @"Adds a new application customizer to the site. This requires that an SPFX solution has been deployed containing the application customizer specified. Be sure to run Install-PnPApp before trying this cmdlet on a site.",
    SortOrder = 2)]
    [CmdletRelatedLink(
        Text = "UserCustomAction",
        Url = "https://docs.microsoft.com/en-us/previous-versions/office/sharepoint-server/ee539583(v=office.15)")]
    [CmdletRelatedLink(
        Text = "BasePermissions",
        Url = "https://docs.microsoft.com/en-us/previous-versions/office/sharepoint-server/ee543321(v=office.15)")]
    public class AddCustomAction : PnPWebCmdlet
    {
        private const string ParameterSet_DEFAULT = "Default";
        private const string ParameterSet_CLIENTSIDECOMPONENTID = "Client Side Component Id";
        [Parameter(Mandatory = true, HelpMessage = "The name of the custom action", ParameterSetName = ParameterSet_DEFAULT)]
#if !SP2013 && !SP2016
        [Parameter(Mandatory = true, HelpMessage = "The name of the custom action", ParameterSetName = ParameterSet_CLIENTSIDECOMPONENTID)]
#endif
        public string Name = string.Empty;

        [Parameter(Mandatory = true, HelpMessage = "The title of the custom action", ParameterSetName = ParameterSet_DEFAULT)]
#if !SP2013 && !SP2016
        [Parameter(Mandatory = true, HelpMessage = "The title of the custom action", ParameterSetName = ParameterSet_CLIENTSIDECOMPONENTID)]
#endif
        public string Title = string.Empty;

        [Parameter(Mandatory = true, HelpMessage = "The description of the custom action", ParameterSetName = ParameterSet_DEFAULT)]
        public string Description = string.Empty;

        [Parameter(Mandatory = true, HelpMessage = "The group where this custom action needs to be added like 'SiteActions'", ParameterSetName = ParameterSet_DEFAULT)]
        public string Group = string.Empty;

        [Parameter(Mandatory = true, HelpMessage = "The actual location where this custom action need to be added like 'CommandUI.Ribbon'", ParameterSetName = ParameterSet_DEFAULT)]
#if !SP2013 && !SP2016
        [Parameter(Mandatory = true, HelpMessage = "The actual location where this custom action need to be added like 'CommandUI.Ribbon'", ParameterSetName = ParameterSet_CLIENTSIDECOMPONENTID)]
#endif
        public string Location = string.Empty;

        [Parameter(Mandatory = false, HelpMessage = "Sequence of this CustomAction being injected. Use when you have a specific sequence with which to have multiple CustomActions being added to the page.", ParameterSetName = ParameterSet_DEFAULT)]
#if !SP2013 && !SP2016
        [Parameter(Mandatory = false, HelpMessage = "Optional activation sequence order for the extensions. Used if multiple extensions are activated on a same scope.", ParameterSetName = ParameterSet_CLIENTSIDECOMPONENTID)]
#endif
        public int Sequence = 0;

        [Parameter(Mandatory = false, HelpMessage = "The URL, URI or ECMAScript (JScript, JavaScript) function associated with the action", ParameterSetName = ParameterSet_DEFAULT)]
        public string Url = string.Empty;

        [Parameter(Mandatory = false, HelpMessage = "The URL of the image associated with the custom action", ParameterSetName = ParameterSet_DEFAULT)]
        public string ImageUrl = string.Empty;

        [Parameter(Mandatory = false, HelpMessage = "XML fragment that determines user interface properties of the custom action", ParameterSetName = ParameterSet_DEFAULT)]
        public string CommandUIExtension = string.Empty;

        [Parameter(Mandatory = false, HelpMessage = "The identifier of the object associated with the custom action.", ParameterSetName = ParameterSet_DEFAULT)]
#if !SP2013 && !SP2016
        [Parameter(Mandatory = false, HelpMessage = "The identifier of the object associated with the custom action.", ParameterSetName = ParameterSet_CLIENTSIDECOMPONENTID)]
#endif
        public string RegistrationId = string.Empty;

        [Parameter(Mandatory = false, HelpMessage = "A string array that contain the permissions needed for the custom action", ParameterSetName = ParameterSet_DEFAULT)]
        public PermissionKind[] Rights;

        [Parameter(Mandatory = false, HelpMessage = "Specifies the type of object associated with the custom action", ParameterSetName = ParameterSet_DEFAULT)]
#if !SP2013 && !SP2016
        [Parameter(Mandatory = false, HelpMessage = "Specifies the type of object associated with the custom action", ParameterSetName = ParameterSet_CLIENTSIDECOMPONENTID)]
#endif
        public UserCustomActionRegistrationType RegistrationType;

        [Parameter(Mandatory = false, HelpMessage = "The scope of the CustomAction to add to. Either Web or Site; defaults to Web. 'All' is not valid for this command.", ParameterSetName = ParameterSet_DEFAULT)]
#if !SP2013 && !SP2016
        [Parameter(Mandatory = false, HelpMessage = "The scope of the CustomAction to add to. Either Web or Site; defaults to Web. 'All' is not valid for this command.", ParameterSetName = ParameterSet_CLIENTSIDECOMPONENTID)]
#endif
        public CustomActionScope Scope = CustomActionScope.Web;
#if !SP2013 && !SP2016
        [Parameter(Mandatory = true, HelpMessage = "The Client Side Component Id of the custom action", ParameterSetName = ParameterSet_CLIENTSIDECOMPONENTID)]
        public GuidPipeBind ClientSideComponentId;

        [Parameter(Mandatory = false, HelpMessage = "The Client Side Component Properties of the custom action. Specify values as a json string : \"{Property1 : 'Value1', Property2: 'Value2'}\"", ParameterSetName = ParameterSet_CLIENTSIDECOMPONENTID)]
        public string ClientSideComponentProperties;
#endif
#if !ONPREMISES
        [Parameter(Mandatory = false, HelpMessage = "The Client Side Host Properties of the custom action. Specify values as a json string : \"{'preAllocatedApplicationCustomizerTopHeight': '50', 'preAllocatedApplicationCustomizerBottomHeight': '50'}\"", ParameterSetName = ParameterSet_CLIENTSIDECOMPONENTID)]
        public string ClientSideHostProperties;
#endif

        protected override void ExecuteCmdlet()
        {
            var permissions = new BasePermissions();
            if (Rights != null)
            {
                foreach (var kind in Rights)
                {
                    permissions.Set(kind);
                }
            }
            CustomActionEntity ca = null;
            if (ParameterSetName == ParameterSet_DEFAULT)
            {

                ca = new CustomActionEntity
                {
                    Name = Name,
                    ImageUrl = ImageUrl,
                    CommandUIExtension = CommandUIExtension,
                    RegistrationId = RegistrationId,
                    RegistrationType = RegistrationType,
                    Description = Description,
                    Location = Location,
                    Group = Group,
                    Sequence = Sequence,
                    Title = Title,
                    Url = Url,
                    Rights = permissions,
                };
            }
            else
            {
#if !SP2013 && !SP2016
                ca = new CustomActionEntity()
                {
                    Name = Name,
                    Title = Title,
                    Location = Location,
                    Sequence = Sequence,
                    ClientSideComponentId = ClientSideComponentId.Id,
                    ClientSideComponentProperties = ClientSideComponentProperties,
#if !ONPREMISES
                    ClientSideHostProperties = ClientSideHostProperties
#endif
                };

                if (ParameterSpecified(nameof(RegistrationId)))
                {
                    ca.RegistrationId = RegistrationId;
                }

                if (ParameterSpecified(nameof(RegistrationType)))
                {
                    ca.RegistrationType = RegistrationType;
                }
#endif
            }

            switch (Scope)
            {
                case CustomActionScope.Web:
                    SelectedWeb.AddCustomAction(ca);
                    break;

                case CustomActionScope.Site:
                    ClientContext.Site.AddCustomAction(ca);
                    break;

                case CustomActionScope.All:
                    WriteWarning("CustomActionScope 'All' is not supported for adding CustomActions");
                    break;
            }
        }
    }
}
