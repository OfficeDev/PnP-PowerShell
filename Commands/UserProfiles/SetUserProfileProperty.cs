#if !ONPREMISES
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.UserProfiles;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using System.Linq;

namespace SharePointPnP.PowerShell.Commands.UserProfiles
{
    [Cmdlet(VerbsCommon.Set, "PnPUserProfileProperty")]
    [CmdletAlias("Set-SPOUserProfileProperty")]
    [CmdletHelp(@"Office365 only: Uses the tenant API to retrieve site information.

You must connect to the tenant admin website (https://:<tenant>-admin.sharepoint.com) with Connect-PnPOnline in order to use this command. 
", DetailedDescription = "Requires a connection to a SharePoint Tenant Admin site.",
        Category = CmdletHelpCategory.UserProfiles)]
    [CmdletExample(
        Code = @"PS:> Set-PnPUserProfileProperty -Account 'user@domain.com' -Property 'SPS-Location' -Value 'Stockholm'",
        Remarks = "Sets the SPS-Location property for the user as specified by the Account parameter",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Set-PnPUserProfileProperty -Account 'user@domain.com' -Property 'MyProperty' -Values 'Value 1','Value 2'",
        Remarks = "Sets the MyProperty multi value property for the user as specified by the Account parameter",
        SortOrder = 2)]
    public class SetUserProfileProperty : SPOAdminCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The account of the user, formatted either as a login name, or as a claims identity, e.g. i:0#.f|membership|user@domain.com", ParameterSetName = ParameterAttribute.AllParameterSets)]
        public string Account;

        [Parameter(Mandatory = true, HelpMessage = "The property to set, for instance SPS-Skills or SPS-Location", ParameterSetName = ParameterAttribute.AllParameterSets)]
        public string PropertyName;

        [Parameter(Mandatory = true, HelpMessage = "The value to set in the case of a single value property", ParameterSetName = "Single")]
        public string Value;

        [Parameter(Mandatory = true, HelpMessage = "The values set in the case of a multi value property, e.g. \"Value 1\",\"Value 2\"",ParameterSetName = "Multi")]
        [AllowEmptyString]
        public string[] Values;

        protected override void ExecuteCmdlet()
        {
            var peopleManager = new PeopleManager(ClientContext);

            var result = Tenant.EncodeClaim(Account);
            ClientContext.ExecuteQueryRetry();

            if (ParameterSetName == "Single")
            {
                peopleManager.SetSingleValueProfileProperty(result.Value, PropertyName, Value);
            }
            else
            {
                peopleManager.SetMultiValuedProfileProperty(result.Value, PropertyName, Values.ToList());
            }

            ClientContext.ExecuteQueryRetry();

        }
    }
}
#endif