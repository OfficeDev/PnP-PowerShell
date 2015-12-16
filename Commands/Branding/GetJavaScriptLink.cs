using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.PowerShell.CmdletHelpAttributes;
using OfficeDevPnP.PowerShell.Commands.Base.PipeBinds;
using OfficeDevPnP.PowerShell.Commands.Enums;

namespace OfficeDevPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Get, "SPOJavaScriptLink")]
    [CmdletHelp("Returns all or a specific custom action(s) with location type ScriptLink", 
        Category = CmdletHelpCategory.Branding)]
    [CmdletExample(Code = "PS:> Get-SPOJavaScriptLink",
                Remarks = "Returns all web and site scoped JavaScriptLinks",
                SortOrder = 1)]
    [CmdletExample(Code = "PS:> Get-SPOJavaScriptLink -Scope Web",
                Remarks = "Returns all site scoped JavaScriptLinks",
                SortOrder = 2)]
    [CmdletExample(Code = "PS:> Get-SPOJavaScriptLink -Scope Site",
                Remarks = "Returns all web scoped JavaScriptLinks",
                SortOrder = 3)]
    [CmdletExample(Code = "PS:> Get-SPOJavaScriptLink -Name Test",
                Remarks = "Returns the JavaScriptLink named Test",
                SortOrder = 4)]
    public class GetJavaScriptLink : SPOWebCmdlet
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true, Position = 0, HelpMessage = "Name of the Javascript link. Omit this parameter to retrieve all script links")]
        [Alias("Key")]
        public string Name = string.Empty;

        [Parameter(Mandatory = false, HelpMessage = "Scope of the action, either Web, Site or omit to return both")]
        public CustomActionScope? Scope = null;

        protected override void ExecuteCmdlet()
        {
            List<UserCustomAction> actions = new List<UserCustomAction>();

            if (!Scope.HasValue || Scope == CustomActionScope.Web)
            {
                actions.AddRange(SelectedWeb.GetCustomActions().Where(c => c.Location == "ScriptLink"));
            }
            if (!Scope.HasValue || Scope == CustomActionScope.Site)
            {
                actions.AddRange(ClientContext.Site.GetCustomActions().Where(c => c.Location == "ScriptLink"));
            }

            if (!string.IsNullOrEmpty(Name))
            {
                var foundAction = actions.FirstOrDefault(x => x.Name == Name);
                WriteObject(foundAction, true);
            }
            else
            {
                WriteObject(actions, true);
            }
        }
    }
}
