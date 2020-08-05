using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Enums;
using Resources = PnP.PowerShell.Commands.Properties.Resources;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.Branding
{
    [Cmdlet(VerbsCommon.Remove, "PnPJavaScriptLink", SupportsShouldProcess = true)]
    [CmdletHelp("Removes a JavaScript link or block from a web or sitecollection",
        Category = CmdletHelpCategory.Branding)]
    [CmdletExample(Code = "PS:> Remove-PnPJavaScriptLink -Identity jQuery",
                Remarks = "Removes the injected JavaScript file with the name jQuery from the current web after confirmation",
                SortOrder = 1)]
    [CmdletExample(Code = "PS:> Remove-PnPJavaScriptLink -Identity jQuery -Scope Site",
                Remarks = "Removes the injected JavaScript file with the name jQuery from the current site collection after confirmation",
                SortOrder = 2)]
    [CmdletExample(Code = "PS:> Remove-PnPJavaScriptLink -Identity jQuery -Scope Site -Confirm:$false",
                Remarks = "Removes the injected JavaScript file with the name jQuery from the current site collection and will not ask for confirmation",
                SortOrder = 3)]
    [CmdletExample(Code = "PS:> Remove-PnPJavaScriptLink -Scope Site",
                Remarks = "Removes all the injected JavaScript files from the current site collection after confirmation for each of them",
                SortOrder = 4)]
    [CmdletExample(Code = "PS:> Remove-PnPJavaScriptLink -Identity faea0ce2-f0c2-4d45-a4dc-73898f3c2f2e -Scope All",
                Remarks = "Removes the injected JavaScript file with id faea0ce2-f0c2-4d45-a4dc-73898f3c2f2e from both the Web and Site scopes",
                SortOrder = 5)]
    [CmdletExample(Code = "PS:> Get-PnPJavaScriptLink -Scope All | ? Sequence -gt 1000 | Remove-PnPJavaScriptLink",
                Remarks = "Removes all the injected JavaScript files from both the Web and Site scope that have a sequence number higher than 1000",
                SortOrder = 6)]
    public class RemoveJavaScriptLink : PnPWebCmdlet
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true, Position = 0, HelpMessage = "Name or id of the JavaScriptLink to remove. Omit if you want to remove all JavaScript Links.")]
        [Alias("Key", "Name")]
        public UserCustomActionPipeBind Identity;

        [Parameter(Mandatory = false)]
        [Obsolete("Use Scope parameter")]
        public SwitchParameter FromSite;

        [Parameter(Mandatory = false, HelpMessage = "Use the -Force flag to bypass the confirmation question")]
        public SwitchParameter Force;

        [Parameter(Mandatory = false, HelpMessage = "Define if the JavaScriptLink is to be found at the web or site collection scope. Specify All to allow deletion from either web or site collection.")]
        public CustomActionScope Scope = CustomActionScope.Web;

        protected override void ExecuteCmdlet()
        {
            // Following code to handle deprecated parameter
#pragma warning disable CS0618 // Type or member is obsolete
            if (ParameterSpecified(nameof(FromSite)))
#pragma warning restore CS0618 // Type or member is obsolete
            {
                Scope = CustomActionScope.Site;
            }

            List<UserCustomAction> actions = new List<UserCustomAction>();

            if (Identity != null && Identity.UserCustomAction != null && Identity.UserCustomAction.Location == "ScriptLink")
            {
                actions.Add(Identity.UserCustomAction);
            }
            else
            {
                if (Scope == CustomActionScope.All || Scope == CustomActionScope.Web)
                {
                    actions.AddRange(SelectedWeb.GetCustomActions().Where(c => c.Location == "ScriptLink"));
                }
                if (Scope == CustomActionScope.All || Scope == CustomActionScope.Site)
                {
                    actions.AddRange(ClientContext.Site.GetCustomActions().Where(c => c.Location == "ScriptLink"));
                }

                if (Identity != null)
                {
                    actions = actions.Where(action => Identity.Id.HasValue ? Identity.Id.Value == action.Id : Identity.Name == action.Name).ToList();

                    if (!actions.Any())
                    {
                        throw new ArgumentException($"No JavaScriptLink found with the {(Identity.Id.HasValue ? "Id" : "name")} '{(Identity.Id.HasValue ? Identity.Id.Value.ToString() : Identity.Name)}' within the scope '{Scope}'", "Identity");
                    }
                }

                if (!actions.Any())
                {
                    WriteVerbose($"No JavaScriptLink registrations found within the scope '{Scope}'");
                    return;
                }
            }

            foreach (var action in actions.Where(action => Force || (ParameterSpecified("Confirm") && !bool.Parse(MyInvocation.BoundParameters["Confirm"].ToString())) || ShouldContinue(string.Format(Resources.RemoveJavaScript0, action.Name, action.Id, action.Scope), Resources.Confirm)))
            {
                switch (action.Scope)
                {
                    case UserCustomActionScope.Web:
                        SelectedWeb.DeleteJsLink(action.Name);
                        break;

                    case UserCustomActionScope.Site:
                        ClientContext.Site.DeleteJsLink(action.Name);
                        break;
                }
            }
        }
    }
}