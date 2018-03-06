using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.Site
{
    [Cmdlet(VerbsCommon.Remove, "PnPRoleDefinition")]
    [CmdletHelp("Remove a Role Definition from a site",
        Category = CmdletHelpCategory.Sites)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPRoleDefinition -Identity MyRoleDefinition",
        Remarks = "Removes the specified Role Definition (Permission Level) from the current site",
        SortOrder = 1)]
    public class RemoveRoleDefinition : PnPCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The identity of the role definition, either a RoleDefinition object or a the name of roledefinition")]
        public RoleDefinitionPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "Do not ask for confirmation to delete the role definition", ParameterSetName = ParameterAttribute.AllParameterSets)]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            var roleDefinition = Identity.GetRoleDefinition(ClientContext.Site);
            if (roleDefinition != null)
            {
                try
                {
                    if (Force || ShouldContinue($@"Remove Role Definition ""{roleDefinition.Name}""?", "Confirm"))
                    {
                        roleDefinition.DeleteObject();
                        ClientContext.ExecuteQueryRetry();
                        WriteVerbose($@"Removed Role Definition ""{roleDefinition.Name}""");
                    }
                }
                catch (ServerException e)
                {
                    WriteWarning($@"Exception occurred while trying to remove the Role Definition: ""{e.Message}"". Will be skipped.");
                }
            }
            else
            {
                WriteWarning($"Unable to remove Role Definition as it wasn't found. Will be skipped.");
            }
        }
    }
}
