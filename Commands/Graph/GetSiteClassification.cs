using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SharePointPnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsCommon.Get, "PnPSiteClassification")]
    [CmdletHelp("Returns the defined Site Classifications for the tenant. Requires a connection to the Microsoft Graph.",
        Category = CmdletHelpCategory.Graph,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = @"PS:> Connect-PnPOnline -Scopes ""Directory.ReadWrite.All""
PS:> Get-PnPSiteClassification",
       Remarks = @"Returns the currently set site classifications for the tenant.",
       SortOrder = 1)]
    public class GetSiteClassification : PnPGraphCmdlet
    {
        protected override void ExecuteCmdlet()
        {
            try
            {
                WriteObject(OfficeDevPnP.Core.Framework.Graph.SiteClassificationUtility.GetSiteClassificationSettings(AccessToken), true);
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
