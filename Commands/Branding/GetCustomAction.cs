using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.PowerShell.CmdletHelpAttributes;
using OfficeDevPnP.PowerShell.Commands.Base.PipeBinds;
using OfficeDevPnP.PowerShell.Commands.Enums;

namespace OfficeDevPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Get, "SPOCustomAction")]
    [CmdletHelp("Returns all or a specific custom action(s)", 
        Category = CmdletHelpCategory.Branding)]
    [CmdletExample(Code = @"PS:> Get-SPOCustomAction", Remarks = @"Returns all custom actions of the current site.", SortOrder = 1)]
    [CmdletExample(Code = @"PS:> Get-SPOCustomAction -Identity aa66f67e-46c0-4474-8a82-42bf467d07f2", Remarks = @"Returns the custom action with the id 'aa66f67e-46c0-4474-8a82-42bf467d07f2'.", SortOrder = 2)]
    [CmdletExample(Code = @"PS:> Get-SPOCustomAction -Scope web", Remarks = @"Returns all custom actions for the current web object.", SortOrder = 3)]
    public class GetCustomAction : SPOWebCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "Identity of the CustomAction to return. Omit to return all CustomActions.")]
        public GuidPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "Scope of the CustomAction, either Web, Site or All to return both")]
        public CustomActionScope Scope = CustomActionScope.Web;

        protected override void ExecuteCmdlet()
        {
            List<UserCustomAction> actions = new List<UserCustomAction>();

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
                var foundAction = actions.FirstOrDefault(x => x.Id == Identity.Id);
                WriteObject(foundAction, true);
            }
            else
            {
                WriteObject(actions,true);
            }
        }
    }
}
