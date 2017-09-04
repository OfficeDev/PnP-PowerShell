#if !ONPREMISES
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System.Linq.Expressions;
using System;
using System.Linq;
using System.Collections.Generic;
using SharePointPnP.PowerShell.Commands.Base;

namespace SharePointPnP.PowerShell.Commands.RecordsManagement
{
    [Cmdlet(VerbsCommon.Set, "PnPInPlaceRecordsManagement")]
    [CmdletHelp("Activates or deactivates in place records management",
        Category = CmdletHelpCategory.RecordsManagement, SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = @"PS:> Set-PnPInPlaceRecordsManagement -On",
        Remarks = "Activates In Place Records Management",
        SortOrder = 1)]
    public class SetInPlaceRecordsManagement : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Turn records management on", ParameterSetName = "On")]
        public SwitchParameter On;

        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Turn records management off", ParameterSetName = "Off")]
        public SwitchParameter Off;

        protected override void ExecuteCmdlet()
        {
            if (MyInvocation.BoundParameters.ContainsKey("On"))
            {
                Microsoft.SharePoint.Client.RecordsManagementExtensions.ActivateInPlaceRecordsManagementFeature(ClientContext.Site);
            } else
            {
                Microsoft.SharePoint.Client.RecordsManagementExtensions.DisableInPlaceRecordsManagementFeature(ClientContext.Site);
            }
        }

    }

}
#endif