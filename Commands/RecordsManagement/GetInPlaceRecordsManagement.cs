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
    [Cmdlet(VerbsCommon.Get, "PnPInPlaceRecordsManagement")]
    [CmdletHelp("Returns if the place records management feature is enabled.",
        Category = CmdletHelpCategory.RecordsManagement)]
    [CmdletExample(
        Code = @"PS:> Get-PnPInPlaceRecordsManagement",
        Remarks = "Returns if $true if in place records management is active",
        SortOrder = 1)]
    public class GetInPlaceRecordsManagement : PnPWebCmdlet
    {
        protected override void ExecuteCmdlet()
        {
            WriteObject(ClientContext.Site.IsFeatureActive(new Guid(Microsoft.SharePoint.Client.RecordsManagementExtensions.INPLACE_RECORDS_MANAGEMENT_FEATURE_ID)));
        }

    }

}
