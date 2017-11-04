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
    [Cmdlet(VerbsCommon.Remove, "PnPSiteClassification")]
    [CmdletHelp("Removes one or more existing site classification values from the list of available values. Requires a connection to the Microsoft Graph",
        Category = CmdletHelpCategory.Graph,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = @"PS:> Connect-PnPOnline -Scopes ""Directory.ReadWrite.All""
PS:> Remove-PnPSiteClassification -Classifications ""HBI""",
        Remarks = @"Removes the ""HBI"" site classification from the list of available values.",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Connect-PnPOnline -Scopes ""Directory.ReadWrite.All""
PS:> Enable-PnPSiteClassification -Classifications ""HBI"",""LBI"",""Top Secret"" -DefaultClassification ""LBI""",
        Remarks = @"Enables Site Classifications for your tenant and provides three classification values. The default value will be set to ""LBI""",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Connect-PnPOnline -Scopes ""Directory.ReadWrite.All""
PS:> Enable-PnPSiteClassification -Classifications ""HBI"",""LBI"",""Top Secret"" -UsageGuidelinesUrl http://aka.ms/sppnp",
        Remarks = @"Enables Site Classifications for your tenant and provides three classification values. The first value will be set as the default and the usage guideliness will be set to the specified URL.",
        SortOrder = 3)]
    public class RemoveSiteClassification : PnPGraphCmdlet
    {

        [Parameter(Mandatory = true)]
        public List<string> Classifications;

        [Parameter(Mandatory = false, HelpMessage = "Specifying the Confirm parameter will allow the confirmation question to be skipped")]
        public SwitchParameter Confirm;

        protected override void ExecuteCmdlet()
        {
            try
            {
                var existingSettings = OfficeDevPnP.Core.Framework.Graph.SiteClassificationUtility.GetSiteClassificationSettings(AccessToken);
                foreach (var classification in Classifications)
                {
                    if (existingSettings.Classifications.Contains(classification))
                    {

                        if (existingSettings.DefaultClassification == classification)
                        {
                            if ((MyInvocation.BoundParameters.ContainsKey("Confirm") && !bool.Parse(MyInvocation.BoundParameters["Confirm"].ToString())) || ShouldContinue(string.Format(Properties.Resources.RemoveDefaultClassification0, classification), Properties.Resources.Confirm))
                            {
                                existingSettings.DefaultClassification = "";
                                existingSettings.Classifications.Remove(classification);
                            }
                        }
                        else
                        {
                            existingSettings.Classifications.Remove(classification);
                        }
                    }
                }
                if (existingSettings.Classifications.Any())
                {
                    OfficeDevPnP.Core.Framework.Graph.SiteClassificationUtility.UpdateSiteClassificationSettings(AccessToken, existingSettings);
                }
                else
                {
                    WriteError(new ErrorRecord(new InvalidOperationException("At least one classification is required. If you want to disable classifications, use Disable-PnPSiteClassification."), "SITECLASSIFICATIONS_ARE_REQUIRED", ErrorCategory.InvalidOperation, null));
                }
            }
            catch (ApplicationException ex)
            {
                if (ex.Message == @"Missing DirectorySettingTemplate for ""Group.Unified""")
                {
                    WriteError(new ErrorRecord(new InvalidOperationException("Site Classification is not enabled for this tenant"), "SITECLASSIFICATION_NOT_ENABLED", ErrorCategory.ResourceUnavailable, null));
                }
            }
        }
    }
}
