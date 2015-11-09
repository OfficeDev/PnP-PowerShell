using System.Collections.Generic;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Entities;
using OfficeDevPnP.PowerShell.CmdletHelpAttributes;
using OfficeDevPnP.PowerShell.Commands.Enums;
using System.Linq;

namespace OfficeDevPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Add, "SPOCustomAction")]
    [CmdletHelp("Adds a custom action to a web", Category = CmdletHelpCategory.Branding)]
    [CmdletExample(Code = @"$cUIExtn = ""<CommandUIExtension><CommandUIDefinitions><CommandUIDefinition Location=""""Ribbon.List.Share.Controls._children""""><Button Id=""""Ribbon.List.Share.GetItemsCountButton"""" Alt=""""Get list items count"""" Sequence=""""11"""" Command=""""Invoke_GetItemsCountButtonRequest"""" LabelText=""""Get Items Count"""" TemplateAlias=""""o1"""" Image32by32=""""_layouts/15/images/placeholder32x32.png"""" Image16by16=""""_layouts/15/images/placeholder16x16.png"""" /></CommandUIDefinition></CommandUIDefinitions><CommandUIHandlers><CommandUIHandler Command=""""Invoke_GetItemsCountButtonRequest"""" CommandAction=""""javascript: alert('Total items in this list: '+ ctx.TotalListItems);"""" EnabledScript=""""javascript: function checkEnable() { return (true);} checkEnable();""""/></CommandUIHandlers></CommandUIExtension>""

Add-SPOCustomAction -Name 'GetItemsCount' -Title 'Invoke GetItemsCount Action' -Description 'Adds custom action to custom list ribbon' -Group 'Microsoft.SharePoint.Client.UserCustomAction.group' -Location 'CommandUI.Ribbon' -CommandUIExtension $cUIExtn",
    Remarks = @"Adds a new custom action to the custom list template, and sets the Title, Name and other fields with the specified values. On click it shows the number of items in that list. Notice, escape quotes in CommandUIExtension.",
    SortOrder = 1)]
    public class AddCustomAction : SPOWebCmdlet
    {
        [Parameter(Mandatory = true)]
        public string Name = string.Empty;

        [Parameter(Mandatory = true)]
        public string Title = string.Empty;

        [Parameter(Mandatory = true)]
        public string Description = string.Empty;

        [Parameter(Mandatory = true)]
        public string Group = string.Empty;

        [Parameter(Mandatory = true)]
        public string Location = string.Empty;

        [Parameter(Mandatory = false)]
        public int Sequence = 0;

        [Parameter(Mandatory = false)]
        public string Url = string.Empty;

        [Parameter(Mandatory = false)]
        public string ImageUrl = string.Empty;

        [Parameter(Mandatory = false)]
        public string CommandUIExtension = string.Empty;

        [Parameter(Mandatory = false)]
        public string RegistrationId = string.Empty;

        [Parameter(Mandatory = false)]
        public PermissionKind[] Rights;

        [Parameter(Mandatory = false)]
        public UserCustomActionRegistrationType RegistrationType;

        [Parameter(Mandatory = false)]
        public CustomActionScope Scope = CustomActionScope.Web;


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

            var ca = new CustomActionEntity
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
                Rights = permissions
            };

            if (Scope == CustomActionScope.Web)
            {
                SelectedWeb.AddCustomAction(ca);
            }
            else
            {
                ClientContext.Site.AddCustomAction(ca);
            }
        }
    }
}
