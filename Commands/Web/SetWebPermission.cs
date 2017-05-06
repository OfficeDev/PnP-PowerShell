using System;
using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using SharePointPnP.PowerShell.Commands.Extensions;

namespace SharePointPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Set, "PnPWebPermission", DefaultParameterSetName = "User")]
    [CmdletAlias("Set-SPOWebPermission")]
    [CmdletHelp("Sets web permissions",
        Category = CmdletHelpCategory.Webs)]
    [CmdletExample(
        Code = "PS:> Set-PnPWebPermission -Url projectA -User 'user@contoso.com' -AddRole 'Contribute'",
        Remarks = "Adds the 'Contribute' permission to the user 'user@contoso.com' for a web, specified by its site relative url",
        SortOrder = 1)]        
    [CmdletExample(
        Code = "PS:> Set-PnPWebPermission -Identity 5fecaf67-6b9e-4691-a0ff-518fc9839aa0 -User 'user@contoso.com' -RemoveRole 'Contribute'",
        Remarks = "Removes the 'Contribute' permission to the user 'user@contoso.com' for a web, specified by its ID",
        SortOrder = 2)]        
    public class SetWebPermission : PnPWebCmdlet
    {
		[Parameter(Mandatory = true, HelpMessage = "Identity/Id/Web object", ParameterSetName = "GroupByWebIdentity", ValueFromPipeline = true)]
		[Parameter(Mandatory = true, HelpMessage = "Identity/Id/Web object", ParameterSetName = "UserByWebIdentity", ValueFromPipeline = true)]
		public WebPipeBind Identity;

		[Parameter(Mandatory = true, HelpMessage = "The site relative url of the web, e.g. 'Subweb1'", ParameterSetName = "GroupByWebUrl")]
		[Parameter(Mandatory = true, HelpMessage = "The site relative url of the web, e.g. 'Subweb1'", ParameterSetName = "UserByWebUrl")]
		public string Url;

		[Parameter(Mandatory = true, ParameterSetName = "Group")]
		[Parameter(Mandatory = true, ParameterSetName = "GroupByWebIdentity")]
		[Parameter(Mandatory = true, ParameterSetName = "GroupByWebUrl")]
		public GroupPipeBind Group;

        [Parameter(Mandatory = true, ParameterSetName = "User")]
        [Parameter(Mandatory = true, ParameterSetName = "UserByWebIdentity")]
        [Parameter(Mandatory = true, ParameterSetName = "UserByWebUrl")]
        public string User;

        [Parameter(Mandatory = false, HelpMessage = "The role that must be assigned to the group or user", ParameterSetName = ParameterAttribute.AllParameterSets)]
		public string[] AddRole = null;

        [Parameter(Mandatory = false, HelpMessage = "The role that must be removed from the group or user", ParameterSetName = ParameterAttribute.AllParameterSets)]
        public string[] RemoveRole = null;

        protected override void ExecuteCmdlet()
		{
			// Get Web
			Web web = SelectedWeb;
			if (ParameterSetName == "GroupByWebIdentity" || ParameterSetName == "UserByWebIdentity")
			{
				if (Identity.Id != Guid.Empty)
				{
					web = ClientContext.Web.GetWebById(Identity.Id);
				}
				else if (Identity.Web != null)
				{
					web = Identity.Web;
				}
				else if (Identity.Url != null)
				{
					web = ClientContext.Web.GetWebByUrl(Identity.Url);
				}
			}
			else if (ParameterSetName == "GroupByWebUrl" || ParameterSetName == "UserByWebUrl")
			{
				web = SelectedWeb.GetWeb(Url);
			}

			// Set permissions
			Principal principal = null;
			if (ParameterSetName == "Group" || ParameterSetName == "GroupByWebUrl" || ParameterSetName == "GroupByWebIdentity")
			{
				if (Group.Id != -1)
				{
					principal = web.SiteGroups.GetById(Group.Id);
				}
				else if (!string.IsNullOrEmpty(Group.Name))
				{
					principal = web.SiteGroups.GetByName(Group.Name);
				}
				else if (Group.Group != null)
				{
					principal = Group.Group;
				}
			}
			else
			{
				principal = web.EnsureUser(User);
			}
			if (principal != null)
			{
				if (AddRole != null)
				{
					foreach (var role in AddRole)
					{
						web.AddPermissionLevelToPrincipal(principal, role);
					}
				}
				if (RemoveRole != null)
				{
					foreach (var role in RemoveRole)
					{
						web.RemovePermissionLevelFromPrincipal(principal, role);
					}
				}
			}
			else
			{
				WriteError(new ErrorRecord(new Exception("Principal not found"), "1", ErrorCategory.ObjectNotFound, null));
			}
		}
	}
}
