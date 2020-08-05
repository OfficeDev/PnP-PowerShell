using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using PnP.PowerShell.Commands.Enums;

namespace PnP.PowerShell.Commands.Branding
{
    [Cmdlet(VerbsCommon.Get, "PnPCustomAction")]
    [CmdletHelp("Return user custom actions",
        "Returns all or a specific user custom action",
        Category = CmdletHelpCategory.Branding,
        OutputType = typeof(List<UserCustomAction>),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.usercustomaction.aspx")]
    [CmdletExample(Code = @"PS:> Get-PnPCustomAction", 
                   Remarks = @"Returns all custom actions of the current site.", 
                   SortOrder = 1)]
    [CmdletExample(Code = @"PS:> Get-PnPCustomAction -Identity aa66f67e-46c0-4474-8a82-42bf467d07f2", 
                   Remarks = @"Returns the custom action with the id 'aa66f67e-46c0-4474-8a82-42bf467d07f2'.", 
                   SortOrder = 2)]
    [CmdletExample(Code = @"PS:> Get-PnPCustomAction -Scope web", 
                   Remarks = @"Returns all custom actions for the current web object.", 
                   SortOrder = 3)]
    public class GetCustomAction : PnPWebRetrievalsCmdlet<UserCustomAction>
    {
        [Parameter(Mandatory = false, HelpMessage = "Identity of the CustomAction to return. Omit to return all CustomActions.")]
        public GuidPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "Scope of the CustomAction, either Web, Site or All to return both")]
        public CustomActionScope Scope = CustomActionScope.Web;

        [Parameter(Mandatory = false, HelpMessage = "Switch parameter if an exception should be thrown if the requested CustomAction does not exist (true) or if omitted, nothing will be returned in case the CustomAction does not exist")]
        public SwitchParameter ThrowExceptionIfCustomActionNotFound;

        protected override void ExecuteCmdlet()
        {
            List<UserCustomAction> actions = new List<UserCustomAction>();

            if (Scope == CustomActionScope.All || Scope == CustomActionScope.Web)
            {
                actions.AddRange(SelectedWeb.GetCustomActions(RetrievalExpressions));
            }
            if (Scope == CustomActionScope.All || Scope == CustomActionScope.Site)
            {
                actions.AddRange(ClientContext.Site.GetCustomActions(RetrievalExpressions));
            }

            if (Identity != null)
            {
                var foundAction = actions.FirstOrDefault(x => x.Id == Identity.Id);
                if (foundAction != null || !ThrowExceptionIfCustomActionNotFound)
                {
                    WriteObject(foundAction, true);
                }
                else
                {
                    throw new PSArgumentException($"No CustomAction found with the Identity '{Identity.Id}' within the scope '{Scope}'", "Identity");
                }
            }
            else
            {
                WriteObject(actions, true);
            }
        }
    }
}