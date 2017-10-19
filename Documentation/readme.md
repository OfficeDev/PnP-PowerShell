# Cmdlet Documentation #
Below you can find a list of all the available cmdlets. Many commands provide built-in help and examples. Retrieve the detailed help with 

```powershell
Get-Help Connect-PnPOnline -Detailed
```

## Apps
Cmdlet|Description|Platforms
:-----|:----------|:--------
**[Add&#8209;PnPApp](Add-PnPApp.md)** |Add/uploads an available app to the app catalog|SharePoint Online
**[Get&#8209;PnPApp](Get-PnPApp.md)** |Returns the available apps from the app catalog|SharePoint Online
**[Install&#8209;PnPApp](Install-PnPApp.md)** |Installs an available app from the app catalog|SharePoint Online
**[Publish&#8209;PnPApp](Publish-PnPApp.md)** |Publishes/Deploys/Trusts an available app in the app catalog|SharePoint Online
**[Remove&#8209;PnPApp](Remove-PnPApp.md)** |Removes an app from the app catalog|SharePoint Online
**[Uninstall&#8209;PnPApp](Uninstall-PnPApp.md)** |Uninstalls an available add-in from the site|All
**[Unpublish&#8209;PnPApp](Unpublish-PnPApp.md)** |Unpublishes/retracts an available add-in from the app catalog|SharePoint Online
**[Get&#8209;PnPAppInstance](Get-PnPAppInstance.md)** |Returns a SharePoint AddIn Instance|All
**[Uninstall&#8209;PnPAppInstance](Uninstall-PnPAppInstance.md)** |Removes an app from a site|All
**[Import&#8209;PnPAppPackage](Import-PnPAppPackage.md)** |Adds a SharePoint Addin to a site|All
## Base Cmdlets
Cmdlet|Description|Platforms
:-----|:----------|:--------
**[Get&#8209;PnPAppAuthAccessToken](Get-PnPAppAuthAccessToken.md)** |Returns the access token|All
**[Get&#8209;PnPAuthenticationRealm](Get-PnPAuthenticationRealm.md)** |Returns the authentication realm|All
**[Get&#8209;PnPAzureADManifestKeyCredentials](Get-PnPAzureADManifestKeyCredentials.md)** |Return the JSON Manifest snippet for Azure Apps|All
**[Get&#8209;PnPContext](Get-PnPContext.md)** |Returns the current context|All
**[Set&#8209;PnPContext](Set-PnPContext.md)** |Set the ClientContext|All
**[Get&#8209;PnPHealthScore](Get-PnPHealthScore.md)** |Retrieves the healthscore|All
**[Connect&#8209;PnPOnline](Connect-PnPOnline.md)** |Connect to a SharePoint site|All
**[Disconnect&#8209;PnPOnline](Disconnect-PnPOnline.md)** |Disconnects the context|All
**[Get&#8209;PnPProperty](Get-PnPProperty.md)** |Returns a previously not loaded property of a ClientObject|All
**[Execute&#8209;PnPQuery](Execute-PnPQuery.md)** |Execute the current queued actions|All
**[Get&#8209;PnPStoredCredential](Get-PnPStoredCredential.md)** |Get a credential|All
**[Set&#8209;PnPTraceLog](Set-PnPTraceLog.md)** |Turn log tracing on or off|All
## Branding
Cmdlet|Description|Platforms
:-----|:----------|:--------
**[Add&#8209;PnPCustomAction](Add-PnPCustomAction.md)** |Adds a custom action|All
**[Get&#8209;PnPCustomAction](Get-PnPCustomAction.md)** |Return user custom actions|All
**[Remove&#8209;PnPCustomAction](Remove-PnPCustomAction.md)** |Removes a custom action|All
**[Get&#8209;PnPHomePage](Get-PnPHomePage.md)** |Return the homepage|All
**[Set&#8209;PnPHomePage](Set-PnPHomePage.md)** |Sets the home page of the current web.|All
**[Add&#8209;PnPJavaScriptBlock](Add-PnPJavaScriptBlock.md)** |Adds a link to a JavaScript snippet/block to a web or site collection|All
**[Add&#8209;PnPJavaScriptLink](Add-PnPJavaScriptLink.md)** |Adds a link to a JavaScript file to a web or sitecollection|All
**[Get&#8209;PnPJavaScriptLink](Get-PnPJavaScriptLink.md)** |Returns all or a specific custom action(s) with location type ScriptLink|All
**[Remove&#8209;PnPJavaScriptLink](Remove-PnPJavaScriptLink.md)** |Removes a JavaScript link or block from a web or sitecollection|All
**[Get&#8209;PnPMasterPage](Get-PnPMasterPage.md)** |Returns the URLs of the default Master Page and the custom Master Page.|All
**[Set&#8209;PnPMasterPage](Set-PnPMasterPage.md)** |Set the masterpage|All
**[Set&#8209;PnPMinimalDownloadStrategy](Set-PnPMinimalDownloadStrategy.md)** |Activates or deactivates the minimal downloading strategy.|All
**[Add&#8209;PnPNavigationNode](Add-PnPNavigationNode.md)** |Adds an item to a navigation element|All
**[Remove&#8209;PnPNavigationNode](Remove-PnPNavigationNode.md)** |Removes a menu item from either the quicklaunch or top navigation|All
**[Disable&#8209;PnPResponsiveUI](Disable-PnPResponsiveUI.md)** |Deactive the PnP Response UI add-on|All
**[Enable&#8209;PnPResponsiveUI](Enable-PnPResponsiveUI.md)** |Activates the PnP Response UI Add-on|All
**[Get&#8209;PnPTheme](Get-PnPTheme.md)** |Returns the current theme/composed look of the current web.|All
**[Set&#8209;PnPTheme](Set-PnPTheme.md)** |Sets the theme of the current web.|All
## Client-Side Pages
Cmdlet|Description|Platforms
:-----|:----------|:--------
**[Get&#8209;PnPAvailableClientSideComponents](Get-PnPAvailableClientSideComponents.md)** |Gets the available client side components on a particular page|SharePoint Online
**[Add&#8209;PnPClientSidePage](Add-PnPClientSidePage.md)** |Adds a Client-Side Page|SharePoint Online
**[Get&#8209;PnPClientSidePage](Get-PnPClientSidePage.md)** |Gets a Client-Side Page|SharePoint Online
**[Remove&#8209;PnPClientSidePage](Remove-PnPClientSidePage.md)** |Removes a Client-Side Page|SharePoint Online
**[Set&#8209;PnPClientSidePage](Set-PnPClientSidePage.md)** |Sets parameters of a Client-Side Page|SharePoint Online
**[Add&#8209;PnPClientSidePageSection](Add-PnPClientSidePageSection.md)** |Adds a new section to a Client-Side page|SharePoint Online
**[Add&#8209;PnPClientSideText](Add-PnPClientSideText.md)** |Adds a text element to a client-side page.|SharePoint Online
**[Add&#8209;PnPClientSideWebPart](Add-PnPClientSideWebPart.md)** |Adds a Client-Side Web Part to a client-side page|SharePoint Online
## Content Types
Cmdlet|Description|Platforms
:-----|:----------|:--------
**[Add&#8209;PnPContentType](Add-PnPContentType.md)** |Adds a new content type|All
**[Get&#8209;PnPContentType](Get-PnPContentType.md)** |Retrieves a content type|All
**[Remove&#8209;PnPContentType](Remove-PnPContentType.md)** |Removes a content type from a web|All
**[Remove&#8209;PnPContentTypeFromList](Remove-PnPContentTypeFromList.md)** |Removes a content type from a list|All
**[Get&#8209;PnPContentTypePublishingHubUrl](Get-PnPContentTypePublishingHubUrl.md)** |Returns the url to Content Type Publishing Hub|All
**[Add&#8209;PnPContentTypeToList](Add-PnPContentTypeToList.md)** |Adds a new content type to a list|All
**[Set&#8209;PnPDefaultContentTypeToList](Set-PnPDefaultContentTypeToList.md)** |Sets the default content type for a list|All
**[Remove&#8209;PnPFieldFromContentType](Remove-PnPFieldFromContentType.md)** |Removes a site column from a content type|All
**[Add&#8209;PnPFieldToContentType](Add-PnPFieldToContentType.md)** |Adds an existing site column to a content type|All
## Document Sets
Cmdlet|Description|Platforms
:-----|:----------|:--------
**[Remove&#8209;PnPContentTypeFromDocumentSet](Remove-PnPContentTypeFromDocumentSet.md)** |Removes a content type from a document set|All
**[Add&#8209;PnPContentTypeToDocumentSet](Add-PnPContentTypeToDocumentSet.md)** |Adds a content type to a document set|All
**[Add&#8209;PnPDocumentSet](Add-PnPDocumentSet.md)** |Creates a new document set in a library.|All
**[Set&#8209;PnPDocumentSetField](Set-PnPDocumentSetField.md)** |Sets a site column from the available content types to a document set|All
**[Get&#8209;PnPDocumentSetTemplate](Get-PnPDocumentSetTemplate.md)** |Retrieves a document set template|All
## Event Receivers
Cmdlet|Description|Platforms
:-----|:----------|:--------
**[Add&#8209;PnPEventReceiver](Add-PnPEventReceiver.md)** |Adds a new event receiver|All
**[Get&#8209;PnPEventReceiver](Get-PnPEventReceiver.md)** |Return registered eventreceivers|All
**[Remove&#8209;PnPEventReceiver](Remove-PnPEventReceiver.md)** |Remove an eventreceiver|All
## Features
Cmdlet|Description|Platforms
:-----|:----------|:--------
**[New&#8209;PnPExtensbilityHandlerObject](New-PnPExtensbilityHandlerObject.md)** |Creates an ExtensibilityHandler Object, to be used by the Get-SPOProvisioningTemplate cmdlet|All
**[Disable&#8209;PnPFeature](Disable-PnPFeature.md)** |Disables a feature|All
**[Enable&#8209;PnPFeature](Enable-PnPFeature.md)** |Enables a feature|All
**[Get&#8209;PnPFeature](Get-PnPFeature.md)** |Returns all activated or a specific activated feature|All
## Fields
Cmdlet|Description|Platforms
:-----|:----------|:--------
**[Add&#8209;PnPField](Add-PnPField.md)** |Add a field|All
**[Get&#8209;PnPField](Get-PnPField.md)** |Returns a field from a list or site|All
**[Remove&#8209;PnPField](Remove-PnPField.md)** |Removes a field from a list or a site|All
**[Set&#8209;PnPField](Set-PnPField.md)** |Changes one or more properties of a field in a specific list or for the whole web|All
**[Add&#8209;PnPFieldFromXml](Add-PnPFieldFromXml.md)** |Adds a field to a list or as a site column based upon a CAML/XML field definition|All
**[Add&#8209;PnPTaxonomyField](Add-PnPTaxonomyField.md)** |Add a taxonomy field|All
**[Set&#8209;PnPView](Set-PnPView.md)** |Change view properties|All
## Files and Folders
Cmdlet|Description|Platforms
:-----|:----------|:--------
**[Add&#8209;PnPFile](Add-PnPFile.md)** |Uploads a file to Web|All
**[Copy&#8209;PnPFile](Copy-PnPFile.md)** |Copies a file or folder to a different location|All
**[Find&#8209;PnPFile](Find-PnPFile.md)** |Finds a file in the virtual file system of the web.|All
**[Get&#8209;PnPFile](Get-PnPFile.md)** |Downloads a file.|All
**[Move&#8209;PnPFile](Move-PnPFile.md)** |Moves a file to a different location|All
**[Remove&#8209;PnPFile](Remove-PnPFile.md)** |Removes a file.|All
**[Rename&#8209;PnPFile](Rename-PnPFile.md)** |Renames a file in its current location|All
**[Set&#8209;PnPFileCheckedIn](Set-PnPFileCheckedIn.md)** |Checks in a file|All
**[Set&#8209;PnPFileCheckedOut](Set-PnPFileCheckedOut.md)** |Checks out a file|All
**[Add&#8209;PnPFolder](Add-PnPFolder.md)** |Creates a folder within a parent folder|All
**[Ensure&#8209;PnPFolder](Ensure-PnPFolder.md)** |Returns a folder from a given site relative path, and will create it if it does not exist.|All
**[Get&#8209;PnPFolder](Get-PnPFolder.md)** |Return a folder object|All
**[Move&#8209;PnPFolder](Move-PnPFolder.md)** |Move a folder to another location in the current web|All
**[Remove&#8209;PnPFolder](Remove-PnPFolder.md)** |Deletes a folder within a parent folder|All
**[Rename&#8209;PnPFolder](Rename-PnPFolder.md)** |Renames a folder|All
**[Get&#8209;PnPFolderItem](Get-PnPFolderItem.md)** |List content in folder|All
**[Copy&#8209;PnPItemProxy](Copy-PnPItemProxy.md)** |Proxy cmdlet for using Copy-Item between SharePoint provider and FileSystem provider|All
**[Move&#8209;PnPItemProxy](Move-PnPItemProxy.md)** |Proxy cmdlet for using Move-Item between SharePoint provider and FileSystem provider|All
## Information Management
Cmdlet|Description|Platforms
:-----|:----------|:--------
**[Get&#8209;PnPSiteClosure](Get-PnPSiteClosure.md)** |Get the site closure status of the site which has a site policy applied|All
**[Set&#8209;PnPSiteClosure](Set-PnPSiteClosure.md)** |Opens or closes a site which has a site policy applied|All
**[Set&#8209;PnPSitePolicy](Set-PnPSitePolicy.md)** |Sets a site policy|All
**[Get&#8209;PnPSitePolicy](Get-PnPSitePolicy.md)** |Retrieves all or a specific site policy|All
## Lists
Cmdlet|Description|Platforms
:-----|:----------|:--------
**[Get&#8209;PnPDefaultColumnValues](Get-PnPDefaultColumnValues.md)** |Gets the default column values for all folders in document library|All
**[Set&#8209;PnPDefaultColumnValues](Set-PnPDefaultColumnValues.md)** |Sets default column values for a document library|All
**[Get&#8209;PnPList](Get-PnPList.md)** |Returns a List object|All
**[New&#8209;PnPList](New-PnPList.md)** |Creates a new list|All
**[Remove&#8209;PnPList](Remove-PnPList.md)** |Deletes a list|All
**[Set&#8209;PnPList](Set-PnPList.md)** |Updates list settings|All
**[Add&#8209;PnPListItem](Add-PnPListItem.md)** |Adds an item to a list|All
**[Get&#8209;PnPListItem](Get-PnPListItem.md)** |Retrieves list items|All
**[Remove&#8209;PnPListItem](Remove-PnPListItem.md)** |Deletes an item from a list|All
**[Set&#8209;PnPListItem](Set-PnPListItem.md)** |Updates a list item|All
**[Set&#8209;PnPListItemPermission](Set-PnPListItemPermission.md)** |Sets list item permissions|All
**[Move&#8209;PnPListItemToRecycleBin](Move-PnPListItemToRecycleBin.md)** |Moves an item from a list to the Recycle Bin|All
**[Set&#8209;PnPListPermission](Set-PnPListPermission.md)** |Sets list permissions|All
**[Get&#8209;PnPProvisioningTemplateFromGallery](Get-PnPProvisioningTemplateFromGallery.md)** |Retrieves or searches provisioning templates from the PnP Template Gallery|All
**[Request&#8209;PnPReIndexList](Request-PnPReIndexList.md)** |Marks the list for full indexing during the next incremental crawl|All
**[Add&#8209;PnPView](Add-PnPView.md)** |Adds a view to a list|All
**[Get&#8209;PnPView](Get-PnPView.md)** |Returns one or all views from a list|All
**[Remove&#8209;PnPView](Remove-PnPView.md)** |Deletes a view from a list|All
## Microsoft Graph
Cmdlet|Description|Platforms
:-----|:----------|:--------
**[Connect&#8209;PnPMicrosoftGraph](Connect-PnPMicrosoftGraph.md)** |Connect to the Microsoft Graph|All
**[Get&#8209;PnPUnifiedGroup](Get-PnPUnifiedGroup.md)** |Gets one Office 365 Group (aka Unified Group) or a list of Office 365 Groups|All
**[New&#8209;PnPUnifiedGroup](New-PnPUnifiedGroup.md)** |Creates a new Office 365 Group (aka Unified Group)|All
**[Remove&#8209;PnPUnifiedGroup](Remove-PnPUnifiedGroup.md)** |Removes one Office 365 Group (aka Unified Group) or a list of Office 365 Groups|All
**[Set&#8209;PnPUnifiedGroup](Set-PnPUnifiedGroup.md)** |Sets Office 365 Group (aka Unified Group) properties|All
## Provisioning
Cmdlet|Description|Platforms
:-----|:----------|:--------
**[Add&#8209;PnPDataRowsToProvisioningTemplate](Add-PnPDataRowsToProvisioningTemplate.md)** |Adds datarows to a list inside a PnP Provisioning Template|All
**[Remove&#8209;PnPFileFromProvisioningTemplate](Remove-PnPFileFromProvisioningTemplate.md)** |Removes a file from a PnP Provisioning Template|All
**[Add&#8209;PnPFileToProvisioningTemplate](Add-PnPFileToProvisioningTemplate.md)** |Adds a file to a PnP Provisioning Template|All
**[Convert&#8209;PnPFolderToProvisioningTemplate](Convert-PnPFolderToProvisioningTemplate.md)** |Creates a pnp package file of an existing template xml, and includes all files in the current folder|All
**[Add&#8209;PnPListFoldersToProvisioningTemplate](Add-PnPListFoldersToProvisioningTemplate.md)** |Adds folders to a list in a PnP Provisioning Template|All
**[Apply&#8209;PnPProvisioningTemplate](Apply-PnPProvisioningTemplate.md)** |Applies a provisioning template to a web|All
**[Convert&#8209;PnPProvisioningTemplate](Convert-PnPProvisioningTemplate.md)** |Converts a provisioning template to an other schema version|All
**[Get&#8209;PnPProvisioningTemplate](Get-PnPProvisioningTemplate.md)** |Generates a provisioning template from a web|All
**[Load&#8209;PnPProvisioningTemplate](Load-PnPProvisioningTemplate.md)** |Loads a PnP file from the file systems|All
**[New&#8209;PnPProvisioningTemplate](New-PnPProvisioningTemplate.md)** |Creates a new provisioning template object|All
**[Save&#8209;PnPProvisioningTemplate](Save-PnPProvisioningTemplate.md)** |Saves a PnP file to the file systems|All
**[New&#8209;PnPProvisioningTemplateFromFolder](New-PnPProvisioningTemplateFromFolder.md)** |Generates a provisioning template from a given folder, including only files that are present in that folder|All
**[Set&#8209;PnPProvisioningTemplateMetadata](Set-PnPProvisioningTemplateMetadata.md)** |Sets metadata of a provisioning template|All
## Publishing
Cmdlet|Description|Platforms
:-----|:----------|:--------
**[Set&#8209;PnPAvailablePageLayouts](Set-PnPAvailablePageLayouts.md)** |Sets the available page layouts for the current site|All
**[Set&#8209;PnPDefaultPageLayout](Set-PnPDefaultPageLayout.md)** |Sets a specific page layout to be the default page layout for a publishing site|All
**[Add&#8209;PnPHtmlPublishingPageLayout](Add-PnPHtmlPublishingPageLayout.md)** |Adds a HTML based publishing page layout|All
**[Add&#8209;PnPMasterPage](Add-PnPMasterPage.md)** |Adds a Masterpage|All
**[Add&#8209;PnPPublishingImageRendition](Add-PnPPublishingImageRendition.md)** |Adds an Image Rendition if the Name of the Image Rendition does not already exist. This prevents creating two Image Renditions that share the same name.|All
**[Get&#8209;PnPPublishingImageRendition](Get-PnPPublishingImageRendition.md)** |Returns all image renditions or if Identity is specified a specific one|All
**[Remove&#8209;PnPPublishingImageRendition](Remove-PnPPublishingImageRendition.md)** |Removes an existing image rendition|All
**[Add&#8209;PnPPublishingPage](Add-PnPPublishingPage.md)** |Adds a publishing page|All
**[Add&#8209;PnPPublishingPageLayout](Add-PnPPublishingPageLayout.md)** |Adds a publishing page layout|All
**[Add&#8209;PnPWikiPage](Add-PnPWikiPage.md)** |Adds a wiki page|All
**[Remove&#8209;PnPWikiPage](Remove-PnPWikiPage.md)** |Removes a wiki page|All
**[Get&#8209;PnPWikiPageContent](Get-PnPWikiPageContent.md)** |Gets the contents/source of a wiki page|All
**[Set&#8209;PnPWikiPageContent](Set-PnPWikiPageContent.md)** |Sets the contents of a wikipage|All
## Records Management
Cmdlet|Description|Platforms
:-----|:----------|:--------
**[Set&#8209;PnPInPlaceRecordsManagement](Set-PnPInPlaceRecordsManagement.md)** |Activates or deactivates in the place records management feature.|All
**[Disable&#8209;PnPInPlaceRecordsManagementForSite](Disable-PnPInPlaceRecordsManagementForSite.md)** |Disables in place records management for a site.|All
**[Enable&#8209;PnPInPlaceRecordsManagementForSite](Enable-PnPInPlaceRecordsManagementForSite.md)** |Enables in place records management for a site.|All
**[Clear&#8209;PnPListItemAsRecord](Clear-PnPListItemAsRecord.md)** |Undeclares a list item as a record|SharePoint Online
**[Set&#8209;PnPListItemAsRecord](Set-PnPListItemAsRecord.md)** |Declares a list item as a record|SharePoint Online
**[Test&#8209;PnPListItemIsRecord](Test-PnPListItemIsRecord.md)** |Checks if a list item is a record|SharePoint Online
## Search
Cmdlet|Description|Platforms
:-----|:----------|:--------
**[Get&#8209;PnPSearchConfiguration](Get-PnPSearchConfiguration.md)** |Returns the search configuration|All
**[Set&#8209;PnPSearchConfiguration](Set-PnPSearchConfiguration.md)** |Sets the search configuration|All
**[Submit&#8209;PnPSearchQuery](Submit-PnPSearchQuery.md)** |Executes an arbitrary search query against the SharePoint search index|All
**[Get&#8209;PnPSiteSearchQueryResults](Get-PnPSiteSearchQueryResults.md)** |Executes a search query to retrieve indexed site collections|All
## SharePoint Recycle Bin
Cmdlet|Description|Platforms
:-----|:----------|:--------
**[Clear&#8209;PnpRecycleBinItem](Clear-PnpRecycleBinItem.md)** |Permanently deletes all or a specific recycle bin item|All
**[Move&#8209;PnpRecycleBinItem](Move-PnpRecycleBinItem.md)** |Moves all items or a specific item in the first stage recycle bin of the current site collection to the second stage recycle bin|SharePoint Online
**[Restore&#8209;PnpRecycleBinItem](Restore-PnpRecycleBinItem.md)** |Restores the provided recycle bin item to its original location|All
**[Get&#8209;PnPRecycleBinItem](Get-PnPRecycleBinItem.md)** |Returns the items in the recycle bin from the context|All
**[Get&#8209;PnPTenantRecycleBinItem](Get-PnPTenantRecycleBinItem.md)** |Returns the items in the tenant scoped recycle bin|SharePoint Online
## SharePoint WebHooks
Cmdlet|Description|Platforms
:-----|:----------|:--------
**[Add&#8209;PnPWebhookSubscription](Add-PnPWebhookSubscription.md)** |Adds a new Webhook subscription|SharePoint Online
**[Remove&#8209;PnPWebhookSubscription](Remove-PnPWebhookSubscription.md)** |Removes a Webhook subscription from the resource|SharePoint Online
**[Set&#8209;PnPWebhookSubscription](Set-PnPWebhookSubscription.md)** |Updates a Webhook subscription|SharePoint Online
**[Get&#8209;PnPWebhookSubscriptions](Get-PnPWebhookSubscriptions.md)** |Gets all the Webhook subscriptions of the resource|SharePoint Online
## Sites
Cmdlet|Description|Platforms
:-----|:----------|:--------
**[Set&#8209;PnPAppSideLoading](Set-PnPAppSideLoading.md)** |Enables the App SideLoading Feature on a site|All
**[Get&#8209;PnPAuditing](Get-PnPAuditing.md)** |Get the Auditing setting of a site|All
**[Set&#8209;PnPAuditing](Set-PnPAuditing.md)** |Set Auditing setting for a site|All
**[Get&#8209;PnPSite](Get-PnPSite.md)** |Returns the current site collection from the context.|All
**[Add&#8209;PnPSiteCollectionAdmin](Add-PnPSiteCollectionAdmin.md)** |Adds one or more users as site collection administrators to the site collection in the current context|All
**[Get&#8209;PnPSiteCollectionAdmin](Get-PnPSiteCollectionAdmin.md)** |Returns the current site collection administrators of the site colleciton in the current context|All
**[Remove&#8209;PnPSiteCollectionAdmin](Remove-PnPSiteCollectionAdmin.md)** |Removes one or more users as site collection administrators from the site collection in the current context|All
**[Install&#8209;PnPSolution](Install-PnPSolution.md)** |Installs a sandboxed solution to a site collection. WARNING! This method can delete your composed look gallery due to the method used to activate the solution. We recommend you to only to use this cmdlet if you are okay with that.|All
**[Uninstall&#8209;PnPSolution](Uninstall-PnPSolution.md)** |Uninstalls a sandboxed solution from a site collection|All
## Taxonomy
Cmdlet|Description|Platforms
:-----|:----------|:--------
**[Get&#8209;PnPSiteCollectionTermStore](Get-PnPSiteCollectionTermStore.md)** |Returns the site collection term store|All
**[Export&#8209;PnPTaxonomy](Export-PnPTaxonomy.md)** |Exports a taxonomy to either the output or to a file.|All
**[Import&#8209;PnPTaxonomy](Import-PnPTaxonomy.md)** |Imports a taxonomy from either a string array or a file|All
**[Set&#8209;PnPTaxonomyFieldValue](Set-PnPTaxonomyFieldValue.md)** |Sets a taxonomy term value in a listitem field|All
**[Get&#8209;PnPTaxonomyItem](Get-PnPTaxonomyItem.md)** |Returns a taxonomy item|All
**[Remove&#8209;PnPTaxonomyItem](Remove-PnPTaxonomyItem.md)** |Removes a taxonomy item|All
**[Get&#8209;PnPTaxonomySession](Get-PnPTaxonomySession.md)** |Returns a taxonomy session|All
**[Get&#8209;PnPTerm](Get-PnPTerm.md)** |Returns a taxonomy term|All
**[New&#8209;PnPTerm](New-PnPTerm.md)** |Creates a taxonomy term|All
**[Get&#8209;PnPTermGroup](Get-PnPTermGroup.md)** |Returns a taxonomy term group|All
**[New&#8209;PnPTermGroup](New-PnPTermGroup.md)** |Creates a taxonomy term group|All
**[Remove&#8209;PnPTermGroup](Remove-PnPTermGroup.md)** |Removes a taxonomy term group and all its containing termsets|All
**[Import&#8209;PnPTermGroupFromXml](Import-PnPTermGroupFromXml.md)** |Imports a taxonomy TermGroup from either the input or from an XML file.|All
**[Export&#8209;PnPTermGroupToXml](Export-PnPTermGroupToXml.md)** |Exports a taxonomy TermGroup to either the output or to an XML file.|All
**[Get&#8209;PnPTermSet](Get-PnPTermSet.md)** |Returns a taxonomy term set|All
**[Import&#8209;PnPTermSet](Import-PnPTermSet.md)** |Imports a taxonomy term set from a file in the standard format.|All
**[New&#8209;PnPTermSet](New-PnPTermSet.md)** |Creates a taxonomy term set|All
## Tenant Administration
Cmdlet|Description|Platforms
:-----|:----------|:--------
**[Get&#8209;PnPAccessToken](Get-PnPAccessToken.md)** |Returns the current OAuth Access token|All
**[New&#8209;PnPSite](New-PnPSite.md)** |BETA: This cmdlet is using early release APIs. Notice that functionality and parameters can change. Creates a new site collection|SharePoint Online
**[Clear&#8209;PnPTenantRecycleBinItem](Clear-PnPTenantRecycleBinItem.md)** |Permanently deletes a site collection from the tenant scoped recycle bin|All
**[Restore&#8209;PnPTenantRecycleBinItem](Restore-PnPTenantRecycleBinItem.md)** |Restores a site collection from the tenant scoped recycle bin|SharePoint Online
**[Get&#8209;PnPTenantSite](Get-PnPTenantSite.md)** |Retrieve site information.|SharePoint Online
**[New&#8209;PnPTenantSite](New-PnPTenantSite.md)** |Creates a new site collection for the current tenant|All
**[Remove&#8209;PnPTenantSite](Remove-PnPTenantSite.md)** |Removes a site collection|SharePoint Online
**[Set&#8209;PnPTenantSite](Set-PnPTenantSite.md)** |Set site information.|SharePoint Online
**[Get&#8209;PnPTimeZoneId](Get-PnPTimeZoneId.md)** |Returns a time zone ID|All
**[Get&#8209;PnPWebTemplates](Get-PnPWebTemplates.md)** |Returns the available web templates.|SharePoint Online
## User and group management
Cmdlet|Description|Platforms
:-----|:----------|:--------
**[Get&#8209;PnPGroup](Get-PnPGroup.md)** |Returns a specific group or all groups.|All
**[New&#8209;PnPGroup](New-PnPGroup.md)** |Adds group to the Site Groups List and returns a group object|All
**[Remove&#8209;PnPGroup](Remove-PnPGroup.md)** |Removes a group from a web.|All
**[Set&#8209;PnPGroup](Set-PnPGroup.md)** |Updates a group|All
**[Get&#8209;PnPGroupPermissions](Get-PnPGroupPermissions.md)** |Returns the permissions for a specific SharePoint group|All
**[Set&#8209;PnPGroupPermissions](Set-PnPGroupPermissions.md)** |Adds and/or removes permissions of a specific SharePoint group|All
**[Get&#8209;PnPUser](Get-PnPUser.md)** |Returns site users of current web|All
**[New&#8209;PnPUser](New-PnPUser.md)** |Adds a user to the built-in Site User Info List and returns a user object|All
**[Remove&#8209;PnPUser](Remove-PnPUser.md)** |Removes a specific user from the site collection User Information List|All
**[Remove&#8209;PnPUserFromGroup](Remove-PnPUserFromGroup.md)** |Removes a user from a group|All
**[Add&#8209;PnPUserToGroup](Add-PnPUserToGroup.md)** |Adds a user to a group|All
## User Profiles
Cmdlet|Description|Platforms
:-----|:----------|:--------
**[New&#8209;PnPPersonalSite](New-PnPPersonalSite.md)** |Office365 only: Creates a personal / OneDrive For Business site|SharePoint Online
**[Get&#8209;PnPUserProfileProperty](Get-PnPUserProfileProperty.md)** |You must connect to the tenant admin website (https://:<tenant>-admin.sharepoint.com) with Connect-PnPOnline in order to use this cmdlet.  |All
**[Set&#8209;PnPUserProfileProperty](Set-PnPUserProfileProperty.md)** |Office365 only: Uses the tenant API to retrieve site information.  You must connect to the tenant admin website (https://:<tenant>-admin.sharepoint.com) with Connect-PnPOnline in order to use this command.  |SharePoint Online
## Utilities
Cmdlet|Description|Platforms
:-----|:----------|:--------
**[Send&#8209;PnPMail](Send-PnPMail.md)** |Sends an email using the Office 365 SMTP Service or SharePoint, depending on the parameters specified. See detailed help for more information.|All
## Web Parts
Cmdlet|Description|Platforms
:-----|:----------|:--------
**[Get&#8209;PnPWebPart](Get-PnPWebPart.md)** |Returns a webpart definition object|All
**[Remove&#8209;PnPWebPart](Remove-PnPWebPart.md)** |Removes a webpart from a page|All
**[Get&#8209;PnPWebPartProperty](Get-PnPWebPartProperty.md)** |Returns a web part property|All
**[Set&#8209;PnPWebPartProperty](Set-PnPWebPartProperty.md)** |Sets a web part property|All
**[Add&#8209;PnPWebPartToWebPartPage](Add-PnPWebPartToWebPartPage.md)** |Adds a webpart to a web part page in a specified zone|All
**[Add&#8209;PnPWebPartToWikiPage](Add-PnPWebPartToWikiPage.md)** |Adds a webpart to a wiki page in a specified table row and column|All
**[Get&#8209;PnPWebPartXml](Get-PnPWebPartXml.md)** |Returns the webpart XML of a webpart registered on a site|All
## Webs
Cmdlet|Description|Platforms
:-----|:----------|:--------
**[Set&#8209;PnPIndexedProperties](Set-PnPIndexedProperties.md)** |Marks values of the propertybag to be indexed by search. Notice that this will overwrite the existing flags, i.e. only the properties you define with the cmdlet will be indexed.|All
**[Add&#8209;PnPIndexedProperty](Add-PnPIndexedProperty.md)** |Marks the value of the propertybag key specified to be indexed by search.|All
**[Remove&#8209;PnPIndexedProperty](Remove-PnPIndexedProperty.md)** |Removes a key from propertybag to be indexed by search. The key and it's value remain in the propertybag, however it will not be indexed anymore.|All
**[Get&#8209;PnPIndexedPropertyKeys](Get-PnPIndexedPropertyKeys.md)** |Returns the keys of the property bag values that have been marked for indexing by search|All
**[Get&#8209;PnPPropertyBag](Get-PnPPropertyBag.md)** |Returns the property bag values.|All
**[Remove&#8209;PnPPropertyBagValue](Remove-PnPPropertyBagValue.md)** |Removes a value from the property bag|All
**[Set&#8209;PnPPropertyBagValue](Set-PnPPropertyBagValue.md)** |Sets a property bag value|All
**[Request&#8209;PnPReIndexWeb](Request-PnPReIndexWeb.md)** |Marks the web for full indexing during the next incremental crawl|All
**[Get&#8209;PnPRequestAccessEmails](Get-PnPRequestAccessEmails.md)** |Returns the request access e-mail addresses|SharePoint Online
**[Set&#8209;PnPRequestAccessEmails](Set-PnPRequestAccessEmails.md)** |Sets Request Access Emails on a web|SharePoint Online
**[Get&#8209;PnPSubWebs](Get-PnPSubWebs.md)** |Returns the subwebs of the current web|All
**[Get&#8209;PnPWeb](Get-PnPWeb.md)** |Returns the current web object|All
**[New&#8209;PnPWeb](New-PnPWeb.md)** |Creates a new subweb under the current web|All
**[Remove&#8209;PnPWeb](Remove-PnPWeb.md)** |Removes a subweb in the current web|All
**[Set&#8209;PnPWeb](Set-PnPWeb.md)** |Sets properties on a web|All
**[Invoke&#8209;PnPWebAction](Invoke-PnPWebAction.md)** |Executes operations on web, lists and list items.|All
**[Set&#8209;PnPWebPermission](Set-PnPWebPermission.md)** |Set permissions|All
## Workflows
Cmdlet|Description|Platforms
:-----|:----------|:--------
**[Add&#8209;PnPWorkflowDefinition](Add-PnPWorkflowDefinition.md)** |Adds a workflow definition|All
**[Get&#8209;PnPWorkflowDefinition](Get-PnPWorkflowDefinition.md)** |Return a workflow definition|All
**[Remove&#8209;PnPWorkflowDefinition](Remove-PnPWorkflowDefinition.md)** |Removes a workflow definition|All
**[Resume&#8209;PnPWorkflowInstance](Resume-PnPWorkflowInstance.md)** |Resume a workflow|All
**[Stop&#8209;PnPWorkflowInstance](Stop-PnPWorkflowInstance.md)** |Stops a workflow instance|All
**[Add&#8209;PnPWorkflowSubscription](Add-PnPWorkflowSubscription.md)** |Adds a workflow subscription to a list|All
**[Get&#8209;PnPWorkflowSubscription](Get-PnPWorkflowSubscription.md)** |Return a workflow subscription|All
**[Remove&#8209;PnPWorkflowSubscription](Remove-PnPWorkflowSubscription.md)** |Remove workflow subscription|All
