# SharePointPnP.PowerShell Changelog
*Please do not commit changes to this file, it is maintained by the repo owner.*

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/).

## [3.23.2007.0] (not yet released)

### Added
- Added `-HideFromAddressLists` and `-HideFromOutlookClients` to `Set-PnPUnifiedGroup` to allow for setting the visibility of Microsoft 365 Groups [PR #2717](https://github.com/pnp/PnP-PowerShell/pull/2717)

### Changed

### Contributors
- Koen Zomers [koenzomers]

## [3.22.2006.1]

Intermediate release due to a fix in the underlying Core Library.

## [3.22.2006.0]

### Added
- Added `-ValuesOnly` option to `Get-PnPLabel` which will return more detailed information regarding the retention label set on a list or library and return the information as properties instead of written text [PR #2710](https://github.com/pnp/PnP-PowerShell/pull/2710)
- Added `-PreferredDataLocation` option to `New-PnPSite` which allows for providing a geography in which the new SharePoint sitecollection should be created. Only applicable on multi-geo enabled tenants. [PR #2708](https://github.com/pnp/PnP-PowerShell/pull/2708)
- Added `EnableAIPIntegration` option to `Set-PnPTenant` which allows enabling Azure Information Protection integration with SharePoint Online and OneDrive for Business on your tenant [PR #2703](https://github.com/pnp/PnP-PowerShell/pull/2703)
- Added `Get-PnPAADUser` cmdlet which allows retrieval of users from Azure Active Directory through the Microsoft Graph API [PR #2626](https://github.com/pnp/PnP-PowerShell/pull/2626)
- Added `Add-PnPGraphSubscription`, `Get-PnPGraphSubscription`, `Remove-PnPGraphSubscription` and `Set-PnPGraphSubscription` to work with Microsoft Graph Subscriptions [PR #2673](https://github.com/SharePoint/PnP-PowerShell/pull/2673)
- Added `Reset-PnPUnifiedGroupExpiration` which allows the expiration date of an Office 365 Group to be extended by the number of days defined in the Azure Active Directory Group Expiration policy [PR #2655](https://github.com/pnp/PnP-PowerShell/pull/2655)
- Added following arguments to `Set-PnPWeb` allowing them to be set: `CommentsOnSitePagesDisabled`, `DisablePowerAutomate`, `MegaMenuEnabled`, `MembersCanShare`, `NavAudienceTargetingEnabled`, `QuickLaunchEnabled` and `NoCrawl` [PR #2633](https://github.com/pnp/PnP-PowerShell/pull/2633)
- Added `Set-PnPUserOneDriveQuota`, `Reset-PnPUserOneDriveQuotaToDefault` and `Get-PnPUserOneDriveQuota` commands to work with quotas on OneDrive for Business sites [PR #2630](https://github.com/SharePoint/PnP-PowerShell/pull/2630)
- Added `Get-PnPTenantSyncClientRestriction` and `Set-PnPTenantSyncClientRestriction` cmdlets to allow configuring tenant wide OneDrive sync restriction settings [PR #2649](https://github.com/pnp/PnP-PowerShell/pull/2649)
- Added `Disable-PnPSharingForNonOwnersOfSite` and `Get-PnPSharingForNonOwnersOfSite` cmdlets to control disabling the ability for only owners of the site to be allowed to share the site or its files and folders with others [PR #2641](https://github.com/pnp/PnP-PowerShell/pull/2641)
- Added `Get-PnPIsSiteAliasAvailable` which allows checking if a certain alias is still available to create a new site collection with [PR #2698](https://github.com/pnp/PnP-PowerShell/pull/2698)
- Added `Get-PnPFooter` and `Set-PnPFooter` to work with the footer shown on Modern Communication pages [PR #2634](https://github.com/pnp/PnP-PowerShell/pull/2634)
- Added ability to getting and setting the title and logo shown in the footer of a Modern Communication site [PR #2715](https://github.com/pnp/PnP-PowerShell/pull/2715)
- Added `-SensitivityLabel` option to `New-PnPSite` which allows for directly assigning a sensitivity label to a SharePoint sitecollection when creating it. Requires modern sensitivity labels and E5 licenses to be enabled on the tenant. [PR #2713](https://github.com/pnp/PnP-PowerShell/pull/2713)
- Added `Get-PnPOffice365CurrentServiceStatus`, `Get-PnPOffice365HistoricalServiceStatus`, `Get-PnPOffice365ServiceMessage` and `Get-PnPOffice365Services` to retrieve information from the Office 365 Management API regarding the Office 365 services [PR #2684](https://github.com/pnp/PnP-PowerShell/pull/2684)
- Added `Get-PnPAvailableLanguage` which returns a list of all supported languages on the SharePoint web [PR #2716](https://github.com/pnp/PnP-PowerShell/pull/2716)
- Added `-HideFromAddressLists` and `-HideFromOutlookClients` to `Set-PnPUnifiedGroup` to allow for setting the visibility of Microsoft 365 Groups [PR #2717](https://github.com/pnp/PnP-PowerShell/pull/2717)

### Changed
- Fixed uploading a file using `Add-PnPFile` using `-ContentType` throwing an exception [PR #2619](https://github.com/pnp/PnP-PowerShell/pull/2619)
- Fixed using `Connect-PnPOnline -AppId <appid> -AppSecret <appsecret> -AADDomain` not actually authenticating to Microsoft Graph [PR #2624](https://github.com/pnp/PnP-PowerShell/pull/2624)
- Updated `Get-PnPWorkflowInstance` to allow passing in a workflow subscription to list all running instances of a specific workflow [PR #2636](https://github.com/pnp/PnP-PowerShell/pull/2636)
- Implementation of `Move-PnPFile` has been changed adding `-TargetServerRelativeLibrary` for SharePoint Online to allow moving files to other site collections [PR #2688](https://github.com/pnp/PnP-PowerShell/pull/2688)

### Contributors
- Alberto Suarez [holylander]
- Rune Sperre [rsperre]
- Nik Charlebois [NikCharlebois]
- Eduardo Garcia-Prieto [egarcia74]
- Koen Zomers [koenzomers]
- James May [fowl2]
- Marc D Anderson [sympmarc]
- Kunj Balkrishna Sangani [kunj-sangani]
- Gautam Sheth [gautamdsheth]

## [3.21.2005.0]

### Added

### Changed
- Invoke-PnPSearchQuery: Allow SelectProperties to take a comma separated string as well as an array

### Contributors

## [3.20.2004.0]

### Added
- Added Set-PnPKnowledgeHubSite, Get-PnPKnowledgeHubSite and Remove-PnPKnowledgeHubSite cmdlets
- Added Register-PnPTenantAppCatalog to create and register a new Tenant App Catalog
- Added `IncludeHasTeam` flag to `Get-PnPUnifiedGroup` to include a `HasTeam` flag for each returned Office 365 Group indicating if a Microsoft Team has been set up for it [PR #2612](https://github.com/SharePoint/PnP-PowerShell/pull/2612)
- Added Initialize-PnPPowerShellAuthentication to create a new Azure AD App, self-signed certificate and set the appropriate permission scopes.

### Changed
- Connect-PnPOnline with clientid and certificate will now, if API permissions have been granted for the Graph work with the PnP Graph Cmdlets. [PR #2515](https://github.com/SharePoint/PnP-PowerShell/pull/2515)
- Save-PnPProvisioningTemplate and Save-PnPTenantTemplate now support XML file as input without the need to first read them into a variable with Read-PnPProvisioningTemplate or Read-PnPTenantTemplate
- Added -Schema parameter to Save-PnPProvisioningTemplate and Save-PnPTenantTemplate to force a specific schema for the embedded template. If not specified it defaults always to the latest released template.
- Fixed using `Connect-PnPOnline -PnPO365ManagementShell` only working if your default system language is English [PR #2613](https://github.com/SharePoint/PnP-PowerShell/pull/2613)
- Updated help text of 'Get-PnPClientSidePage' and 'Get-PnPClientSideComponent' to indicate that the out of the box homepage of a modern site will not return its contents as designed [PR #2592](https://github.com/SharePoint/PnP-PowerShell/pull/2592)
- Fixed using `Connect-PnPOnline -PnPO365ManagementShell` not working in PowerShell ISE [PR #2590](https://github.com/SharePoint/PnP-PowerShell/pull/2590)
- Uploading files using `Add-PnPFile` no longer requires Site Owner rights on the entire site. Read rights on the site and at least contribute rights on the document library where the file needs to be uploaded to will suffice as long as the target folder already exists. [PR #2478](https://github.com/SharePoint/PnP-PowerShell/pull/2478)
- Use of `Set-PnPSite` in combination with `-Owners`, `-NoScriptSite`, `-LocaleId` and/or `-AllowSelfServiceUpgrade` is now possible on SharePoint 2013, 2016 and 2019 as well [PR #2293](https://github.com/SharePoint/PnP-PowerShell/pull/2293)
- Using `Connect-PnPOnline -CurrentCredentials` against an on-premises SharePoint farm having more than one authentication provider configured on the webapplication no longer causes an access denied [PR #2571](https://github.com/SharePoint/PnP-PowerShell/pull/2571)
- Fixed `Add-PnPSiteDesignTask` not actually applying the Site Design to the site [PR #2542](https://github.com/SharePoint/PnP-PowerShell/pull/2542)
- Fixed `Get-PnPSiteDesignTask` throwing a collection not initialized error [PR #2545](https://github.com/SharePoint/PnP-PowerShell/pull/2545)

### Contributors
- Lane Blundell [fastlaneb]
- Markus Hanisch [Markus-Hanisch]
- Dan Myhre [danmyhre]
- Jens Otto Hatlevold [jensotto]
- Raphael [PowershellNinja]
- Razvan Hrestic [CodingSinceThe80s]
- Giacomo Pozzoni [jackpoz]
- [victorbutuza]
- Koen Zomers [koenzomers]

## [3.19.2003.0]

### Added
- Added `-HeaderLayoutType` option to `Add-PnPClientSidePage` which allows setting the header layout of a new Site Page [PR # 2514](https://github.com/SharePoint/PnP-PowerShell/pull/2514)
- Added `TermMappingFile` and `SkipTermStoreMapping` parameters to the `ConvertTo-PnPClientSidePage` cmdlet for supporting managed metadata mapping
- Added `AsFileObject` parameter to `Get-PnPFile` which allows the result to be returned as file objects instead of list iems [PR # 2550](https://github.com/SharePoint/PnP-PowerShell/pull/2550)
- Added `RemoveEmptySectionsAndColumns` parameter to the `ConvertTo-PnPClientSidePage` cmdlet for allowing for empty sections and columns to stay after conversion [PR # 2539](https://github.com/SharePoint/PnP-PowerShell/pull/2539)

### Changed
- Fixed issue where downloading a file using `Get-PnPFile` would throw an user not found exception if the user having created or having last modified the file no longer existed [PR #2504](https://github.com/SharePoint/PnP-PowerShell/pull/2504)
- Removed `FieldOptions` argument from `Add-PnPField` as it was marked as obsolete since 2015 already and wasn't used anymore [PR # 2497](https://github.com/SharePoint/PnP-PowerShell/pull/2497)
- Marked the -Rights parameter as obsolete on Grant-PnPHubSiteRights. Use Revoke-PnPHubSiteRights to revoke rights for specific users to join sites ot the hubsite
- Output just the URL for the Redirect url in Get-PnPSearchSettings. Return web setting on sc root as fallback if sc setting is missing (via core).
- Made it possible to set property bag value on a NoScript site using `Set-PnPPropertyBagValue` by providing the argument `Folder` and specifying something other than the root folder [PR # 2544](https://github.com/SharePoint/PnP-PowerShell/pull/2544)
- The February 2020 release of PnP PowerShell was throwing an error on not being able to find and load the Newtonsoft assembly when used in an Azure Function. Fixed in this release.

### Contributors
- Arun Kumar Perumal [arunkumarperumal]
- Paul Bullock [pkbullock]
- Jens Otto Hatlevold [jensotto]
- Pepe [ingepepe]
- [bjdekker]
- [N4TheKing]
- Koen Zomers [koenzomers]
- kadu-jr
- [a1mery]

## [3.18.2002.0]

### Added
- Fixed issue with access token forcing site connection url to be the the value of the Audience in the token
- Added ability to set HostProperties to Add-PnPCustomAction.
- `Get-PnPManagementApiAccessToken` to retrieve access token for the Office 365 Management API using app credentials, app should be registered in AAD and assigned to interact with Management API
- `Get-PnPUnifiedAuditLog` to retrieve unified audit logs from the Office 365 Management API
- Added ability to connect to an on-premises SharePoint 2013/2016/2019 farm using a High Trust Server 2 Server App + User context by providing the username using `-Username`. Before only a High Trust App Only context was possible. [PR #2213](https://github.com/SharePoint/PnP-PowerShell/pull/2213)
- Added ability to use `Set-PnPRequestAccessEmails` with `-Disabled` to disable requesting access to a site and `-Disabled:$false` to set the access requests to be sent to the default owners of the site [PR #2456](https://github.com/SharePoint/PnP-PowerShell/pull/2456)
- Added `Get-PnPSiteScriptFromList` and `Get-PnPSiteScriptFromWeb` commands which allow generation of Site Script JSON based off of existing lists or an entire site [PR # 2459](https://github.com/SharePoint/PnP-PowerShell/pull/2459)
- Added `-Aggregations` argument to `Add-PnPView` and `Set-PnPView` to allow for creating a Totals count in a view [PR #2257](https://github.com/SharePoint/PnP-PowerShell/pull/2257)
- Added `New-PnPTermLabel` to add a localized label to an existing taxonomy term [PR #2475](https://github.com/SharePoint/PnP-PowerShell/pull/2475)
- Deprecated old tenant level `Enable-PnPCommSite` cmdlet and added new `Enable-PnPCommSite` command to enable the modern communication site experience on an classic team site. This one can be applied by non tenant admins as well
- Added `Clear-PnPTenantAppCatalogUrl` to remove the tenant configuration for the tenant scoped app catalog [PR # 2485](https://github.com/SharePoint/PnP-PowerShell/pull/2485)
- Added `Set-PnPTenantAppCatalogUrl` to configure the tenant for the site collection to use for the tenant scoped app catalog [PR # 2485](https://github.com/SharePoint/PnP-PowerShell/pull/2485)

### Changed
- Fixed samples on Set-PnPRequestAccessEmail, added ability to revert back to default owners group, added ability to disable requesting access [PR #2456](https://github.com/SharePoint/PnP-PowerShell/pull/2456)
- Optimized Invoke-PnPSearchQuery when using the -All parameter to ensure all results are returned by ordering on IndexDocId, and changed the default ClientType to 'PnP'
- `Add-PnPFolder` will now return the newly created folder instance [PR #2463](https://github.com/SharePoint/PnP-PowerShell/pull/2463)
- Using `Set-PnPSite -LogoFilePath` now checks if the site collection has a GroupId set instead of validating if the site template name starts with Group to determine if the site is a modern site [PR # 2328](https://github.com/SharePoint/PnP-PowerShell/pull/2328)
- Fixed using `Import-Module` to the PnP PowerShell Module located on a UNC fileshare path resulting in an error [PR # 2490](https://github.com/SharePoint/PnP-PowerShell/pull/2490)
- `New-PnPList` will now return the newly created list instance [PR # 2481](https://github.com/SharePoint/PnP-PowerShell/pull/2481)
- Fixed documentation for Export-PnPTaxonomy [PR #2462](https://github.com/SharePoint/PnP-PowerShell/pull/2462)
- Connect-PnPOnline can now use different Azure Authentication endpoints when using App Only auth. [PR #2355](https://github.com/SharePoint/PnP-PowerShell/pull/2355)
- Fix examples in Start-PnPWorkflowInstance [PR #2483](https://github.com/SharePoint/PnP-PowerShell/pull/2483)

### Contributors
- Ivan Vagunin [ivanvagunin]
- Thomas Meckel [tmeckel]
- Koen Zomers [koenzomers]
- Giacomo Pozzoni [jackpoz]
- Jarbas Horst [JarbasHorst]
- Gautam Sheth [gautamdsheth]
- Craig Hair [MacsInSpace]
- Dan Cecil [danielcecil]
- gobigfoot [gobigfoot]
- Markus Hanisch [Markus-Hanisch]
- Raphael [PowershellNinja]

## [3.17.2001.2]

### 
- No changed to PnP PowerShell but an updated reference to an updated intermediate release of PnP Sites Core which fixes an issue with PnP Provisioning.

## [3.17.2001.1]

### Changed 
- Fixed issue with not correctly identifying framework version and not including required assemblies in packaging.

## [3.17.2001.0]

### Added
- Add/Remove/Set/Get-PnPApplicationCustomizer commands to allow working with SharePoint Framework Application Customizer Extensions
- Ability to pipe in a result from Get-PnPFolder to Get-PnPFolderItem using the -Identity parameter
- Added ability to pipe Get-PnPUnifiedGroup to Get-PnPUnifiedGroupOwners and Get-PnPUnifiedGroupMembers
- Added permissions required for each of the \*-PnPUnifiedGroup\* commands in the Azure Active Directory App Registration to the help text of the commands
- Added option to use Connect-PnPOnline with a base64 encoded private key for use in i.e. PnP PowerShell within an Azure Function v1 and an option to provide a certificate reference for use in i.e. Azure Function v2
- Added option to use Connect-PnPOnline with a public key certificate for use in i.e. Azure Runbooks
- Added option -RowLimit to Get-PnPRecycleBinItem to avoid getting throttled on full recycle bins
- Added `-WriteToConsole` option to `Set-PnPTraceLog` to allow writing trace listener output from the PnP Templating commands to both the console and to a file. Doesn't work for .NET Core.
- Added `Reset-PnPLabel` command to allow removal of an Office 365 Retention Label from a list
- Add/Remove/Get-PnPOrgNewsSite commands to set site collections as authoritive news sources to SharePoint Online
- Add/Remove/Get-PnPOrgAssetsLibrary commands to set document libraries as organizational asset sources on SharePoint Online
- `-Recursive` option to `Get-PnPFolderItem` to allow retrieving all files and folders recursively

### Changed 
- Marked Get-PnPHealthScore as obsolete for SharePoint Online.
- Fixes issues with connections not properly closing under some conditions when using Disconnect-PnPOnline
- When using commands that utilize the Graph API but not being connected to one of the Graph API Connect-PnPOnline methods, it would throw a NullReferenceException. It will now throw a cleaner exception indicating you should connect with the Graph API first.
- Fixed an issue where using Get-PnPUser -WithRightsAssigned would not return the proper users with actually having access to that site
- Fixed an issue when using ConvertTo-PnPClientSidePage to convert Delve Blog posts that it would throw a nullreference exception in some scenarios
- Fixed an issue using `Add-PnPDataRowsToProvisioningTemp` to add data from a list containing a multi choice to a PnP Provisioning Template where the data would be shown as `System.String[]` instead of the actual data
- Bumped to .Net 4.6.1 as minimal .Net runtime version
- Changed the way properties are being set in Set-PnPField to support setting field specific properties such as the Lookup list on a Lookup field
- Fixed an issue where using Apply-PnPProvisioningTemplate -InputInstance $instance would throw a connectionString error if being executed from the root of a drive, i.e. c:\ or d:\
- Fixed issue with access token not returning correctly after update to newer version of NewtonSoft JSON.
- Fixes issue with pipeline not returning object correctly.

### Contributors
- Koen Zomers [koenzomers]
- Robin Meure [robinmeure]
- Michael Rees Pullen [mrpullen]
- Giacomo Pozzoni [jackpoz]
- Rene Modery [modery]
- Krystian Niepsuj [MrDoNotBreak]
- Piotr Siatka [siata13]
- Heinrich Ulbricht [heinrich-ulbricht]
- Dan Cecil [danielcecil]
- Gautam Sheth [gautamdsheth]
- Giacomo Pozzoni [jackpoz]
- Will Holland [willholland]
- Ivan Vagunin [ivanvagunin]

## [3.16.1912.0]

### Added

- Add-PnPTeamsTeam: new cmdlet that creates a Teams team for the current, Office 365 group connected, site collection
- Added Get-PnPTenantId to retrieve the current tenant id.
- ConvertTo-PnPClientSidePage: Added support for enforcing the specified target page folder via `-TargetPageFolderOverridesDefaultFolder`
- ConvertTo-PnPClientSidePage: Added support for Delve blog page modernization via the `-DelveBlogPage` and `-DelveKeepSubTitle ` parameters

### Changed 

- Added various additional switches and options to Set-PnPTenantSite
- Added -Wait parameter to New-PnPSite which will wait until the site creation process has been completely finished and all artifacts are present.
- Fixes issue with App Only with certificate and context cloning. Now Apply-PnPTenantTemplate works as expected.
- Added CorrelationId and TimeStampUtc to output of Get-PnPException which can help in analyzing ULS entries.
- ConvertTo-PnPClientSidePage: The `-Identity` parameter now also accepts the item id as value to find a page

### Contributors

## [3.15.1911.0]

### Added

- Added Request-PnPAccessToken to retrieve an OAuth2 access token using the password grant.
- Added additional properties to Set-PnPHubSite
- Added Get-PnPHubSiteChild cmdlet to list all child sites of a hubsite
- ConvertTo-PnPClientSidePage: Added support for user mapping via `-UserMappingFile, `-LDAPConnectionString` and `-SkipUserMapping` parameters  #2340
- ConvertTo-PnPClientSidePage: Added support for defining the target folder of a transformed page via `-TargetPageFolder`
- Added Get-PnPSearchSettings to retreive current search settings for a site
- Added Set-PnPSearchSettings to set search related settings on a site

### Changed

- Cmdlets related to provisioning and tenant templates now output more detailed error information in case of a schema issue.
- Fixes issue where site design was not being applied when using New-PnPSite
- Fixed incorrect usage of SwitchParameter in Set-PnPSite cmdlet
- Fixed issue when connecting to single level domain URLs
- Disabled TimeZone as mandatory parameter for New-PnPTenantSite when using an on-premises version of PnP PowerShell

### Contributors

- Gautam Sheth [gautamdsheth]
- Koen Zomers [KoenZomers]
- Laurens Hoogendoorn [laurens1984]
- Jens Otto Hatlevold [jensotto]
- Paul Bullock [pkbullock]

## [3.14.1910.1]

### Added

- ConvertTo-PnPClientSidePage: Added support for logging to console via `-LogType Console`
- Copy-PnPFile: Fixes (#2300)
- ConvertTo-PnPClientSidePage: Added support for controlling the target page name is any cross site transformation (so wiki, web part, blog in addition the already existing option for publishing pages) via the `-TargetPageName` parameter

### Changed

### Contributors

## [3.14.1910.0]

### Added

- Added Set-PnPFolderPermission to set specific folder permissions
- ConvertTo-PnPClientSidePage: Added support for keeping the source page Author, (Editor), Created and Modified page properties (only when source page lives in SPO) (KeepPageCreationModificationInformation parameter)
- ConvertTo-PnPClientSidePage: Added support for posting the created page as news (PostAsNews parameter)
- ConvertTo-PnPClientSidePage: Added support for modernizing blog pages (BlogPage parameter)
- ConvertTo-PnPClientSidePage: Added option to populate the author in the modern page header based upon the author of the source page (only when source page lives in SPO) (SetAuthorInPageHeader parameter)
- Export-ClientSidePageMappings: Added logging support (#2272)

### Changed

- Several documentation fixes
- Add-PnPClientSideWebPart now also works for SP2019
- Added -List parameter to Get-PnPFolder to retrieve all folders in a list
- Added owner paramter to New-PnPSite when create Communications site
- Fixed issues after static code analysis
- Added -ThumbnailUrl parameter to Set-PnPClientSidePage
- ConvertTo-PnPClientSidePage: AddTableListImageAsImageWebPart default set to true to align with similar change in the page transformation framework
- ConvertTo-PnPClientSidePage: moved log flushing to finally block to ensure it happens in case of something unexpected

### Contributors

- Aleksandr SaPozhkov [shurick81]
- Garry Trinder [garrytrinder]
- Koen Zomers [KoenZomers]
- Gautam Sheth [gautamdsheth]
- Giacomo Pozzoni [jackpoz]
- Paul Bullock [pkbullock]
- Andres Mariano Gorzelany [get-itips]

## [3.13.1909.0]

### Added

### Changed

- Added -Label parameter to Add-PnPList and Set-PnPListItem to allow setting a retention label
- ConvertTo-PnPClientSidePage: Added support for skipping the default URL rewriting while still applying any custom URL rewriting if specified (SkipDefaultUrlRewriting parameter)
- ConvertTo-PnPClientSidePage: Added support reverting to the pre September 2019 behaviour for images insides tables/lists. As of September 2019 these images are not created anymore as additional separate image web part since the modern text editor is not dropping the embedded images anymore on edit (AddTableListImageAsImageWebPart parameter)
- Get-PnPSearchCrawlLog: Added switch to show raw crawl log data, as data can change in the back-end. Fixed to show log message.
- Set-PnPTenant: Added switch to set disabled 1st party web parts

### Contributors

- Dan Cecil [danielcecil]
- Koen zomers [KoenZomers]

## [3.12.1908.0]

### Added
- Added -ResetRoleInheritance to Set-PnPList
- Documentation updates
- Added a TemplateId parameter to Apply-PnPProvisioningTemplate to allow to apply a specific template in case multiple templates exist in a single file.

### Changed

- Fixed potential issue when using -CurrentCredentials with Connect-PnPOnline in an on-premises context
- Fixed bug in Set-PnPListItem when using SystemUpdate and setting a content type.
- Grant-PnPTenantServicePrincipalPermission now handles multi-language environments where the Tenant App Catalog is in a different language than English.
- Save-PnPProvisioningTemplate, if saving an instance to a PnP file, will now store referenced files etc in the PnP file.

### Contributors

- Lars Fernhomberg [lafe]
- Chris O'Connor [kachihro]
- Koen Zomers [KoenZomers]
- Gautam Sheth [gautamdsheth]
- Andres Mariano Gorzelany [get-itips]

## [3.11.1907.0]

### Added

- Added Export-PnPListToProvisioningTemplate cmdlet to export one or more lists to a provisioning template skipping all other artifacts.

### Changed

- ConvertTo-PnPClientSidePage: Added support for specifying a custom URL mapping file (UrlMappingFile parameter)
- Get-PnPField: Return managed metadata fields as TaxonomyField instead of generic Field (#2130)
- Submit-PnPSearchQuery: Added alias Invoke-PnPSearchQuery for semantic aligning the verbs (#2168)
- Copy-PnPFile: Bugfix (#2103 #2148)

### Contributors
- Andi Krüger [andikrueger]

## [3.10.1906.0]

### Added

- Several bugfixes
- Save-PnPClientSidePageConversionLog: use this cmdlet to save the pending page transformation logs. Needs to be used in conjunction with the -LogSkipFlush flag on the ConvertTo-PnPClientSidePage cmdlet.

### Changed

- Updated documentation for several cmdlets
- Cleanup private key only for file or pem based certificate login (#2101)
- ConvertTo-PnPClientSidePage: Added support to transform web part pages that live outside of a library (so in the root of the site)
- ConvertTo-PnPClientSidePage: Added support to specify the target site as a connection using the TargetConnection parameter. This allows to read a page in one environment (on-premises, tenant A) and create in another online location (tenant B). (#2098)

### Contributors

- Paul Bullock [pkbullock]
- Andres Mariano Gorzelany [get-itips]
- Koen Zomers [KoenZomers]
- Giacomo Pozzoni [jackpoz]
- Tom Resing [tomresing]

## [3.9.1905.3 - May 2019 Intermediate Release 3]

### Changed

- Updated core provisioning to handle token issue during extraction and reintroduced content type fieldlink reordering in the engine.

## [3.9.1905.2 - May 2019 Intermediate Release 2]

### Changed
- Updated core provisioning to handle token parser issue

## [3.9.1905.1 - May 2019 Intermediate Release]

### Added

### Changed
- Updated core provisioning engine to handle a server side issue.
- Added support for certificate thumbprint login with ADAL and updated connection sample
- Added support for outputting .cer file from New-PnPAzureCertificate

### Deprecated
- Out parameter for New-PnPAzureCertificate replaced with OutPfx

### Contributors

## [3.9.1905.0 - May 2019 Release]

### Added

- Added Template as a possible PromoteAs value for a Add-PnPClientSidePage and Set-PnPClientSidePage
- Added -HeaderLayout and -HeaderEmphasis parameters to Set-PnPWeb
- Support to specify lcid for Export-PnPTaxonomy for a particular termset
- Added support in the Navigation cmdlets to manage the site footer on modern sites.
- Added Invoke-PnPSPRestMethod cmdlet to execute REST requests towards a SharePoint site.
- Added Enable-PnPCommSite cmdlet to convert the root site collection of a tenant into a communication site.

### Changed

- Updated documentation
- ConvertTo-PnPClientSidePage: modernize the first page in case there's multiple pages matching the provided pattern (parameters identity, folder and library)
- ConvertTo-PnPClientSidePage: added parameter `-PublishingTargetPageName` parameter that allows one to override the name of the target publishing page. This is needed for some pages like default.aspx
- ConvertTo-PnPClientSidePage: added parameter `-SkipUrlRewrite` to prevent URL rewriting in cross site transformation scenarios
- Export-PnPClientSidePageMapping: added parameter `-PublishingPage` to scope the page layout analysis to the page layout of the provided file
- Export-PnPClientSidePageMapping: added parameter `-AnalyzeOOBPageLayouts` to allow analysis of OOB page layouts. By default OOB page layouts will be skipped
- Fix to allow setting list property bag values on NoScript sites

### Deprecated
- Removed support for the Template Gallery as the gallery itself is not online anymore.

### Contributors
- Heinrich Ulbricht [heinrich-ulbricht] 
- Andres Mariano Gorzelany [get-itips]

## [3.8.1904.0]

### Added

- Added Sync-PnPAppToTeams to synchronize an app from the tenant app catalog to the Microsoft Teams App Catalog
- Added Export-PnPClientSidePageMapping to export the mapping files needed during publishing page transformation

### Changed

- Added a -Kerberos switch to Connect-PnPOnline to facility authentication using Kerberos
- Added the ability to set the view fields using Set-PnPView -Fields
- Added the ability to add and removed indexed property keys to lists
- Added the option to search by title on Get-PnPAlert
- Added -CreateTeam to New-PnPUnifiedGroup and Set-PnPUnifiedGroup
- Added -ContentType parameter to Add-PnPClientSidePage and Set-PnPClientSidePage
- ConvertTo-PnPClientSidePage: added -Library and -Folder parameters to support modernization of pages living outside of the SitePages folder
- ConvertTo-PnPClientSidePage: added -LogType, -LogFolder, -LogSkipFlush and -LogVerbose parameters to support log generation to an md file or SharePoint page
- ConvertTo-PnPClientSidePage: added -DontPublish and -DisablePageComments parameters to control the page publishing and commenting
- ConvertTo-PnPClientSidePage: added -PublishingPage and -PageLayoutMapping to support publishing page transformation

### Contributors

- Heinrich Ulbricht [heinrich-ulbricht] 
- Gautam Sheth [gautamdsheth]
- Thomas Meckel [tmeckel]
- Jose Gabriel Ortega Castro [j0rt3g4]
- Fabian Seither [fabianseither]

## [3.7.1903.0]

### Added
- Added support for client side pages on SP2019
- Added support for ALM cmdlets (Add-PnPApp, Get-PnPApp etc.) on SP2019
- Added Add-PnPAlert cmdlet to create alerts (SPO and SP2019 only)
- Added Get-PnPAlert to list alerts (SPO and SP2019 only)
- Added Remove-PnPAlert to remove alerts (SPO and SP2019 only)
- Added support to Connect-PnPOnline authenticate to SharePoint Online when Legacy Authentication has been turned off (Set-PnPTenant -LegacyAuthProtocolsEnabled:$false / Set-SPOTenant -LegacyAuthProtocolsEnabled:$false)
- Support for cross site page transformation (create modern pages in other site then the one hosting the classic pages) via the TargetWebUrl parameter
- Support for page transformation mapping parameters (UseCommunityScriptEditor and SummaryLinksToHtml). The first one will use the community script editor as a possible modern target web part, the second one will transform the summarylinks web part to html text instead of the default QuickLinks web part

### Changed
- Many typo fixes in code
- Apply-PnPTenantTemplate will now list the sites created after applying a tenant template.
- Fixed an issue with Connect-PnPOnline throwing an exception when authenticating using the -SPOManagementShell parameter.
- Fixed connection issue with URL's containing spaces - #1250

### Contributors

- Heinrich Ulbricht [heinrich-ulbricht] 
- Nick Schonning [nschonni]
- Koen Zomers [KoenZomers]
- Marvin Dickhaus [Weishaupt]
- Lars Fernhomberg [lafe]

## [3.6.1902.2]

### Added

### Changed
- Fixed issue where New-PnPSite would through a null reference exception when creating a site collection without associating it to a hubsite.
- Fixed issue were Save-PnPTenantTemplate was not adding files the PnP file.
- Fixed issue where Save-PnPTenantTemplate would not allow files larger than 10MB.

### Contributors

## [3.6.1902.1]

### Added

### Changed
- Fixed Set-PnPTenantSite where the NoScriptSite parameter would always be set to false if not specified.

### Contributors


## [3.6.1902.0]

### Added
- Added initial support for SharePoint 2019
- Added Clear-PnPDefaultColumnValues cmdlet
- Added Remove-PnPSearchConfiguration cmdlet
- Added Export-PnPClientSidePage to export a page to a Provisioning Template
- Added Add-PnPSiteDesignTask to apply a site design to a site. Intended as a replacement for Invoke-PnPSiteDesign as it the task can handle more than 30 actions.
- Added Get-PnPSiteDesignRun to retrieve the list of site designs applied to a site collection
- Added Get-PnPSiteDesignRunStatus to retrieve a list of all site script actions executed for a specified site design applied to a site
- Added Get-PnPSiteDesignTask to retrieve a list of all currently scheduled site design tasks.
- Added Remove-PnPSiteDesignTask to remove a previously scheduled site design task.
- Added -IncludeHiddenLists to Get-PnPProvisioningTemplate to optionally also extract hidden lists in a template.
- Added -HubSiteId to New-PnPSite to associate the site with a hubsite at creation time
- Added -Owners to New-PnPSite to set the owners while creating a modern team site.

### Changed
- Set-PnPDefaultColumnValues: Fixed character encoding issue on folders #1706
- Fixed import of search configuration to tenant via string
- Set-PnPTenantSite: Added support for setting default sharing and sharing permissions
- ConvertTo-PnPClientSidePage: Added support for copying page metadata to the modern version of the page + parameter to clear the transformation cache
- Enable-PnPTelemetry and Disable-PnPTelemetry do not require a connection anymore.
- Return more friendly exception if App Catalog does not exist when using Set-PnPStorageEntity, Get-PnPStorageEntity or Remove-PnPStorageEntity
- Added -SystemUpdate flag to Set-PnPListItemPermission
- Clean up temp data when using PEM string certificates, and support password on PEM string certificates.
- Updated Set-PnPGroup to update both the Notes -and- the Description of a SharePoint group if using the -Description parameter

### Contributors
- Koen Zomers (KoenZomers)
- Gautam Sheth (gautamdsheth)

## [3.5.1901.0]

### Added
- Added Reset-PnPFileVersion cmdlet

### Changed

- Add-PnPClientSidePageSection: Added support for section the section background of a client side page
- Updated file and folder cmdlets to support special characters

### Deprecated

### Contributors
- Eric Skaggs (skaggej)
- Gautam Sheth (gautamdsheth)

## [3.4.1812.0]

### Added

- ConvertTo-PnPClientSidePage: creates a modern client side page from a classic wiki or web part page

### Changed
- Added support for setting the page header type in Set-PnPClientSidePage

### Deprecated

### Contributors

## [3.3.1811.0]
### Added

### Changed
- Copy-PnPFile now supports special characters like '&' in file names
- Updated New-PnPSite to support language/locale for new sites.
- Updated documentation for New-PnPTenantSite
- Fixed documentation for Measure-PnPWeb, Set-PnPSite
- Updated samples
- Fixes issue with Set-PnPUnifiedGroup where if you only change for instance the displayname a private group would be marked as public.
- Renamed (and created aliases for the old cmdlet name) Apply-PnPProvisioningHierarchy to Apply-PnPTenantTemplate
- Renamed (and created aliases for the old cmdlet name) Add-PnPProvisioningSequence to Add-PnPTenantSequence
- Renamed (and created aliases for the old cmdlet name) Add-PnPProvisioningSite to Add-PnPTenantSequenceSite
- Renamed (and created aliases for the old cmdlet name) Add-PnPPnPProvisioningSubSite to Add-PnPTenantSequenceSubSite
- Renamed (and created aliases for the old cmdlet name) Get-PnPProvisioningSequence to Get-PnPTenantSequence
- Renamed (and created aliases for the old cmdlet name) Get-PnPProvisioningSite to Get-PnPTenantSequenceSite
- Renamed (and created aliases for the old cmdlet name) New-PnPProvisioningSequence to New-PnPTenantSequence
- Renamed (and created aliases for the old cmdlet name) New-PnPProvisioningTeamSite to New-PnPTenantSequenceTeamSite
- Renamed (and created aliases for the old cmdlet name) New-PnPProvisioningCommunicationSite to New-PnPTenantSequenceCommunicationSite
- Renamed (and created aliases for the old cmdlet name) New-PnPProvisioningTeamNoGroupSite to New-PnPTenantSequenceTeamNoGroupSite
- Renamed (and created aliases for the old cmdlet name) New-PnPProvisioningTeamNoGroupSubSite to New-PnPTenantSequenceTeamNoGroupSubSite
- Renamed (and created aliases for the old cmdlet name) New-PnPProvisioningHierarchy to New-PnPTenantTemplate
- Renamed (and created aliases for the old cmdlet name) Read-PnPProvisioningHierarchy to Read-PnPTenantTemplate
- Renamed (and created aliases for the old cmdlet name) Save-PnPProvisioningHierarchy to Save-PnPTenantTemplate
- Renamed (and created aliases for the old cmdlet name) Test-PnPProvisioningHierarchy to Test-PnPTenantTemplate

### Deprecated
- Marked Get-PnPProvisioningTemplateFromGallery as deprecated as the PnP Template Gallery has been shut down.

### Contributors
- Paul Bullock (pkbullock)
- Fran�ois-Xavier Cat (lazywinadmin)
- Koen Zomers (KoenZomers)
- Kevin McDonnell (kevmcdonk)

## [3.2.1810.0] Released
### Added
- Add-PnPProvisioningSequence : Adds an in-memory sequence to an in-memory provisioning hierarchy
- Add-PnPProvisioningSite : Adds an in-memory site definition to a in-memory sequence
- Add-PnPProvisioningSubSite : Adds an in-memory sub site defintion to an in-memory site
- Apply-PnPProvisioningHierarchy : Applies a provisioninghierarchy with a site sequence to a tenant
- Get-PnPProvisioningSite : Returns a site as an in-memory object from a given provisioning hierarchy
- New-PnPProvisioningHierarchy : Creates a new in-memory provisioning hierarchy
- New-PnPProvisioningSequence : Creates a new in-memory provisioning sequence
- New-PnPProvisioningCommunicationSite : Creates a new in-memory communication site definition
- New-PnPProvisioningTeamNoGroupSite : Creates a new in-memory team site definition which has no associated office365 group
- New-PnPProvisioningTeamNoGroupSubSite : Creates a new in-memory team sub site definition which has no associated office365 group
- New-PnPProvisioningTeamSite : Creates a new in-memory team site definition
- Read-PnPProvisioningHierarchy : Reads an existing (file based) provisioning hierarchy into an in-memory instance
- Save-PnPProvisioningHierarchy : Saves an in-memory provisioning hierarchy to a pnp file
- Test-PnPProvisioningHierarchy : Tests an in-memory hierarchy if all template references are correct in the site sequence
- Get-PnPException : Returns the last occurred exception that occurred while using PowerShell.

### Changed
- Updated Set-PnPSite to allow for setting of a logo on modern team site
- Updated Get-PnPTerm to allow for -IncludeChildTerms parameter, which will load, if available all child terms
- Updated Get-PnPTerm to allow for only specifying the id of a termset, without needing to require to specify the termset and termgroup.

### Deprecated

### Contributors

## [3.1.1809.0]

### Changed
- Minor bugfixes
- Updated core library

## [3.0.1808.0]
### Added
- Added Get-PnPLabel and Set-PnPLabel to get and set compliancy tags/labels on a list or library. Only available for SharePoint Online.

### Changed
- Fixed Get-PnPSearchCrawlLog where listing user profile crawl entries failed for some tenants
- Added default pipebind to Get-PnPListitem 
- Add-PnPDocumentSet now adds the content type to the document library.
- Updated documentation for Clear-PnPRecycleBinItem and Restore-PnPRecycleBinItem
- Updated documentation for New-PnPSite

### Contributors
- KoenZomers
- robinmeure

## [2.28.1807.0]
### Changed
- Added IncludeClassification to Get-PnPUnifiedGroup
- Updated documentation for Get-PnPSearchCrawlLog
- Added -NewFileName to Add-PnPFile cmdlet

### Contributors
- vipulkelkar
- wobba
- koenzomers

## [2.27.1806.0]
### Added
- Added Grant-PnPTenantServicePrincipalPermission to explicitly grant a permission on a resource for the tenant.

### Changed
- Fixed edge cases where progress sent to PowerShell would be null, causing the provisioning of a template to end prematurely.
- Fixed Unregister-PnPHubSite where you could not unregister a hub site if the site was deleted before unregistering

### Deprecated

### Contributors

## [2.26.1805.1]
### Added

- Added -Timeout option to Add-PnPApp
- Added -CollapseSpecification option to Submit-PnPSearchQuery
- Added -InSiteHierarchy to Get-PnPField to search for fields in the site collection
- Added Get-PnPSearchCrawlLog

### Changed
- Fix for issue where using Add-PnPFile and setting Created and Modified did not update values

## [2.26.1805.0]
### Added
- Added Enable-PnPPowerShellTelemetry, Disable-PnPPowerShellTelemetry, Get-PnPPowershellTelemetryEnabled
- Added Enable-PnPTenantServicePrincipal
- Added Disable-PnPTenantServicePrincipal
- Added Get-PnPTenantServicePrincipal
- Added Get-PnPTenantServicePermissionRequests
- Added Get-PnPTenantServicePermissionGrants
- Added Approve-PnPTenantServicePrincipalPermissionRequest
- Added Deny-PnPTenantServicePrincipalPermissionRequest
- Added Revoke-PnPTenantServicePrincipalPermission
- Added -Scope parameter to Get-PnPStorageEntity, Set-PnPStorageEntity and Remove-PnPStorageEntity to allow for handling storage entity on site collection scope. This only works on site collections which have a site collection app catalog available.
- Added -CertificatePassword option to New-PnPAzureCertificate
- Added output of thumbprint for New-PnPAzureCertificate and Get-PnPAzureCertificat

### Changed
- Added -NoTelemetry switch to Connect-PnPOnline
- Updated Connect-PnPOnline to allow for -LoginProviderName when using -UseAdfs to authenticate
- Fixed issue where Add-PnPApp would fail where -Publish parameter was specified and -Scope was set to Site
- Fixed issue where New-PnPUnifiedGroup prompted for creation even though mail alias did not exist

### Deprecated

### Contributors
- Martin Duceb [cebud]
- Kev Maitland [kevmaitland]
- Martin Loitzl [mloitzl]

## [2.25.1804.1]
### Changed
- Now using signed core library assembly
- Updated Set-PnPTenantSite to handle changing the Site Lock State correctly. You cannot use both -LockState and set other properties at the same time due to possible delays in making the lockstate effective.


## [2.25.1804.0]
### Added
- Added -Tree parameter to Get-PnPNavigationNode which will return a tree representation of the selected navigation structure
- Added -Parent parameter which takes an ID to Add-PnPNavigationNode instead of using the -Header parameter
- Added -Scope parameter to Add-PnPApp, Get-PnPApp, Install-PnPApp, Publish-PnPApp, Remove-PnPApp, Uninstall-PnPApp, Unpublish-PnPApp, Update-PnPApp to support site collection app catalog
- Added -Wait parameter to Install-PnPApp which will wait for the installation to finish
- Added Get-PnPHideDefaultThemes cmdlet
- Added Set-PnPHideDefaultThemes cmdlet
- Added Get-PnPListRecordDeclaration cmdlet
- Added Set-PnPListRecordDeclaration cmdlet
- Added Get-PnPInPlaceRecordsManagement cmdlet
- Added Get-PnPInformationRightsManagement cmdlet
- Added Set-PnPInformationRightsManagement cmdlet
- Added New-PnPUPABulkImportJob cmdlet
- Added Get-PnPUPABulkImportStatus cmdlet

### Changed

- Added additional properties to Set-PnPList: Description, EnableFolderCreation, ForceCheckout, ListExperience
- ALM Cmdlets (Add-PnPApp, etc.) now allow for specifying the app title instead of only an id.
- Updated Set-PnPInPlaceRecordsManagement cmdlet to use a -Enabled parameter instead of -On and -Off
- Add-PnPClientSideWebPart and Add-PnPClientSideText now return the client side component added
- Fixed issue with Set-PnPTenantTheme not recognizing a parameter value accordingly
- Added -HideDefaultThemes parameter to Set-PnPTenant
- Get-PnPTenant now returns if default themes are hidden or not
- Added ability to cancel Device Login requests with CTRL+C
- Renamed Connect-PnPHubSite to Add-PnPHubSiteAssociation and added alias for Connect-PnPHubSite
- Renamed Disconnect-PnPHubSite to Remove-PnPHubSiteAssociation and added alias for Disconnect-PnPHubSite
- Fixed output of File/Folder objects which caused the creation of an error message that was not thrown to the output but was available in the $error built-in variable
- Fixed Set-PnPUserProfileProperty cmdlet to accept $null values to clear properties
- Fixed Invoke-PnPSiteDesign where you connected to the -admin URL, and it ignored the WebUrl parameter when applying the site design
- Added WebUrl parameter to Set-PnPWebTheme to support connection via -admin URL needed by app-only connections
- Fixed issue with 

### Deprecated
- Deprecated -Header parameter on Add-PnPNavigationNode in favor or -Parent [Id]
- Deprecated Disable-PnPInPlaceRecordsManagementForSite in favor of Set-PnPInPlaceRecordsManagement -Enabled $true
- Deprecated Enabled-PnPInPlaceRecordsManagementForSite in favor of Set-PnPInPlaceRecordsManagement -Disabled $true
- Deprecated Connect-PnPHubSite. Use Add-PnPHubSiteAssociation
- Deprecated Disconnect-PnPHubSite. Use Remove-PnPHubSiteAssociation

### Contributors
casselc
stevebeauge
velingeorgiev
cebud
jensotto


## [2.24.1803.0] - 2018-03-06
### Added
- Added Get-PnPTenant cmdlet
- Added Set-PnPTenant cmdlet
- Added Set-PnPWebTheme cmdlet
- Added Invoke-PnPSiteDesign cmdlet
- Added Read-PnPProvisioningTemplate cmdlet [Rename: see deprecated section]
- Added Invoke-PnPQuery cmdlet [Rename: see deprecated section]
- Added Resolve-PnPFolder cmdlet [Rename: see deprecated section]
- Added New-PnPAzureCertificate cmdlet
- Added Get-PnPAzureCertificate cmdlet
- Added Test-PnPOffice365GroupAliasIsUsed cmdlet
- Added Remove-PnPStoredCredential
- Added Add-PnPStoredCredential
- Added Get-PnPHubSite cmdlet
- Added Set-PnPHubSite cmdlet
- Added Grant-PnPHubSiteRights cmdlet
- Added Register-PnPHubSite cmdlet
- Added Unregister-PnPHubSite cmdlet
- Added Connect-PnPHubSite cmdlet
- Added Disconnect-PnPHubSite cmdlet
- Added Add-PnPTenantTheme cmdlet
- Added Get-PnPTenantTheme cmdlet
- Added Remove-PnPTenantTheme cmdlet
- Added Set-PnPTenantCdnEnabled cmdlet
- Added Get-PnPTenantCdnEnabled cmdlet
- Added Get-PnPTenantCdnOrigin cmdlet
- Added Add-PnPTenantCdnOrigin cmdlet
- Added Remove-PnPTenantCdnOrigin cmdlet
- Added Get-PnPTenantCdnPolicies cmdlet
- Added Set-PnPTenantCdnPolicy cmdlet
- Added Add-PnPSiteCollectionAppCatalog cmdlet
- Added Remove-PnPSiteCollectionAppCatalog cmdlet
- Added Get-PnPNavigationNode cmdlet
- Added Get-PnPRoleDefinition cmdlet
- Added Add-PnPRoleDefinition cmdlet
- Added Remove-PnPRoleDefinition cmdlet
- Implemented .NET 2.0 Standard project to allow for cross-platform use with PowerShell 6.0

### Changed
- Added "Formula" dynamic parameter to Add-PnPField to allow creating calculated fields.
- Updated Set-PnPClientSidePage to support setting the page title
- Added -Graph [and -LaunchBrowser] option to authenticate with Connect-PnPOnline to the Graph using the PnP O365 Management Shell Azure AD Application 
- Updated the UnifiedGroup cmdlets to also take an Alias of group as a value for the -Identity parameter
- Minor documentations updates [thechriskent]
- Updated Connect-PnPOnline to support connecting using PEM encoded certificate strings
- Updated Connect-PnPOnline for On-Premises to allow for additional HighTrustCertificate parameters [fowl2]
- Added -EnableAttachment parameter for Set-PnPList [Laskewitz]
- Added -Approve parameter for Set-PnPFileCheckedIn [Aproxmiation]
- Added -EnableModeration for Set-PnPList [Apromixation]
- Fixed issue where it was not possible to use New-PnPSite when using Connect-PnPOnline with the -UseWebLogin parameter
- Fixed issue with Copy-PnPFile when copying to a location within the current web where metadata was not being retained
- Fixed issue with Add-PnPFile when a new file was uploaded and using the cmdlet also field values where set, the version would increase to 2.0 instead of the expected 1.0
- Fixed issues with Set-PnPTheme cmdlet not accepting site relative urls
- Move-PnPFolder now returns the folder that has been moved
- Updated Get-PnPStoredCredentials to support .NET Standard
- Updated/fixed documentation on various cmdlets
- Fixed issue with Get-PnPTenantSite not returning all sites in large tenants
- Added -PnPO365ManagementShell [and -LaunchBrowser] login option to Connect-PnPOnline
- Changed changelog format
- Updated Remove-PnPNavigationNode cmdlet to support removal by Id
- Updated Remove-PnPNavigationNode cmdlet to support the -All parameter
- Updated Set-PnPList cmdlet to change moderation setting
- Updated Set-PnPFileCheckedIn to approve the file

### Deprecated
- [SharePoint Online Only] Deprecated Get-PnPAppInstance, Import-PnPAppPackage, Uninstall-PnPAppPackage. Use Add-PnPApp, Install-PnPApp, Publish-PnPApp, Uninstall-PnPApp, Remove-PnPApp instead where applicable.
- Deprecated Load-PnPProvisioningTemplate, renaming it to Read-PnPProvisioningTemplate which follows the PowerShell approved verb standard. Load-PnPProvisioningTemplate has been added as an alias for Read-PnPProvisioningTemplate.
- Deprecated Execute-PnPQuery, renaming it to Invoke-PnPQuery which follows the PowerShell approved verb standard. Execute-PnPQuery has been added as an alias for Invoke-PnPQuery.
- Deprecated Ensure-PnPFolder, moving functionality to Resolve-PnPFolder which follows the PowerShell approved verb standard. Ensure-PnPFolder has been added as an alias for Resolve-PnPFolder.
- Documentation/Markdown generation has been removed from build, now points to https://docs.microsoft.com/en-us/powershell/sharepoint/sharepoint-pnp/sharepoint-pnp-cmdlets?view=sharepoint-ps
- Deprecated Remove-PnPNavigationNode -Title and -Header parameters. Use the Identity parameter instead.
- Marked -WebTemplate parameter on Get-PnPTenantSite as obsolete. Use -Template instead.
- Deprecated Get-PnPAzureADManifestKeyCredentials. Use Get-PnPAzureCertificate instead.

## [2.23.1802.0] - 2018-02-05
### Added
- Added Set-PnPSiteDesign and Set-PnPSiteScript cmdlets

## [2.22.1801.0]
### Added
- Added Get-PnPTenantAppCatalogUrl
- Start-PnPWorkflowInstance
- Get-PnPWorkflowInstance

**2017-12-06**
* Added cmdlets for Site Designs: Add-PnPSiteDesign, Add-PnPSiteScript, Get-PnPSiteDesign, Get-PnPSiteScript, Get-PnPSiteDesignRights, Grant-PnPSiteDesignRights, Remove-PnPSiteDesign, Remove-PnPSiteScript, Revoke-PnPSiteDesignRights

**2017-12-02**
* Added additional authentication option with Connect-PnPOnline allowing you use an existing Access Token for authentication
* Added Get-PnPClientSideComponent, Move-PnPClientSideComponent, Remove-PnPClientSideComponent, Set-PnPClientSideText, Set-PnPClientSideWebPart

**2017-11-20**
* Added Measure-PnPWeb, Measure-PnPList and Measure-PnPResponseTime cmdlets
* Added Set-PnPStorageEntity, Get-PnPStorageEntity and Remove-PnPStorageEntity cmdlets to manage storage entities / farm properties

**2017-11-19**
* Fixed issues with Set-PnPListItem -Values, Add-PnPListItem -Values and Add-PnPFile -Values, updated documentation for these cmdlets
* Added confirmation prompt to Get-PnPFile to ask if local file should be overwritten. Use -Force to overwrite this

**2017-05-06**
* Added Set-PnPWebPermissions
* Updated Get-PnPListItem to execute scriptblocks
* Added Set-PnPListItemPermissions
* Added Get-PnPDefaultColumnValues
* Added Set-PnPListPermissions


**2017-01-27**
* Added Get-PnPTerm
* Added Get-PnPTermSet
* Added New-PnPTerm
* Added New-PnPTermSet
* Added New-PnPTermGroup
* Updated Get-PnPTermGroup to optionally return all termgroups in a TermStore

**2017-01-22**
* Introducing the -Includes parameter. The parameter will allow you (on selected cmdlets) to retrieve values for properties that are not being retrieved by default. The parameter exposes the possible values on pressing tab, and you can specify multiple values. The parameter is available on the following cmdlets: Get-PnPAppInstance, Get-PnPCustomAction, Get-PnPDocumentSetTemplate, Get-PnPEventReceiver, Get-PnPFeature, Ensure-PnPFolder, Get-PnPFolder, Get-PnPList, Get-PnPView, Get-PnPGroup, Get-PnPRecyclyBinItem, Get-PnPSite, Get-PnPTermGroup, Get-PnPWeb.
* Updated the output of a view cmdlets so return table formatted data.

**2017-01-14**
* Added Submit-PnPSearchQuery cmdlet
* Added Set-PnPSiteClosure and Get-PnPSiteClosure cmdlets
* Added Get-PnPContentTypePublishingHubUrl
* Added Get-PnPSiteCollectionTermStore which returns the Site Collection Term Store.

**2017-01-05**
* Added Get-PnPTenantRecyclyBinItem cmdlet to list all items in the tenant scoped recycle bin
* Added -Wait and -LockState properties to Set-PnPTenantSite
* The Tenant cmdlets now report progress if the -Wait parameter is specified (where applicable)

**2017-01-03**
* HAPPY NEW YEAR!
* Added Clear-PnPRecyclyBinItem, Clear-PnPTenantRecyclyBinItem, Get-PnPRecyclyBinItem, Move-PnPRecyclyBinItem, Restore-PnPRecyclyBinItem, Restore-PnPTenantRecyclyBinItem cmdlets
* Added Move-PnPFolder, Rename-PnPFolder cmdlets
* Added Add-PnPPublishingImageRendition, Get-PnPPublishingImageRendition and Remove-PnPPublishingImageRendition cmdlets
* Refactored Get-PnPFile. ServerRelativeUrl and SiteRelativeUrl are now obsolete (but will still work), use the Url parameter instead which takes either a server or site relative url.

**2016-11-21**
* Added support to enable versionining and set the maximum number of versions to keep on a list and library with Set-PnPList
* Updated Add-PnPUserToGroup to allow to send invites to external users

**2016-11-09**
* Added Set-PnPUnifiedGroup cmdlet

**2016-11-01**
* Exposed ResetSubwebsToInherit and UpdateRootwebOnly parameters to Set-PnPTheme.

**2016-10-29**
* Marked Get-SPOSite as deprecated. We will remove this cmdlet in the January 2017 release. Please switch as soon as possible to Get-PnPSite instead. A warning will be shown the moment Get-SPOSite is used.
* Renamed all cmdlet verbs from -SPO* to -PnP*. From now all cmdlets follow the *Verb*-PnP*Noun* pattern. There are corresponding aliases available now that allow existing scripts to continue to work.

**2016-10-19**
* Added Get-SPOProvisioningTemplateFromGallery cmdlet

**2016-10-13**
* Added Get-SPOFolder cmdlet
* Minor update to Set-SPOListItem
* Added attributes to Get-SPOFile
* Added return type to generated documentation for those cmdlets that return an object or value

**2016-10-01**
* Added Load-SPOPRovisioningTemplate
* Added Save-SPOProvisioningTemplate

**2016-09-29**
* Live from MS Ignite: Added Remove-SPOTaxonomyItem cmdlet
* Live from MS Ignite: Added Remove-SPOTermGroup cmdlet

**2016-06-03**
* Added Add-SPODocumentSet cmdlet

**2016-06-02**
* Added Enable-SPOResponsiveUI and Disable-SPOResponsiveUI cmdlets
* Added -CreateDrive parameter to Connect-SPOnline cmdlet, allowing to create a virtual drive into a SharePoint site
* Added Invoke-SPOWebAction cmdlet

**2016-05-09**
* Namespace, Assembly and Project rename from OfficeDevPnP.PowerShell to SharePointPnP.PowerShell

**2016-04-08**
* Added -ExtensibilityHandlers parameter to Get-SPOPRovisioningTemplate

**2016-03-11**
* Added List parameter to Get-SPOContentType, allowing to retrieve the ContentTypes added to a list.

**2016-03-08**
* Added Remove-SPOListItem
* Updated Get-SPOWeb and Get-SPOSubWebs to include ServerRelativeUrl
* Added Ensure-SPOFolder cmdlet

**2016-03-07**
* Added Remove-SPOFieldFromContentType cmdlet
* Added Get-SPOSiteSearchQueryResults cmdlet

**2016-02-04**
* Added -PersistPublishingFiles and -IncludeNativePublishingFiles parameters to Get-SPOProvisioningTemplate

**2016-02-03 **
* Added -ExcludedHandlers attribute to Apply-SPOProvisioningTemplate and Get-SPOPRovisioningTemplate
**2016-02-01**

* Added Convert-SPOProvisioningTemplate cmdlet

**2015-12-26**

* Added -AsIncludeFile parameter to New-SPOProvisioningTemplateFromFolder

**2015-12-21**

* Added a Set-SPOContext cmdlet

**2015-12-14**

* Added Set-SPOListItem cmdlet

**2015-11-21**

* Added, where applicable, Site Relative Url parameters, besides the existing Server Relative Url parameters on cmdlets.
* Implemented the use of PnP Monitored Scope. Turn on the trace log with Set-SPOTraceLog -On -Level Information -LogFile c:\pathtoyourlogfile.log to see the tracelog.
* Added a Get-SPOTheme cmdlet

**2015-10-26**

* Added New-SPOProvisioningTemplateFromFolder cmdlet

**2015-10-14**

* Added optional -Encoding parameter to Export-SPOTaxonomy

**2015-09-23**

* Update Get-SPOSearchConfiguration and Set-SPOSearchConfiguration to support Path parameter to export to or import from a file

**2015-09-21**

* Added -Parameters parameter to Apply-SPOProvisioningTemplate. See help for the cmdlet for more info.
* Renamed PageUrl parameter of web part cmdlets to ServerRelativePageUrl. Set PageUrl as parameter alias to not break existing scripts.

**2015-09-17**

* Added Get-SPOProperty to dynamically load specified properties from objects.

**2015-09-10**

* Renamed Path parameter of Set-SPOHomePage to RootFolderRelativeUrl. Set Path as parameter alias.

**2015-09-02**

* Started adding unit tests
* Added warning when using Install-SPOSolution to documentation. The cmdlet can potentially clear the composed look gallery.

**2015-08-18**

* Added Set-SPOTraceLog cmdlet

**2015-08-15**

* Added -Recurse parameter to Get-SPOSubWebs cmdlet to recursively retrieve all subwebs

**2015-08-14**

* Modified Connect-SPOnline to output version number when specifying -Verbose parameter

**2015-08-10**

* Added Get-SPOWebPartXml cmdlet to export web part XML from a page.

**2015-08-07**

* Added Set-SPOUserProfileProperty (only available for SharePoint Online due to limitations of the On-Premises CSOM SDK)
**2015-07-22**

* Added Remove-SPOGroup cmdlet

**2015-07-14**

* Added additional attribute (-Key) to Get-SPOWebPartProperty cmdlet

**2015-07-13**

* Added additional functionality for connect-sponline in resolving credentials. If no credentials are specified throught the -Credentials parameter, a query is done against the Windows Credentials Manager to retrieve credentials. First is checked for the full URL of the connect request, e.g. https://yourserver/sites/yoursite. If no credential is found for that entry, a query is done for https://yourserver/sites. If no credential is found that entry, a query is done for https://yourserver, if no credential is found for that entry a query is done for 'yourserver'. So:
```
Connect-SPOnline -Url https://yourtenant.sharepoint.com/sites/demosite
``` 
will mean that it will check your credential manager for entries in this order:

```
https://yourtenant.sharepoint.com/sites/demosite
https://yourtenant.sharepoint.com/sites
https://yourtenant.sharepoint.com
yourtenant.sharepoint.com
```

Notice that using
```
Connect-SPOnline -Url https://yourtenant.sharepoint.com/sites/demosite -Credentials <yourlabel>
```
still works as before.

**2015-07-08**

* Added Get-SPOSearchConfiguration and Set-SPOSearchConfiguration cmdlets
* Added support for folder property bags in Set-SPOPropertyBagValue, Get-SPOPropertyBag and Remove-SPOPropertyBagValue. See the help of the cmdlets for more details and examples.

**2015-07-01**

* Added Add-SPOIndexedProperty and Remove-SPOIndexedProperty to allow adding or removing single keys from a set of indexed properties.

**2015-06-29**

* Added OverwriteSystemPropertyBagValues parameter to Apply-SPOProvisioningTemplate cmdlet
* Updated installer to allow for setting advanced properties.

**2015-06-10**

* Changed installers from 64 bit to 32 bit.
* Added ResourceFolder parameter to Apply-SPOProvisioningTemplate cmdlet

**2015-06-03**

* Added OnQuickLaunch parameter to New-SPOList cmdlet

**2015-06-01**

* Added Add-SPOWorkflowDefinition cmdlet
* Updated Add-SPOField to allow for -Field parameter to add a site column to a list.

**2015-05-28**

* Added Set-SPOSitePolicy and Get-SPOSitePolicy cmdlets

**2015-05-22**

* Updated Add-SPOHtlPublishingPageLayout and Add-SPOPublishingPageLayout to support DestinationFolderHierarchy parameter
* Updated Add-SPOFile to create the target folder is not present
* Updated Remove-SPOUserFromGroup to accept either a login name or an email address of a user.

**2015-05-15**

* Updated Set-SPOList to switching if ContentTypes are enabled on the list

**2015-04-24**

* Updated Get-SPOProvisioningTemplate and Apply-SPOProvisioningTemplate to show a progress bar
* Updated GEt-SPOProvisioningTemplate with optional switches to export either Site Collection Term Group (if available) or all Term Groups in the default site collection termstore.
* Added Export-SPOTermGroup cmdlet that supports the provisioning engine XML format
* Added Import-SPOTermGroup cmdlet that supports the provisioning engine XML format

**2015-04-20**

* Admin cmdlets: Get-SPOTenantSite, New-SPOTenantSite, Remove-SPOTenantSite, Set-SPOTenantSite and Get-SPOWebTemplates now automatically switch context. This means that you don't have to connect to https://<tenant>-admin.sharepoint.com first in order to execute them.

**2015-04-08**

* Added Apply-SPOProvisioningTemplate cmdlet
* Added Get-SPOPRovisioningTemplate cmdlet
* Extended Enable-SPOFeature cmdlet to handle Sandboxed features

**2015-03-11**

* Added Get-SPOJavaScript link cmdlet
* Refactored JavaScript related cmdlets to use -Name parameter instead of -Key (-Key still works for backwards compatibility reasons)
* Refactored JavaScript related cmdlets to use -Scope [Web|Site] parameter instead of -FromSite, -SiteScoped and -AddToSite parameters. The old parameters still work for backwards compatibility reasons.
* Fixed an issue in cmdlet help generation where the syntax would not be shown for cmdlets with only one parameter set.

**2015-03-10**

* Added Sequence parameter to Add-SPOJavaScriptLink and Add-SPOJavaScriptBlock cmdlets
* Added Remove-SPOFile cmdlet

**2015-02-25**

* Updated Location parameter in Add-/Remove-SPONavigationNode

**2015-01-07**

* Introduced new Cmdlet: Get-SPOWebPartProperty to return web part properties
* Updated Set-SPOWebPartProperty cmdlet to support int values

**2015-01-02**

* Removed SetAssociatedGroup parameter from new-spogroup cmdlet and moved it to a separate cmdlet: Set-SPOGroup
* Introduced new Cmdlet: Set-SPOGroup to set the group as an associated group and optionally add or remove role assignments
* Introduced new Cmdlet: Set-SPOList to set list properties
* Introduced new Cmdlet: Set-SPOListPermission to set list permissions

**2014-12-30**

* Changed New-SPOWeb to return the actual web as an object instead of a success message.
* Added -SetAssociatedGroup parameter to New-SPOGroup to set a group as a default associated visitors, members or owners group
* Updated New-SPOGroup to allow setting groups as owners

**2014-12-01**

* Added Get-SPOListItem cmdlet to retrieve list items by id, unique id, or CAML. Optionally you can define which fields to load.

**2014-11-05**

* Added Add-SPOFolder cmdlet

**2014-11-04**

* Added Get-SPOIndexedPropertyBagKeys cmdlet
* Updated Set-SPOPropertyBagValue to not remove a property from the indexed properties if it was already in the indexed properties.
* Updated Get-SPOTenantSite output formatting

**2014-11-03**

* Split up Add-SPOField into Add-SPOField and Add-SPOFieldFromXml. The latter only accepts XML input while the first takes parameters to create fields

**2014-10-15**

* Added Add-SPOWorkflowSubscription, Get-SPOWorkflowDefinition, Get-SPOWorkflowSubscription, Remove-SPOWorkflowDefinition, Remove-SPOWorkflowSubscription, Resume-SPOWorkflowInstance, Stop-SPOWorkflowInstance

**2014-10-14**

* Added Get-SPOUserProfileProperty cmdlet
* Added New-SPOPersonalSite cmdlet
* Fixed Get-SPOView cmdlet

**2014-10-08**

* Added Set-SPODefaultColumnValue 

**2014-09-19**

* Removed Url Parameters on Add-SPOFile and made Folder parameter mandatory.

**2014-09-06**

* Added new Set-SPOWeb cmdlet to set Title, SiteLogo, or AlternateCssUrl

**2014-09-03**

* Renamed Add-SPOApp to Import-SPOAppPackage to align with server cmdlet
* Renamed Remove-SPOApp to Uninstall-SPOAppInstance to align with server cmdlet

**2014-08-29**

* Removed OfficeDevPnP.PowerShell.Core project, not in use anymore as all cmdlets now make use of the OfficeDevPnP.Core project.

**2014-08-27**

* Split up Add-SPOWebPart in two cmdlets, Add-SPOWebPartToWikiPage and Add-SPOWebPartToWebPartPage, to reduce confusing parameter sets
* Changed parameters of Add-SPOCustomAction cmdlet
* Changed name of Add-SPONavigationLink to Add-SPONavigationNode, in sync with method name of OfficeDevPnP.Core. Changed parameters of cmdlet.


**2014-08-26**

* Updated several commands to use OfficeDevPnP.Core instead of OfficeDevPnP.PowerShell.Core
* Marked SPOSite and SPOTaxonomy as obsolete. Use OfficeDevPnP.Core extensions instead

**2014-08-23**

* Simplified connection code, added functionality to connect with add-in Id and add-in Secret. 
* Added connection samples in samples folder. 
* Added Get-SPORealm command.

**2014-08-22**

* Namespace change from OfficeDevPnP.SPOnline to OfficeDevPnP.PowerShell
