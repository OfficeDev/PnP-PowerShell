using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Enums;
using Resources = SharePointPnP.PowerShell.Commands.Properties.Resources;

namespace SharePointPnP.PowerShell.Commands.Branding
{
    [Cmdlet(VerbsCommon.Remove, "PnPJavaScriptLink", SupportsShouldProcess = true)]
    [CmdletHelp("Removes a JavaScript link or block from a web or sitecollection",
        Category = CmdletHelpCategory.Branding)]
    [CmdletExample(Code = "PS:> Remove-PnPJavaScriptLink -Name jQuery",
                Remarks = "Removes the injected JavaScript file with the name jQuery from the current web after confirmation",
                SortOrder = 1)]
    [CmdletExample(Code = "PS:> Remove-PnPJavaScriptLink -Name jQuery -Scope Site",
                Remarks = "Removes the injected JavaScript file with the name jQuery from the current site collection after confirmation",
                SortOrder = 2)]
    [CmdletExample(Code = "PS:> Remove-PnPJavaScriptLink -Name jQuery -Scope Site -Force",
                Remarks = "Removes the injected JavaScript file with the name jQuery from the current site collection and will not ask for confirmation",
                SortOrder = 3)]
    [CmdletExample(Code = "PS:> Remove-PnPJavaScriptLink -Scope Site",
                Remarks = "Removes all the injected JavaScript files with from the current site collection after confirmation for each of them",
                SortOrder = 4)]
    public class RemoveJavaScriptLink : PnPWebCmdlet
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true, Position = 0, HelpMessage = "Name of the JavaScriptLink to remove. Omit if you want to remove all JavaScript Links.")]
        [Alias("Key")]
        public string Name = string.Empty;

        [Parameter(Mandatory = false)]
        [Obsolete("Use Scope parameter")]
        public SwitchParameter FromSite;

        [Parameter(Mandatory = false, HelpMessage = "Use the -Force flag to bypass the confirmation question")]
        public SwitchParameter Force;

        [Parameter(Mandatory = false, HelpMessage = "Define if the JavaScriptLink is to be found at the web or site collection scope. Specify All to allow deletion from either web or site collection.")]
        public CustomActionScope Scope = CustomActionScope.Web;

        protected override void ExecuteCmdlet()
        {
            // Following code to handle desprecated parameter
            if (MyInvocation.BoundParameters.ContainsKey("FromSite"))
            {
                Scope = CustomActionScope.Site;
            }

            List<UserCustomAction> actions = new List<UserCustomAction>();

            if (Scope == CustomActionScope.All || Scope == CustomActionScope.Web)
            {
                actions.AddRange(SelectedWeb.GetCustomActions().Where(c => c.Location == "ScriptLink"));
            }
            if (Scope == CustomActionScope.All || Scope == CustomActionScope.Site)
            {
                actions.AddRange(ClientContext.Site.GetCustomActions().Where(c => c.Location == "ScriptLink"));
            }

            if (!string.IsNullOrEmpty(Name))
            {
                actions = actions.Where(action => action.Name == Name).ToList();

                if (!actions.Any())
                {
                    throw new PSArgumentException($"No JavaScriptLink found with the name '{Name}' within the scope '{Scope}'", "Name");
                }
            }

            if (!actions.Any())
            {
                WriteVerbose($"No JavaScriptLink registrations found within the scope '{Scope}'");
                return;
            }

            foreach (var action in actions.Where(action => Force || (MyInvocation.BoundParameters.ContainsKey("Confirm") && !bool.Parse(MyInvocation.BoundParameters["Confirm"].ToString())) || ShouldContinue(string.Format(Resources.RemoveJavaScript0, action.Name, action.Id, action.Scope), Resources.Confirm)))
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