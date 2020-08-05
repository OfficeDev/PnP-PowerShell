using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;

namespace PnP.PowerShell.Commands.InformationManagement
{

    [Cmdlet(VerbsCommon.Get, "PnPSiteClosure")]
    [CmdletHelp("Get the site closure status of the site which has a site policy applied", Category = CmdletHelpCategory.InformationManagement)]
    [CmdletExample(
      Code = @"PS:> Get-PnPSiteClosure",
      Remarks = @"Get the site closure status of the site.", SortOrder = 1)]
    public class GetSiteClosure : PnPWebCmdlet
    {
        protected override void ExecuteCmdlet()
        {
            var isClosed = SelectedWeb.IsClosedBySitePolicy();

            WriteObject(isClosed ? ClosureState.Closed : ClosureState.Open, false);
        }
    }
}
