using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using SharePointPnP.PowerShell.Commands.Enums;
using Resources = SharePointPnP.PowerShell.Commands.Properties.Resources;
using System.Collections.Generic;
using System.Linq;
using System;

namespace SharePointPnP.PowerShell.Commands.Branding
{
    [Cmdlet(VerbsCommon.Remove, "PnPCustomAction", ConfirmImpact = ConfirmImpact.High, SupportsShouldProcess = true)]
    [CmdletHelp("Removes a custom action",
        Category = CmdletHelpCategory.Branding)]
    [CmdletExample(Code = @"PS:> Remove-PnPCustomAction -Identity aa66f67e-46c0-4474-8a82-42bf467d07f2 -Scope All", 
                Remarks = @"Removes the custom action with the id 'aa66f67e-46c0-4474-8a82-42bf467d07f2' from the web and site", 
                SortOrder = 1)]
    [CmdletExample(Code = @"PS:> Remove-PnPCustomAction -Identity aa66f67e-46c0-4474-8a82-42bf467d07f2 -Scope Web", 
                Remarks = @"Removes the custom action with the id 'aa66f67e-46c0-4474-8a82-42bf467d07f2' from the current web", 
                SortOrder = 2)]
    [CmdletExample(Code = @"PS:> Remove-PnPCustomAction -Identity jQuery -Scope Site -Confirm:$false", 
                Remarks = @"Removes the custom action with the name 'jQuery' from the current site without asking for confirmation", 
                SortOrder = 3)]
    [CmdletExample(Code = @"PS:> Get-PnPCustomAction -Scope All | ? Location -eq ScriptLink | Remove-PnPCustomAction",
                Remarks = @"Removes all custom actions that are ScriptLinks",
                SortOrder = 4)]
    public class RemoveCustomAction : PnPWebCmdlet
    {
        [Parameter(Mandatory = false, Position = 0, ValueFromPipeline = true, HelpMessage = "The id or name of the CustomAction that needs to be removed or a CustomAction instance itself")]
        public UserCustomActionPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "Define if the CustomAction is to be found at the web or site collection scope. Specify All to allow deletion from either web or site collection.")]
        public CustomActionScope Scope = CustomActionScope.Web;

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
                        throw new ArgumentException($"No CustomAction found with the {(Identity.Id.HasValue ? "Id" : "name")} '{(Identity.Id.HasValue ? Identity.Id.Value.ToString() : Identity.Name)}' within the scope '{Scope}'", "Identity");
                    }
                }

                if (!actions.Any())
                {
                    WriteVerbose($"No CustomAction found within the scope '{Scope}'");
                    return;
                }
            }

            foreach (var action in actions.Where(action => Force || (MyInvocation.BoundParameters.ContainsKey("Confirm") && !bool.Parse(MyInvocation.BoundParameters["Confirm"].ToString())) || ShouldContinue(string.Format(Resources.RemoveCustomAction, action.Name, action.Id, action.Scope), Resources.Confirm)))
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
