using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands.InformationManagement
{
    [Cmdlet(VerbsCommon.Get, "PnPSitePolicy")]
    [CmdletAlias("Get-SPOSitePolicy")]
    [CmdletHelp("Retrieves all or a specific site policy",
        Category = CmdletHelpCategory.InformationManagement,
        OutputType=typeof(OfficeDevPnP.Core.Entities.SitePolicyEntity))]
    [CmdletExample(
     Code = @"PS:> Get-PnPSitePolicy",
     Remarks = @"Retrieves the current applied site policy.", SortOrder = 1)]
    [CmdletExample(
     Code = @"PS:> Get-PnPSitePolicy -AllAvailable",
     Remarks = @"Retrieves all available site policies.", SortOrder = 2)]
    [CmdletExample(
      Code = @"PS:> Get-PnPSitePolicy -Name ""Contoso HBI""",
      Remarks = @"Retrieves an available site policy with the name ""Contoso HBI"".", SortOrder = 3)]

    public class GetSitePolicy : SPOWebCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "Retrieve all available site policies")]
        public SwitchParameter AllAvailable;

        [Parameter(Mandatory = false, HelpMessage = "Retrieves a site policy with a specific name")]
        public string Name;

        protected override void ExecuteCmdlet()
        {

            if (!MyInvocation.BoundParameters.ContainsKey("AllAvailable") && !MyInvocation.BoundParameters.ContainsKey("Name"))
            {
                // Return the current applied site policy
                WriteObject(this.SelectedWeb.GetAppliedSitePolicy());
            }
            else
            {
                if (MyInvocation.BoundParameters.ContainsKey("AllAvailable"))
                {
                    WriteObject(SelectedWeb.GetSitePolicies(),true);
                    return;
                }
                if (MyInvocation.BoundParameters.ContainsKey("Name"))
                {
                    WriteObject(SelectedWeb.GetSitePolicyByName(Name));
                }
                
            }
        }

    }

}


