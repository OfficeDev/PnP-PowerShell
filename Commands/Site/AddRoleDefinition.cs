using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using System.Collections.Generic;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System;

namespace PnP.PowerShell.Commands.Site
{
    [Cmdlet(VerbsCommon.Add, "PnPRoleDefinition")]
    [CmdletHelp("Adds a Role Defintion (Permission Level) to the site collection in the current context",
        DetailedDescription = "This command allows adding a custom Role Defintion (Permission Level) to the site collection in the current context. It does not replace or remove existing Role Definitions.",
        Category = CmdletHelpCategory.Sites)]
    [CmdletExample(
        Code = @"PS:> Add-PnPRoleDefinition -RoleName ""CustomPerm""",
        Remarks = @"Creates additional permission level with no permission flags enabled.", SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Add-PnPRoleDefinition -RoleName ""NoDelete"" -Clone ""Contribute"" -Exclude DeleteListItems",
        Remarks = @"Creates additional permission level by cloning ""Contribute"" and removes flags DeleteListItems", SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Add-PnPRoleDefinition -RoleName ""AddOnly"" -Clone ""Contribute"" -Exclude DeleteListItems, EditListItems",
        Remarks = @"Creates additional permission level by cloning ""Contribute"" and removes flags DeleteListItems and EditListItems", SortOrder = 3)]
    [CmdletExample(
        Code = @"PS:> $roleDefinition = Get-PnPRoleDefinition -Identity ""Contribute""
PS:> Add-PnPRoleDefinition -RoleName ""AddOnly"" -Clone $roleDefinition -Exclude DeleteListItems, EditListItems",
        Remarks = @"Creates additional permission level by cloning ""Contribute"" and removes flags DeleteListItems and EditListItems", SortOrder = 4)]

    public class AddRoleDefinition : PnPSharePointCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Name of new permission level.")]
        public string RoleName;

        [Parameter(Mandatory = false, HelpMessage = "An existing permission level or the name of an permission level to clone as base template.")]
        public RoleDefinitionPipeBind Clone;

        [Parameter(Mandatory = false, HelpMessage = "Specifies permission flags(s) to enable. Please visit https://docs.microsoft.com/previous-versions/office/sharepoint-csom/ee536458(v%3Doffice.15) for the PermissionKind enum")]
        public PermissionKind[] Include;

        [Parameter(Mandatory = false, HelpMessage = "Specifies permission flags(s) to disable. Please visit https://docs.microsoft.com/previous-versions/office/sharepoint-csom/ee536458(v%3Doffice.15) for the PermissionKind enum")]
        public PermissionKind[] Exclude;

        [Parameter(Mandatory = false, HelpMessage = "Optional description for the new permission level.")]
        public string Description;

        protected override void ExecuteCmdlet()
        {
            // Validate user inputs
            RoleDefinition roleDefinition = null;
            try
            {
                roleDefinition = ClientContext.Site.RootWeb.RoleDefinitions.GetByName(RoleName);
                ClientContext.Load(roleDefinition);
                ClientContext.ExecuteQuery();
            }
            catch { }
            if (roleDefinition.ServerObjectIsNull == null)
            {
                var spRoleDef = new RoleDefinitionCreationInformation();
                var spBasePerm = new BasePermissions();

                if (ParameterSpecified(nameof(Clone)))
                {
                    var clonePerm = Clone.GetRoleDefinition(ClientContext.Site);
                    spBasePerm = clonePerm.BasePermissions;
                }

                // Include and Exclude Flags
                if (ParameterSpecified(nameof(Include)))
                {
                    foreach (var flag in Include)
                    {
                        spBasePerm.Set(flag);
                    }
                }
                if (ParameterSpecified(nameof(Exclude)))
                {
                    foreach (var flag in Exclude)
                    {
                        spBasePerm.Clear(flag);
                    }
                }

                // Create Role Definition
                spRoleDef.Name = RoleName;
                spRoleDef.Description = Description;
                spRoleDef.BasePermissions = spBasePerm;
                roleDefinition = ClientContext.Site.RootWeb.RoleDefinitions.Add(spRoleDef);
                ClientContext.Load(roleDefinition);
                ClientContext.ExecuteQueryRetry();
                WriteObject(roleDefinition);
            }
            else
            {
                WriteWarning($"Unable to add Role Definition as there is an existing role definition with the same name. Will be skipped.");
            }
        }
    }
}
