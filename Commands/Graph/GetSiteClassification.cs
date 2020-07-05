#if !ONPREMISES
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using System;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsCommon.Get, "PnPSiteClassification")]
    [CmdletHelp("Returns the defined Site Classifications for the tenant",
        Category = CmdletHelpCategory.Graph,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = @"PS:> Get-PnPSiteClassification",
       Remarks = @"Returns the currently set site classifications for the tenant.",
       SortOrder = 1)]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.Directory_ReadWrite_All | MicrosoftGraphApiPermission.Directory_Read_All)]
    public class GetSiteClassification : PnPGraphCmdlet
    {
        protected override void ExecuteCmdlet()
        {
            try
            {
                WriteObject(OfficeDevPnP.Core.Framework.Graph.SiteClassificationsUtility.GetSiteClassificationsSettings(AccessToken), true);
            }
            catch (ApplicationException ex)
            {
                if (ex.Message == @"Missing DirectorySettingTemplate for ""Group.Unified""")
                {
                    WriteError(new ErrorRecord(new InvalidOperationException("Site Classification is not enabled for this tenant. Use Enable-PnPSiteClassification to enable classifications."), "SITECLASSIFICATION_NOT_ENABLED", ErrorCategory.ResourceUnavailable, null));
                } else
                {
                    throw;
                }
            }
        }
    }
}
#endif