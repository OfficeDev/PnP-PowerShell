using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.Site
{
    [Cmdlet(VerbsCommon.Get, "PnPRoleDefinition")]
    [CmdletHelp("Retrieves a Role Definitions of a site",
        Category = CmdletHelpCategory.Sites,
        OutputType = typeof(RoleDefinition),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.roledefinition.aspx")]
    [CmdletExample(
        Code = @"PS:> Get-PnPRoleDefinition",
        Remarks = "Retrieves the Role Definitions (Permission Levels) settings of the current site",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPRoleDefinition -Identity Read",
        Remarks = "Retrieves the specified Role Definition (Permission Level) settings of the current site",
        SortOrder = 2)]
    public class GetRoleDefinition : PnPCmdlet
    {
        [Parameter(Mandatory = false, Position = 0, ValueFromPipeline = true, HelpMessage = "The name of a role definition to retrieve.")]
        public RoleDefinitionPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            if (MyInvocation.BoundParameters.ContainsKey("Identity"))
            {
                var roleDefinition = Identity.GetRoleDefinition(ClientContext.Site);
                ClientContext.Load(roleDefinition);
                ClientContext.ExecuteQueryRetry();
                WriteObject(roleDefinition);
            }
            else
            {
                var roleDefinitions = ClientContext.Site.RootWeb.RoleDefinitions;
                ClientContext.Load(roleDefinitions);
                ClientContext.ExecuteQueryRetry();
                WriteObject(roleDefinitions);
            }
        }
    }
}
