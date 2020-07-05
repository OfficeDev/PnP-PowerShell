#if !ONPREMISES
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsCommon.Get, "PnPAADUser", DefaultParameterSetName = ParameterSet_LIST)]
    [CmdletHelp("Retrieves users from Azure Active Directory",
        Category = CmdletHelpCategory.Graph,
        OutputTypeLink = "https://docs.microsoft.com/graph/api/user-get",
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = "PS:> Get-PnPAADUser",
       Remarks = "Retrieves all users from Azure Active Directory",
       SortOrder = 1)]
    [CmdletExample(
       Code = "PS:> Get-PnPAADUser -Identity 328c7693-5524-44ac-a946-73e02d6b0f98",
       Remarks = "Retrieves the user from Azure Active Directory with the id 328c7693-5524-44ac-a946-73e02d6b0f98",
       SortOrder = 2)]
    [CmdletExample(
       Code = "PS:> Get-PnPAADUser -Identity john@contoso.com",
       Remarks = "Retrieves the user from Azure Active Directory with the user principal name john@contoso.com",
       SortOrder = 3)]
    [CmdletExample(
       Code = "PS:> Get-PnPAADUser -Identity john@contoso.com -Select \"DisplayName\",\"extension_3721d05137db455ad81aa442e3c2d4f9_extensionAttribute1\"",
       Remarks = "Retrieves only the DisplayName and extensionAttribute1 properties of the user from Azure Active Directory which has the user principal name john@contoso.com",
       SortOrder = 4)]
    [CmdletExample(
       Code = "PS:> Get-PnPAADUser -Filter \"accountEnabled eq false\"",
       Remarks = "Retrieves all the disabled users from Azure Active Directory",
       SortOrder = 5)]
    [CmdletExample(
       Code = "PS:> Get-PnPAADUser -Filter \"startswith(DisplayName, 'John')\" -OrderBy \"DisplayName\"",
       Remarks = "Retrieves all the users from Azure Active Directory of which their DisplayName starts with 'John' and sort the results by the DisplayName",
       SortOrder = 6)]
    [CmdletExample(
       Code = "PS:> Get-PnPAADUser -Delta",
       Remarks = "Retrieves all the users from Azure Active Directory and include a delta DeltaToken which can be used by providing -DeltaToken <token> to query for changes to users in Active Directory since this run",
       SortOrder = 7)]
    [CmdletExample(
       Code = "PS:> Get-PnPAADUser -Delta -DeltaToken abcdef",
       Remarks = "Retrieves all the users from Azure Active Directory which have had changes since the provided DeltaToken was given out",
       SortOrder = 8)]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.User_Read_All | MicrosoftGraphApiPermission.User_ReadWrite_All | MicrosoftGraphApiPermission.Directory_Read_All | MicrosoftGraphApiPermission.Directory_ReadWrite_All)]
    public class GetAADUser : PnPGraphCmdlet
    {
        const string ParameterSet_BYID = "Return by specific ID";
        const string ParameterSet_LIST = "Return a list";
        const string ParameterSet_DELTA = "Return the delta";

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_BYID, HelpMessage = "Returns the user with the provided user id")]
        public string Identity;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_LIST, HelpMessage = "Includes a filter to the retrieval of the users. Use OData instructions to construct the filter, i.e. \"startswith(DisplayName, 'John')\".")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_DELTA, HelpMessage = "Includes a filter to the retrieval of the users. Use OData instructions to construct the filter, i.e. \"startswith(DisplayName, 'John')\".")]
        public string Filter;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_LIST, HelpMessage = "Includes a custom sorting instruction to the retrieval of the users. Use OData syntax to construct the orderby, i.e. \"DisplayName desc\".")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_DELTA, HelpMessage = "Includes a custom sorting instruction to the retrieval of the users. Use OData syntax to construct the orderby, i.e. \"DisplayName desc\".")]
        public string OrderBy;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_BYID, HelpMessage = "Allows providing an array with the property names of specific properties to return. If not provided, the default properties will be returned.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_LIST, HelpMessage = "Allows providing an array with the property names of specific properties to return. If not provided, the default properties will be returned.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_DELTA, HelpMessage = "Allows providing an array with the property names of specific properties to return. If not provided, the default properties will be returned.")]
        public string[] Select;

        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_DELTA, HelpMessage = "Retrieves all users and provides a SkipToken delta token to allow to query for changes since this run when querying again by adding -DeltaToken to the command")]
        public SwitchParameter Delta;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_DELTA, HelpMessage = "The change token provided during the previous run with -Delta to query for the changes to user objects made in Azure Active Directory since that run")]
        public string DeltaToken;

        protected override void ExecuteCmdlet()
        {
            if (ParameterSpecified(nameof(Identity)))
            {
                OfficeDevPnP.Core.Framework.Graph.Model.User user;
                if (Guid.TryParse(Identity, out Guid identityGuid))
                {
                    user = OfficeDevPnP.Core.Framework.Graph.UsersUtility.GetUser(AccessToken, identityGuid);
                }
                else
                {
                    user = OfficeDevPnP.Core.Framework.Graph.UsersUtility.GetUser(AccessToken, Identity, Select);
                }
                WriteObject(user);
            }
            else if (ParameterSpecified(nameof(Delta)))
            {
                OfficeDevPnP.Core.Framework.Graph.Model.UserDelta userDelta = OfficeDevPnP.Core.Framework.Graph.UsersUtility.ListUserDelta(AccessToken, DeltaToken, Filter, OrderBy, Select);
                WriteObject(userDelta);
            } 
            else
            {
                List<OfficeDevPnP.Core.Framework.Graph.Model.User> users = OfficeDevPnP.Core.Framework.Graph.UsersUtility.ListUsers(AccessToken, Filter, OrderBy, Select);
                WriteObject(users, true);
            }
        }
    }
}
#endif