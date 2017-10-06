using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.Principals
{
    [Cmdlet(VerbsCommon.Get, "PnPUsersInGroup")]
    [CmdletHelp("Retrieves all users in a group",
        Category = CmdletHelpCategory.Principals,
        DetailedDescription = "This command will return all the users that are a member of the provided SharePoint Group",
        OutputType = typeof(User),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.user.aspx")]
    [CmdletExample(
        Code = @"PS:> Get-PnPUserInGroup -Identity 'Marketing Site Members'",
        SortOrder = 1,
        Remarks = @"Returns all the users that are a member of the group 'Marketing Site Members' in the current sitecollection")]
    [CmdletExample(
        Code = @"PS:> Get-PnPGroup | Get-PnPUsersInGroup",
        SortOrder = 2,
        Remarks = @"Returns all the users that are a member of any of the groups in the current sitecollection")]
    [CmdletExample(
        Code = @"PS:> Get-PnPGroup | ? Title -Like 'Marketing*' | Get-PnPUsersInGroup",
        SortOrder = 3,
        Remarks = @"Returns all the users that are a member of any of the groups of which their name starts with the word 'Marketing' in the current sitecollection")]
    public class GetUsersInGroup : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "A group object, an ID or a name of a group")]
        [Alias("GroupName")]
        public GroupPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            var group = Identity.GetGroup(SelectedWeb);
            WriteObject(group.Users, true);
        }
    }
}