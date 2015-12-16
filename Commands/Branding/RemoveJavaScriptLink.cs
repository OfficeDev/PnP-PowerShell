using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.PowerShell.CmdletHelpAttributes;
using OfficeDevPnP.PowerShell.Commands.Enums;
using Resources = OfficeDevPnP.PowerShell.Commands.Properties.Resources;
using System;
using System.Collections.Generic;

namespace OfficeDevPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Remove, "SPOJavaScriptLink", SupportsShouldProcess = true)]
    [CmdletHelp("Removes a JavaScript link or block from a web or sitecollection",
        Category = CmdletHelpCategory.Branding)]
    [CmdletExample(Code = "PS:> Remove-SPOJavaScriptLink -Name jQuery",
                Remarks = "Removes the injected JavaScript file with the name jQuery from the current web after confirmation",
                SortOrder = 1)]
    [CmdletExample(Code = "PS:> Remove-SPOJavaScriptLink -Name jQuery -Scope Site",
                Remarks = "Removes the injected JavaScript file with the name jQuery from the current site collection after confirmation",
                SortOrder = 2)]
    [CmdletExample(Code = "PS:> Remove-SPOJavaScriptLink -Name jQuery -Scope Site -Force",
                Remarks = "Removes the injected JavaScript file with the name jQuery from the current site collection and will not ask for confirmation",
                SortOrder = 3)]
    public class RemoveJavaScriptLink : SPOWebCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "Name of the JavaScriptLink to remove")]
        [Alias("Key")]
        public string Name = string.Empty;

        [Parameter(Mandatory = false)]
        [Obsolete("Use Scope parameter")]
        public SwitchParameter FromSite;

        [Parameter(Mandatory = false, HelpMessage = "Use the -Force flag to bypass the confirmation question")]
        public SwitchParameter Force;

        [Parameter(Mandatory = false, HelpMessage = "Define if the JavaScriptLink is to be found at the web or site collection scope. Omit to allow deletion from either web or site collection.")]
        public CustomActionScope? Scope;

        protected override void ExecuteCmdlet()
        {
            // Following code to handle desprecated parameter
            if (MyInvocation.BoundParameters.ContainsKey("FromSite"))
            {
                Scope = CustomActionScope.Site;
            }

            List<UserCustomAction> actions = new List<UserCustomAction>();

            if (!Scope.HasValue || Scope == CustomActionScope.Web)
            {
                actions.AddRange(SelectedWeb.GetCustomActions().Where(c => c.Location == "ScriptLink"));
            }
            if (!Scope.HasValue || Scope == CustomActionScope.Site)
            {
                actions.AddRange(ClientContext.Site.GetCustomActions().Where(c => c.Location == "ScriptLink"));
            }

            if (!actions.Any()) return;

            foreach (var action in actions.Where(action => Force || ShouldContinue(string.Format(Resources.RemoveJavaScript0, action.Name), Resources.Confirm)))
            {
                switch (action.Scope)
                {
                    case UserCustomActionScope.Web:
                        SelectedWeb.DeleteJsLink(Name);
                        break;

                    case UserCustomActionScope.Site:
                        ClientContext.Site.DeleteJsLink(Name);
                        break;
                }
            }
        }
    }
}
