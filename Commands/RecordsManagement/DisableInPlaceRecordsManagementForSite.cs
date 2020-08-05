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
    [Cmdlet(VerbsLifecycle.Disable, "PnPInPlaceRecordsManagementForSite")]
    [CmdletHelp("Disables in place records management for a site.",
        Category = CmdletHelpCategory.RecordsManagement)]
    [CmdletExample(
        Code = @"PS:> Disable-PnPInPlaceRecordsManagementForSite",
        Remarks = "The in place records management feature will be disabled",
        SortOrder = 1)]

    [Obsolete("Use Set-PnPInPlaceRecordsManagement -Enabled $false instead")]
    public class DisableInPlaceRecordsManagementForSite : PnPSharePointCmdlet
    {
        protected override void ExecuteCmdlet()
        {
            this.ClientContext.Site.DisableInPlaceRecordsManagementFeature();
        }

    }

}
