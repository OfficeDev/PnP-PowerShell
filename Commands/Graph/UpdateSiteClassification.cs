using OfficeDevPnP.Core.Framework.Graph.Model;
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
    [Cmdlet(VerbsData.Update, "PnPSiteClassification")]
    [CmdletHelp("Updates Site Classifications for the tenant. Requires a connection to the Microsoft Graph.",
        Category = CmdletHelpCategory.Graph,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = @"PS:> Connect-PnPOnline -Scopes ""Directory.ReadWrite.All""
PS:> Update-PnPSiteClassification -Classifications ""HBI"",""Top Secret""",
        Remarks = @"Replaces the existing values of the site classification settings",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Connect-PnPOnline -Scopes ""Directory.ReadWrite.All""
PS:> Update-PnPSiteClassification -DefaultClassification ""LBI""",
        Remarks = @"Sets the default classification value to ""LBI"". This value needs to be present in the list of classification values.",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Connect-PnPOnline -Scopes ""Directory.ReadWrite.All""
PS:> Update-PnPSiteClassification -UsageGuidelinesUrl http://aka.ms/sppnp",
        Remarks = @"sets the usage guideliness URL to the specified URL.",
        SortOrder = 3)]
    public class UpdateSiteClassification : PnPGraphCmdlet
    {
        const string ParameterSet_SETTINGS = "Settings";
        const string ParameterSet_SPECIFIC = "Specific";

        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_SETTINGS, HelpMessage ="A settings object retrieved by Get-PnPSiteClassification")]
        public SiteClassificationSettings Settings;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_SPECIFIC, HelpMessage = @"A list of classifications, separated by commas. E.g. ""HBI"",""LBI"",""Top Secret""")]
        public List<string> Classifications;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_SPECIFIC, HelpMessage = @"The default classification to be used. The value needs to be present in the list of possible classifications")]
        public string DefaultClassification;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_SPECIFIC, HelpMessage = @"The UsageGuidelinesUrl. Set to """" to clear.")]
        public string UsageGuidelinesUrl = "";

        protected override void ExecuteCmdlet()
        {
            try
            {
                var changed = false;
                var settings = OfficeDevPnP.Core.Framework.Graph.SiteClassificationUtility.GetSiteClassificationSettings(AccessToken);
                if (MyInvocation.BoundParameters.ContainsKey("Classifications"))
                {
                    if (settings.Classifications != Classifications)
                    {
                        settings.Classifications = Classifications;
                        changed = true;
                    }
                }
                if (MyInvocation.BoundParameters.ContainsKey("DefaultClassification"))
                {
                    if (settings.Classifications.Contains(DefaultClassification))
                    {
                        if (settings.DefaultClassification != DefaultClassification)
                        {
                            settings.DefaultClassification = DefaultClassification;
                            changed = true;
                        }
                    }
                }
                if (MyInvocation.BoundParameters.ContainsKey("UsageGuidelinesUrl"))
                {
                    if (settings.UsageGuidelinesUrl != UsageGuidelinesUrl)
                    {
                        settings.UsageGuidelinesUrl = UsageGuidelinesUrl;
                        changed = true;
                    }
                }
                if (changed)
                {
                    if (settings.Classifications.Contains(settings.DefaultClassification))
                    {
                        OfficeDevPnP.Core.Framework.Graph.SiteClassificationUtility.UpdateSiteClassificationSettings(AccessToken, settings);
                    } else
                    {
                        WriteError(new ErrorRecord(new InvalidOperationException("You are trying to set the default classification to a value that is not available in the list of possible values."), "SITECLASSIFICATION_DEFAULTVALUE_INVALID", ErrorCategory.InvalidArgument, null));
                    }
                }
            }
            catch (ApplicationException ex)
            {
                if (ex.Message == @"Missing DirectorySettingTemplate for ""Group.Unified""")
                {
                    WriteError(new ErrorRecord(new InvalidOperationException("Site Classification is not enabled for this tenant. Use Enable-PnPSiteClassification to enable classifications."), "SITECLASSIFICATION_NOT_ENABLED", ErrorCategory.ResourceUnavailable, null));
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
