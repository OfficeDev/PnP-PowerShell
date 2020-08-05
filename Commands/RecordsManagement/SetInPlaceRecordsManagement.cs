using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System.Linq.Expressions;
using System;
using System.Linq;
using System.Collections.Generic;
using PnP.PowerShell.Commands.Base;

namespace PnP.PowerShell.Commands.RecordsManagement
{
    [Cmdlet(VerbsCommon.Set, "PnPInPlaceRecordsManagement")]
    [CmdletHelp("Activates or deactivates in the place records management feature.",
        Category = CmdletHelpCategory.RecordsManagement)]
    [CmdletExample(
        Code = @"PS:> Set-PnPInPlaceRecordsManagement -Enabled $true",
        Remarks = "Activates In Place Records Management",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Set-PnPInPlaceRecordsManagement -Enabled $false",
        Remarks = "Deactivates In Place Records Management",
        SortOrder = 2)]
    public class SetInPlaceRecordsManagement : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, ParameterSetName="Enable or Disable")]
        public bool Enabled;

        [Obsolete("Use -Enabled $true")]
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Turn records management on", ParameterSetName = "On")]
        public SwitchParameter On;

        [Obsolete("Use -Enabled $false")]
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Turn records management off", ParameterSetName = "Off")]
        public SwitchParameter Off;

        protected override void ExecuteCmdlet()
        {
            if (ParameterSpecified(nameof(Enabled)))
            {
                if (Enabled)
                {
                    ClientContext.Site.ActivateInPlaceRecordsManagementFeature();
                }
                else
                {
                    ClientContext.Site.DisableInPlaceRecordsManagementFeature();
                }
            }
            else
            {
                // obsolete
#pragma warning disable CS0618 // Type or member is obsolete
                if (ParameterSpecified(nameof(On)))
#pragma warning restore CS0618 // Type or member is obsolete
                {
                    ClientContext.Site.ActivateInPlaceRecordsManagementFeature();
                }
                else
                {
                    ClientContext.Site.DisableInPlaceRecordsManagementFeature();
                }
            }
        }

    }

}
