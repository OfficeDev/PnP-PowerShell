using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands.InformationManagement
{

    [Cmdlet(VerbsCommon.Get, "PnPSiteClosure")]
    [CmdletAlias("Get-SPOSiteClosure")]
    [CmdletHelp("Get the site closure status of the site which has a site policy applied", Category = CmdletHelpCategory.InformationManagement)]
    [CmdletExample(
      Code = @"PS:> Get-PnPSiteClosure",
      Remarks = @"Get the site closure status of the site.", SortOrder = 1)]
    public class GetSiteClosure : SPOWebCmdlet
    {
        protected override void ExecuteCmdlet()
        {
            var isClosed = SelectedWeb.IsClosedBySitePolicy();

            WriteObject(isClosed ? ClosureState.Closed : ClosureState.Open, false);
        }
    }
}
