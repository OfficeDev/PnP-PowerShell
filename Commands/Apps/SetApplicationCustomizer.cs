#if !SP2013 && !SP2016
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using PnP.PowerShell.Commands.Enums;
using System.Collections.Generic;
using System.Linq;

namespace PnP.PowerShell.Commands.Branding
{
    [Cmdlet(VerbsCommon.Set, "PnPApplicationCustomizer", ConfirmImpact = ConfirmImpact.High, SupportsShouldProcess = true)]
    [CmdletHelp("Updates a SharePoint Framework client side extension application customizer",
        "Updates a SharePoint Framework client side extension application customizer by updating its custom action. Only the properties that will be provided will be updated. Others will remain as they are.",
        Category = CmdletHelpCategory.Apps,
        SupportedPlatform = CmdletSupportedPlatform.Online | CmdletSupportedPlatform.SP2019)]
    [CmdletExample(Code = @"PS:> Set-PnPApplicationCustomizer -Identity aa66f67e-46c0-4474-8a82-42bf467d07f2", 
                   Remarks = @"Updates the custom action representing the client side extension registration with the id 'aa66f67e-46c0-4474-8a82-42bf467d07f2'.", 
                   SortOrder = 1)]
    [CmdletExample(Code = @"PS:> Set-PnPApplicationCustomizer -ClientSideComponentId aa66f67e-46c0-4474-8a82-42bf467d07f2 -Scope web -ClientSideComponentProperties ""{`""sourceTermSet`"":`""PnP-CollabFooter-SharedLinks`"",`""personalItemsStorageProperty`"":`""PnP-CollabFooter-MyLinks`""}", 
                   Remarks = @"Updates the custom action(s) properties being registered for a SharePoint Framework solution having the id 'aa66f67e-46c0-4474-8a82-42bf467d07f2' in its manifest from the current web.", 
                   SortOrder = 2)]
    public class SetApplicationCustomizer : PnPWebCmdlet
    {
        private const string ParameterSet_CUSTOMACTIONID = "Custom Action Id";
        private const string ParameterSet_CLIENTSIDECOMPONENTID = "Client Side Component Id";

        [Parameter(Mandatory = false, Position = 0, ValueFromPipeline = true, HelpMessage = "The id or name of the CustomAction representing the client side extension registration that needs to be updated or a CustomAction instance itself", ParameterSetName = ParameterSet_CUSTOMACTIONID)]
        public UserCustomActionPipeBind Identity = null;

        [Parameter(Mandatory = false, HelpMessage = "The Client Side Component Id of the SharePoint Framework client side extension application customizer found in the manifest for which existing custom action(s) should be updated", ParameterSetName = ParameterSet_CLIENTSIDECOMPONENTID)]
        public GuidPipeBind ClientSideComponentId = null;

        [Parameter(Mandatory = false, HelpMessage = "Define if the CustomAction representing the client side extension registration is to be found at the web or site collection scope. Specify All to update the component on both web and site collection level.")]
        public CustomActionScope Scope = CustomActionScope.Web;

        [Parameter(Mandatory = false, HelpMessage = "The title of the application customizer. Omit to not update this property.")]
        public string Title = null;

        [Parameter(Mandatory = false, HelpMessage = "The description of the application customizer. Omit to not update this property.")]
        public string Description = null;

        [Parameter(Mandatory = false, HelpMessage = "Sequence of this application customizer being injected. Use when you have a specific sequence with which to have multiple application customizers being added to the page. Omit to not update this property.")]
        public int? Sequence = null;

        [Parameter(Mandatory = false, HelpMessage = "The Client Side Component Properties of the application customizer to update. Specify values as a json string : \"{Property1 : 'Value1', Property2: 'Value2'}\". Omit to not update this property.")]
        public string ClientSideComponentProperties = null;

        protected override void ExecuteCmdlet()
        {
            List<UserCustomAction> actions = new List<UserCustomAction>();

            if (Identity != null && Identity.UserCustomAction != null)
            {
                actions.Add(Identity.UserCustomAction);
            }
            else
            {
                if (Scope == CustomActionScope.All || Scope == CustomActionScope.Web)
                {
                    actions.AddRange(SelectedWeb.GetCustomActions());
                }
                if (Scope == CustomActionScope.All || Scope == CustomActionScope.Site)
                {
                    actions.AddRange(ClientContext.Site.GetCustomActions());
                }

                if (Identity != null)
                {
                    actions = actions.Where(action => Identity.Id.HasValue ? Identity.Id.Value == action.Id : Identity.Name == action.Name).ToList();

                    if (!actions.Any())
                    {
                        throw new PSArgumentException($"No CustomAction representing the client side extension registration found with the {(Identity.Id.HasValue ? "Id" : "name")} '{(Identity.Id.HasValue ? Identity.Id.Value.ToString() : Identity.Name)}' within the scope '{Scope}'", "Identity");
                    }
                }
            }
            
            // If a ClientSideComponentId has been provided, only leave those who have a matching client side component id
            if(ParameterSetName == ParameterSet_CLIENTSIDECOMPONENTID)
            {
                actions = actions.Where(a => a.ClientSideComponentId == ClientSideComponentId.Id && a.Location == "ClientSideExtension.ApplicationCustomizer").ToList();
            }

            if (!actions.Any())
            {
                WriteVerbose($"No CustomAction representing the client side extension registration found within the scope '{Scope}'");
                return;
            }

            // Update each of the matched custom actions
            foreach (var action in actions)
            {
                bool isDirty = false;

                if(Title != null)
                {
                    action.Title = Title;
                    isDirty = true;
                }
                if(Description != null)
                {
                    action.Description = Description;
                    isDirty = true;
                }
                if (Sequence.HasValue)
                {
                    action.Sequence = Sequence.Value;
                    isDirty = true;
                }
                if (ClientSideComponentProperties != null)
                {
                    action.ClientSideComponentProperties = ClientSideComponentProperties;
                    isDirty = true;
                }

                if (isDirty)
                {
                    action.Update();
                    ClientContext.ExecuteQueryRetry();
                }
            }
        }
    }
}
#endif
