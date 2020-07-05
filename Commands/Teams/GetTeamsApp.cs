#if !ONPREMISES
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using SharePointPnP.PowerShell.Commands.Utilities;
using SharePointPnP.PowerShell.Core.Attributes;
using System;
using System.Linq;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsCommon.Get, "PnPTeamsApp")]
    [CmdletHelp("Gets one Microsoft Teams App or a list of all apps.",
        Category = CmdletHelpCategory.Teams,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = "PS:> Get-PnPTeamsApp",
       Remarks = "Retrieves all the Microsoft Teams Apps",
       SortOrder = 1)]
    [CmdletExample(
       Code = "PS:> Get-PnPTeamsApp -Identity a54224d7-608b-4839-bf74-1b68148e65d4",
       Remarks = "Retrieves a specific Microsoft Teams App",
       SortOrder = 2)]
    [CmdletExample(
       Code = "PS:> Get-PnPTeamsApp -Identity \"MyTeamsApp\"",
       Remarks = "Retrieves a specific Microsoft Teams App",
       SortOrder = 3)]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.Directory_ReadWrite_All)]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.AppCatalog_Read_All)]
    [CmdletTokenType(TokenType = TokenType.Delegate)]
    public class GetTeamsApp : PnPGraphCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "Specify the name, id or external id of the app.")]
        public TeamsAppPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            if (ParameterSpecified(nameof(Identity)))
            {
                var apps = TeamsUtility.GetApps(AccessToken, HttpClient);
                if (Identity.Id != Guid.Empty)
                {
                    WriteObject(apps.FirstOrDefault(a => a.Id == Identity.Id.ToString() || a.ExternalId == a.Id.ToString()));
                }
                else
                {
                    WriteObject(apps.FirstOrDefault(a => a.DisplayName.Equals(Identity.StringValue, StringComparison.OrdinalIgnoreCase)));
                }
            }
            else
            {
                WriteObject(TeamsUtility.GetApps(AccessToken, HttpClient), true);
            }
        }
    }
}
#endif