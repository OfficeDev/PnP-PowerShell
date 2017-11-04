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
    [Cmdlet(VerbsLifecycle.Disable, "PnPSiteClassification")]
    [CmdletHelp("Disables Site Classifications for the tenant. Requires a connection to the Microsoft Graph.",
        Category = CmdletHelpCategory.Graph,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = @"PS:> Connect-PnPOnline -Scopes ""Directory.ReadWrite.All""
PS:> Disable-PnPSiteClassification",
       Remarks = @"Disables Site Classifications for your tenant.",
       SortOrder = 1)]
    public class DisableSiteClassification : PnPGraphCmdlet
    {

        protected override void ExecuteCmdlet()
        {
            try
            {
                OfficeDevPnP.Core.Framework.Graph.SiteClassificationUtility.DisableSiteClassifications(AccessToken);
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
