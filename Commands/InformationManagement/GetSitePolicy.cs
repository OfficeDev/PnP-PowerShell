using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;

namespace PnP.PowerShell.Commands.InformationManagement
{
    [Cmdlet(VerbsCommon.Get, "PnPSitePolicy")]
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

    public class GetSitePolicy : PnPWebCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "Retrieve all available site policies")]
        public SwitchParameter AllAvailable;

        [Parameter(Mandatory = false, HelpMessage = "Retrieves a site policy with a specific name")]
        public string Name;

        protected override void ExecuteCmdlet()
        {

            if (!ParameterSpecified(nameof(AllAvailable)) && !ParameterSpecified(nameof(Name)))
            {
                // Return the current applied site policy
                WriteObject(this.SelectedWeb.GetAppliedSitePolicy());
            }
            else
            {
                if (ParameterSpecified(nameof(AllAvailable)))
                {
                    WriteObject(SelectedWeb.GetSitePolicies(),true);
                    return;
                }
                if (ParameterSpecified(nameof(Name)))
                {
                    WriteObject(SelectedWeb.GetSitePolicyByName(Name));
                }
                
            }
        }

    }

}


