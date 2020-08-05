#if !SP2013 && !SP2016
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using PnP.PowerShell.Commands.Enums;
using Resources = PnP.PowerShell.Commands.Properties.Resources;
using System.Collections.Generic;
using System.Linq;

namespace PnP.PowerShell.Commands.Branding
{
    [Cmdlet(VerbsCommon.Remove, "PnPApplicationCustomizer", ConfirmImpact = ConfirmImpact.High, SupportsShouldProcess = true)]
    [CmdletHelp("Removes a SharePoint Framework client side extension application customizer",
        "Removes a SharePoint Framework client side extension application customizer by removing a user custom action from a web or sitecollection",
        Category = CmdletHelpCategory.Apps,
        SupportedPlatform = CmdletSupportedPlatform.Online | CmdletSupportedPlatform.SP2019)]
    [CmdletExample(Code = @"PS:> Remove-PnPApplicationCustomizer -Identity aa66f67e-46c0-4474-8a82-42bf467d07f2", 
                   Remarks = @"Removes the custom action representing the client side extension registration with the id 'aa66f67e-46c0-4474-8a82-42bf467d07f2'.", 
                   SortOrder = 1)]
    [CmdletExample(Code = @"PS:> Remove-PnPApplicationCustomizer -ClientSideComponentId aa66f67e-46c0-4474-8a82-42bf467d07f2 -Scope web", 
                   Remarks = @"Removes the custom action(s) being registered for a SharePoint Framework solution having the id 'aa66f67e-46c0-4474-8a82-42bf467d07f2' in its manifest from the current web.", 
                   SortOrder = 2)]
    public class RemoveApplicationCustomizer : PnPWebCmdlet
    {
        private const string ParameterSet_CUSTOMACTIONID = "Custom Action Id";
        private const string ParameterSet_CLIENTSIDECOMPONENTID = "Client Side Component Id";

        [Parameter(Mandatory = false, Position = 0, ValueFromPipeline = true, HelpMessage = "The id or name of the CustomAction representing the client side extension registration that needs to be removed or a CustomAction instance itself", ParameterSetName = ParameterSet_CUSTOMACTIONID)]
        public UserCustomActionPipeBind Identity;

        [Parameter(Mandatory = true, HelpMessage = "The Client Side Component Id of the SharePoint Framework client side extension application customizer found in the manifest for which existing custom action(s) should be removed", ParameterSetName = ParameterSet_CLIENTSIDECOMPONENTID)]
        public GuidPipeBind ClientSideComponentId;

        [Parameter(Mandatory = false, HelpMessage = "Define if the CustomAction representing the client side extension registration is to be found at the web or site collection scope. Specify All to allow deletion from either web or site collection (default).")]
        public CustomActionScope Scope = CustomActionScope.All;

        [Parameter(Mandatory = false, HelpMessage = "Use the -Force flag to bypass the confirmation question")]
        public SwitchParameter Force;

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

            // Only take the customactions which are application customizers
            actions = actions.Where(a => a.Location == "ClientSideExtension.ApplicationCustomizer").ToList();

            // If a ClientSideComponentId has been provided, only leave those who have a matching client side component id
            if (ParameterSetName == ParameterSet_CLIENTSIDECOMPONENTID)
            {
                actions = actions.Where(a => a.ClientSideComponentId == ClientSideComponentId.Id).ToList();
            }

            if (!actions.Any())
            {
                WriteVerbose($"No CustomAction representing the client side extension registration found within the scope '{Scope}'");
                return;
            }

            foreach (var action in actions.Where(action => Force || (ParameterSpecified("Confirm") && !bool.Parse(MyInvocation.BoundParameters["Confirm"].ToString())) || ShouldContinue(string.Format(Resources.RemoveCustomAction, action.Name, action.Id, action.Scope), Resources.Confirm)))
            {
                switch (action.Scope)
                {
                    case UserCustomActionScope.Web:
                        SelectedWeb.DeleteCustomAction(action.Id);
                        break;

                    case UserCustomActionScope.Site:
                        ClientContext.Site.DeleteCustomAction(action.Id);
                        break;
                }
            }
        }
    }
}
#endif
