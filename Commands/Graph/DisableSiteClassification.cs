#if !ONPREMISES
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using System;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsLifecycle.Disable, "PnPSiteClassification")]
    [CmdletHelp("Disables Site Classifications for the tenant",
        Category = CmdletHelpCategory.Graph,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = @"PS:> Disable-PnPSiteClassification",
       Remarks = @"Disables Site Classifications for your tenant.",
       SortOrder = 1)]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.Directory_ReadWrite_All)]
    public class DisableSiteClassification : PnPGraphCmdlet
    {
        protected override void ExecuteCmdlet()
        {
            try
            {
                OfficeDevPnP.Core.Framework.Graph.SiteClassificationsUtility.DisableSiteClassifications(AccessToken);
            }
            catch (ApplicationException ex)
            {
                if (ex.Message == @"Missing DirectorySettingTemplate for ""Group.Unified""")
                {
                    // swallow exception
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
#endif