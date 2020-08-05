#if !ONPREMISES
using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Enums;
using Resources = PnP.PowerShell.Commands.Properties.Resources;

namespace PnP.PowerShell.Commands.Search
{
    [Cmdlet(VerbsCommon.Set, "PnPSearchSettings")]
    [CmdletHelp("Sets search settings for a site",
        Category = CmdletHelpCategory.Search, SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = @"PS:> Set-PnPSearchSettings -SearchBoxInNavBar Hidden -Scope Site",
        Remarks = "Hide the suite bar search box on all pages and sites in the site collection",
        SortOrder = 1)]

    [CmdletExample(
        Code = @"PS:> Set-PnPSearchSettings -SearchBoxInNavBar Hidden -Scope Web",
        Remarks = "Hide the suite bar search box on all pages in the current site",
        SortOrder = 2)]

    [CmdletExample(
        Code = @"PS:> Set-PnPSearchSettings -SearchPageUrl ""https://contoso.sharepoint.com/sites/mysearch/SitePages/search.aspx""",
        Remarks = "Redirect the suite bar search box in the site to a custom URL",
        SortOrder = 3)]

    [CmdletExample(
        Code = @"PS:> Set-PnPSearchSettings -SearchPageUrl """"",
        Remarks = "Clear the suite bar search box URL and revert to the default behavior",
        SortOrder = 4)]

    [CmdletExample(
        Code = @"PS:> Set-PnPSearchSettings -SearchPageUrl ""https://contoso.sharepoint.com/sites/mysearch/SitePages/search.aspx"" -Scope Site",
        Remarks = "Redirect classic search to a custom URL",
        SortOrder = 5)]

    [CmdletExample(
        Code = @"PS:> Set-PnPSearchSettings -SearchBoxPlaceholderText ""Search for contracts""",
        Remarks = "Set a custom placeholder text in the search box for the site",
        SortOrder = 6)]

    [CmdletExample(
        Code = @"PS:> Set-PnPSearchSettings -SearchBoxPlaceholderText """"",
        Remarks = "Clear the custom placeholder text in the search box and revert to the default text",
        SortOrder = 7)]

    [CmdletExample(
        Code = @"PS:> Set-PnPSearchSettings -SearchBoxPlaceholderText ""Search for contracts"" -Scope Site",
        Remarks = "Set a custom placeholder text in the search box for the site collection",
        SortOrder = 8)]

    [CmdletExample(
        Code = @"PS:> Set-PnPSearchSettings -SearchScope Tenant",
        Remarks = "Set default behavior of the suite bar search box to show tenant wide results instead of site or hub scoped results",
        SortOrder = 9)]

    [CmdletExample(
        Code = @"PS:> Set-PnPSearchSettings -SearchScope Hub",
        Remarks = "Set default behavior of the suite bar search box to show hub results instead of site results on an associated hub site",
        SortOrder = 10)]

    public class SetSearchSettings : PnPWebCmdlet
    {
        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets,
            HelpMessage = "Set the scope of which the suite bar search box shows. Possible values: Inherit, AllPages, ModernOnly, Hidden")]
        public SearchBoxInNavBarType? SearchBoxInNavBar;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets,
            HelpMessage = "Set the URL where the search box should redirect to.")]
        public string SearchPageUrl;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets,
            HelpMessage = "Set the text to show in the search box.")]
        public string SearchBoxPlaceholderText;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets,
            HelpMessage = "Set the search scope of the suite bar search box. Possible values: DefaultScope, Tenant, Hub, Site")]
        public SearchScopeType? SearchScope;

        [Parameter(Mandatory = false, HelpMessage = @"Scope to apply the setting to. Possible values: Web (default), Site\r\n\r\nFor a root site, the scope does not matter.")]
        public SearchSettingsScope Scope = SearchSettingsScope.Web;

        [Parameter(Mandatory = false, HelpMessage = "Do not ask for confirmation.")]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            bool hasSearchPageUrl = ParameterSpecified(nameof(SearchPageUrl));
            if (hasSearchPageUrl && SearchPageUrl == null)
            {
                SearchPageUrl = string.Empty;
            }

            bool hasSearchPlaceholderText = ParameterSpecified(nameof(SearchBoxPlaceholderText));
            if (hasSearchPlaceholderText && SearchBoxPlaceholderText == null)
            {
                SearchBoxPlaceholderText = string.Empty;
            }

            if (!Force && SearchBoxInNavBar.HasValue && SearchBoxInNavBar.Value == SearchBoxInNavBarType.Hidden)
            {
                var url = ClientContext.Url;
                var uri = new Uri(url);
                var uriParts = uri.Host.Split('.');

                var rootUrl = $"https://{string.Join(".", uriParts)}";

                bool shouldContinue;
                if (ClientContext.Url.TrimEnd('/').Equals(rootUrl.TrimEnd('/'), System.StringComparison.InvariantCultureIgnoreCase))
                {
                    shouldContinue = ShouldContinue($"If you hide the suite bar search box in the root site collection, take notice that this will also affect the SharePoint Home Site, as well as the current site ({url}), it's lists and it's libraries.\r\n\r\nOnly continue if you are aware of the implications.", Resources.Confirm);
                }
                else
                {
                    shouldContinue = ShouldContinue($"Hiding the search box will hide the search box from the current site ({url}), as well as it's lists and libraries.\r\n\r\nOnly continue if you are aware of the implications.", Resources.Confirm);
                }

                if (!shouldContinue)
                {
                    return;
                }
            }

            ClientContext.Web.EnsureProperty(w => w.SearchScope);
            if (this.Scope == SearchSettingsScope.Site)
            {
                if (this.SearchBoxInNavBar.HasValue)
                {
                    ClientContext.Site.SearchBoxInNavBar = this.SearchBoxInNavBar.Value;
                }
                if (hasSearchPageUrl)
                {
                    ClientContext.Web.SetSiteCollectionSearchCenterUrl(SearchPageUrl);
                }
                if (hasSearchPlaceholderText)
                {
                    ClientContext.Site.SetSearchBoxPlaceholderText(SearchBoxPlaceholderText);
                }
                if (SearchScope.HasValue && ClientContext.Site.RootWeb.SearchScope != SearchScope.Value)
                {
                    ClientContext.Site.RootWeb.SearchScope = SearchScope.Value;
                    ClientContext.Site.RootWeb.Update();
                }
            }
            else
            {
                if (this.SearchBoxInNavBar.HasValue)
                {
                    ClientContext.Web.SearchBoxInNavBar = this.SearchBoxInNavBar.Value;
                }
                if (hasSearchPageUrl)
                {
                    ClientContext.Web.SetWebSearchCenterUrl(SearchPageUrl);
                }
                if (hasSearchPlaceholderText)
                {
                    ClientContext.Web.SetSearchBoxPlaceholderText(SearchBoxPlaceholderText);
                }
                if (SearchScope.HasValue && ClientContext.Web.SearchScope != SearchScope.Value)
                {
                    ClientContext.Web.SearchScope = SearchScope.Value;
                }
                ClientContext.Web.Update();
            }

            ClientContext.ExecuteQueryRetry();
        }
    }
}
#endif