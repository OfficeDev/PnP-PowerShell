using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using System.Management.Automation;

namespace PnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Set, "PnPWeb")]
    [CmdletHelp("Sets properties on a web", 
        DetailedDescription = "Allows setting various properties on a web",
        Category = CmdletHelpCategory.Webs,
        SupportedPlatform = CmdletSupportedPlatform.All)]
    [CmdletExample(
        Code = @"PS:> Set-PnPWeb -CommentsOnSitePagesDisabled:$true",
        Remarks = "Disables the page comments to be shown below each page in the current web by default",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Set-PnPWeb -QuickLaunchEnabled:$false",
        Remarks = "Hides the quick launch from being shown in the current web",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Set-PnPWeb -NoCrawl:$true",
        Remarks = "Prevents the current web from being returned in search results",
        SortOrder = 3)]
    public class SetWeb : PnPWebCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "Sets the logo of the web to the current url. If you want to set the logo to a modern team site, use Set-PnPSite -LogoFilePath.")]
        public string SiteLogoUrl;

        [Parameter(Mandatory = false, HelpMessage = "Sets the AlternateCssUrl of the web. Only works for classic pages.")]
        public string AlternateCssUrl;

        [Parameter(Mandatory = false, HelpMessage = "Sets the title of the web")]
        public string Title;

        [Parameter(Mandatory = false, HelpMessage = "Sets the description of the web")]
        public string Description;

        [Parameter(Mandatory = false, HelpMessage = "Sets the MasterUrl of the web. Only works for classic pages.")]
        public string MasterUrl;

        [Parameter(Mandatory = false, HelpMessage = "Sets the CustomMasterUrl of the web. Only works for classic pages.")]
        public string CustomMasterUrl;

        [Parameter(Mandatory = false, HelpMessage = "Defines if the quick launch menu on the left side of modern Team Sites should be shown ($true) or hidden ($false)")]
        public SwitchParameter QuickLaunchEnabled;

        [Parameter(Mandatory = false, HelpMessage = "Indicates if members of this site can share the site and individual sites with others ($true) or only owners can do this ($false)")]
        public SwitchParameter MembersCanShare;

        [Parameter(Mandatory = false, HelpMessage = "Indicates if this site should not be returned in search results ($true) or if it should be ($false)")]
        public SwitchParameter NoCrawl;

#if !ONPREMISES
        [Parameter(Mandatory = false)]
        public HeaderLayoutType HeaderLayout = HeaderLayoutType.Standard;

        [Parameter(Mandatory = false)]
        public SPVariantThemeType HeaderEmphasis = SPVariantThemeType.None;

        [Parameter(Mandatory = false, HelpMessage = "Defines if the navigation menu on a modern site should be enabled for modern audience targeting ($true) or not ($false)")]
        public SwitchParameter NavAudienceTargetingEnabled;

        [Parameter(Mandatory = false, HelpMessage = "Defines if the navigation menu should be shown as the mega menu ($true) or the smaller sized menu ($false)")]
        public SwitchParameter MegaMenuEnabled;

        [Parameter(Mandatory = false, HelpMessage = "Defines if Power Automate should be available on lists and document libraries ($false) or if the option should be hidden ($true)")]
        public SwitchParameter DisablePowerAutomate;

        [Parameter(Mandatory = false, HelpMessage = "Defines if comments on modern site pages should be enabled by default ($false) or they should be hidden ($true)")]
        public SwitchParameter CommentsOnSitePagesDisabled;
#endif

        protected override void ExecuteCmdlet()
        {
            var dirty = false;

            foreach (var key in MyInvocation.BoundParameters.Keys)
            {
                switch (key)
                {
                    case nameof(SiteLogoUrl):
                        SelectedWeb.SiteLogoUrl = SiteLogoUrl;
                        dirty = true;
                        break;

                    case nameof(AlternateCssUrl):
                        SelectedWeb.AlternateCssUrl = AlternateCssUrl;
                        dirty = true;
                        break;

                    case nameof(Title):
                        SelectedWeb.Title = Title;
                        dirty = true;
                        break;

                    case nameof(Description):
                        SelectedWeb.Description = Description;
                        dirty = true;
                        break;

                    case nameof(MasterUrl):
                        SelectedWeb.MasterUrl = MasterUrl;
                        dirty = true;
                        break;

                    case nameof(CustomMasterUrl):
                        SelectedWeb.CustomMasterUrl = CustomMasterUrl;
                        dirty = true;
                        break;

                    case nameof(QuickLaunchEnabled):
                        SelectedWeb.QuickLaunchEnabled = QuickLaunchEnabled.ToBool();
                        dirty = true;
                        break;
#if !ONPREMISES
                    case nameof(MembersCanShare):
                        SelectedWeb.MembersCanShare = MembersCanShare.ToBool();
                        dirty = true;
                        break;

                    case nameof(NoCrawl):
                        SelectedWeb.NoCrawl = NoCrawl.ToBool();
                        dirty = true;
                        break;

                    case nameof(HeaderLayout):
                        SelectedWeb.HeaderLayout = HeaderLayout;
                        dirty = true;
                        break;

                    case nameof(HeaderEmphasis):
                        SelectedWeb.HeaderEmphasis = HeaderEmphasis;
                        dirty = true;
                        break;

                    case nameof(NavAudienceTargetingEnabled):
                        SelectedWeb.NavAudienceTargetingEnabled = NavAudienceTargetingEnabled.ToBool();
                        dirty = true;
                        break;

                    case nameof(MegaMenuEnabled):
                        SelectedWeb.MegaMenuEnabled = MegaMenuEnabled.ToBool();
                        dirty = true;
                        break;

                    case nameof(DisablePowerAutomate):
                        SelectedWeb.DisableFlows = DisablePowerAutomate.ToBool();
                        dirty = true;
                        break;

                    case nameof(CommentsOnSitePagesDisabled):
                        SelectedWeb.CommentsOnSitePagesDisabled = CommentsOnSitePagesDisabled.ToBool();
                        dirty = true;
                        break;
#endif
                }
            }

            if (dirty)
            {
                SelectedWeb.Update();
                ClientContext.ExecuteQueryRetry();
            }
        }
    }
}
