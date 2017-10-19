# PnP PowerShell overview

SharePoint Patterns and Practices (PnP) contains a library of PowerShell commands (PnP PowerShell) that allows you to perform complex provisioning and artifact management actions towards SharePoint. The commands use CSOM and can work against both SharePoint Online as SharePoint On-Premises.

![SharePoint Patterns and Practices](https://devofficecdn.azureedge.net/media/Default/PnP/sppnp.png)

_**Applies to:** SharePoint Online | SharePoint 2016 | SharePoint 2013_

## Installation #

There are 3 ways to install the cmdlets. We recommend, where possible, to install them from the [PowerShell Gallery](https://www.powershellgallery.com). Alternatively you can download the setup files or run a PowerShell script to download the PowerShellGet module and install the cmdlets subsequently.

### PowerShell Gallery ###
If you main OS is Windows 10, of if you have [PowerShellGet](https://github.com/powershell/powershellget) installed, you can run the following commands to install the PowerShell cmdlets:

|**SharePoint Version**|**Command to install**|
|------------------|------------------|
|SharePoint Online|```Install-Module SharePointPnPPowerShellOnline ```|
|SharePoint 2016|```Install-Module SharePointPnPPowerShell2016```|
|SharePoint 2013|```Install-Module SharePointPnPPowerShell2013```|

*Notice*: if you install the latest PowerShellGet from Github, you might receive an error message stating 
>PackageManagement\Install-Package : The version 'x.x.x.x' of the module 'SharePointPnPPowerShellOnline' being installed is not catalog signed.

In order to install the cmdlets when you get this error specify the -SkipPublisherCheck switch with the Install-Module cmdlet, e.g. ```Install-Module SharePointPnPPowerShellOnline -SkipPublisherCheck -AllowClobber```

### Setup files ##
You can download setup files from the [releases](https://github.com/officedev/pnp-powershell/releases) section of the PnP PowerShell repository. These files will up be updated on a monthly basis. Run the install and restart any open instances of PowerShell to use the cmdlets.

### Installation script ##
This is an alternative for installation on machines that have at least PowerShell v3 installed. You can find the version of PowerShell  by opening PowerShell and running ```$PSVersionTable.PSVersion```. The value for ```Major``` should be above 3.

To install the cmdlets you can run the below command which will install PowerShell Package Management and then install the PowerShell Modules from the PowerShell Gallery.

```powershell
Invoke-Expression (New-Object Net.WebClient).DownloadString('https://raw.githubusercontent.com/OfficeDev/PnP-PowerShell/master/Samples/Modules.Install/Install-SharePointPnPPowerShell.ps1')
```

## Updating ##
Every month a new release will be made available of the PnP PowerShell Cmdlets. If you earlier installed the cmdlets using the setup file, simply download the [latest version](https://github.com/SharePoint/PnP-PowerShell/releases/latest) and run the setup. This will update your existing installation.

If you have installed the cmdlets using PowerShellGet with ```Install-Module``` from the PowerShell Gallery then you will be able to use the following command to install the latest updated version:

```powershell
Update-Module SharePointPnPPowerShell*
``` 

This will automatically load the module after starting PowerShell 3.0.

You can check the installed PnP-PowerShell versions with the following command:

```powershell
Get-Module SharePointPnPPowerShell* -ListAvailable | Select-Object Name,Version | Sort-Object Version -Descending
```

## Getting Started #

To use the library you first need to connect to your tenant:

```powershell
Connect-PnPOnline –Url https://yoursite.sharepoint.com –Credentials (Get-Credential)
```

To view all cmdlets, enter

```powershell
Get-Command -Module *PnP*
```

At the following links you will find a few videos on how to get started with the cmdlets:

* https://channel9.msdn.com/blogs/OfficeDevPnP/PnP-Web-Cast-Introduction-to-Office-365-PnP-PowerShell
* https://channel9.msdn.com/blogs/OfficeDevPnP/Introduction-to-PnP-PowerShell-Cmdlets
* https://channel9.msdn.com/blogs/OfficeDevPnP/PnP-Webcast-PnP-PowerShell-Getting-started-with-latest-updates

### Setting up credentials ##
See this [wiki page](https://github.com/OfficeDev/PnP-PowerShell/wiki/How-to-use-the-Windows-Credential-Manager-to-ease-authentication-with-PnP-PowerShell) for more information on how to use the Windows Credential Manager to setup credentials that you can use in unattended scripts

## Cmdlet overview


### Apps 
Cmdlet|Description|Platform
:-----|:----------|:-------
**[Add&#8209;PnPApp](AddPnPApp.md)** |Add/uploads an available app to the app catalog|SharePoint Online
**[Get&#8209;PnPApp](GetPnPApp.md)** |Returns the available apps from the app catalog|SharePoint Online
**[Install&#8209;PnPApp](InstallPnPApp.md)** |Installs an available app from the app catalog|SharePoint Online
**[Publish&#8209;PnPApp](PublishPnPApp.md)** |Publishes/Deploys/Trusts an available app in the app catalog|SharePoint Online
**[Remove&#8209;PnPApp](RemovePnPApp.md)** |Removes an app from the app catalog|SharePoint Online
**[Uninstall&#8209;PnPApp](UninstallPnPApp.md)** |Uninstalls an available add-in from the site|All
**[Unpublish&#8209;PnPApp](UnpublishPnPApp.md)** |Unpublishes/retracts an available add-in from the app catalog|SharePoint Online
**[Get&#8209;PnPAppInstance](GetPnPAppInstance.md)** |Returns a SharePoint AddIn Instance|All
**[Uninstall&#8209;PnPAppInstance](UninstallPnPAppInstance.md)** |Removes an app from a site|All
**[Import&#8209;PnPAppPackage](ImportPnPAppPackage.md)** |Adds a SharePoint Addin to a site|All


### Base Cmdlets 
Cmdlet|Description|Platform
:-----|:----------|:-------
**[Get&#8209;PnPAppAuthAccessToken](GetPnPAppAuthAccessToken.md)** |Returns the access token|All
**[Get&#8209;PnPAuthenticationRealm](GetPnPAuthenticationRealm.md)** |Returns the authentication realm|All
**[Get&#8209;PnPAzureADManifestKeyCredentials](GetPnPAzureADManifestKeyCredentials.md)** |Return the JSON Manifest snippet for Azure Apps|All
**[Get&#8209;PnPContext](GetPnPContext.md)** |Returns the current context|All
**[Set&#8209;PnPContext](SetPnPContext.md)** |Set the ClientContext|All
**[Get&#8209;PnPHealthScore](GetPnPHealthScore.md)** |Retrieves the healthscore|All
**[Connect&#8209;PnPOnline](ConnectPnPOnline.md)** |Connect to a SharePoint site|All
**[Disconnect&#8209;PnPOnline](DisconnectPnPOnline.md)** |Disconnects the context|All
**[Get&#8209;PnPProperty](GetPnPProperty.md)** |Returns a previously not loaded property of a ClientObject|All
**[Execute&#8209;PnPQuery](ExecutePnPQuery.md)** |Execute the current queued actions|All
**[Get&#8209;PnPStoredCredential](GetPnPStoredCredential.md)** |Get a credential|All
**[Set&#8209;PnPTraceLog](SetPnPTraceLog.md)** |Turn log tracing on or off|All


### Branding 
Cmdlet|Description|Platform
:-----|:----------|:-------
**[Add&#8209;PnPCustomAction](AddPnPCustomAction.md)** |Adds a custom action|All
**[Get&#8209;PnPCustomAction](GetPnPCustomAction.md)** |Return user custom actions|All
**[Remove&#8209;PnPCustomAction](RemovePnPCustomAction.md)** |Removes a custom action|All
**[Get&#8209;PnPHomePage](GetPnPHomePage.md)** |Return the homepage|All
**[Set&#8209;PnPHomePage](SetPnPHomePage.md)** |Sets the home page of the current web.|All
**[Add&#8209;PnPJavaScriptBlock](AddPnPJavaScriptBlock.md)** |Adds a link to a JavaScript snippet/block to a web or site collection|All
**[Add&#8209;PnPJavaScriptLink](AddPnPJavaScriptLink.md)** |Adds a link to a JavaScript file to a web or sitecollection|All
**[Get&#8209;PnPJavaScriptLink](GetPnPJavaScriptLink.md)** |Returns all or a specific custom action(s) with location type ScriptLink|All
**[Remove&#8209;PnPJavaScriptLink](RemovePnPJavaScriptLink.md)** |Removes a JavaScript link or block from a web or sitecollection|All
**[Get&#8209;PnPMasterPage](GetPnPMasterPage.md)** |Returns the URLs of the default Master Page and the custom Master Page.|All
**[Set&#8209;PnPMasterPage](SetPnPMasterPage.md)** |Set the masterpage|All
**[Set&#8209;PnPMinimalDownloadStrategy](SetPnPMinimalDownloadStrategy.md)** |Activates or deactivates the minimal downloading strategy.|All
**[Add&#8209;PnPNavigationNode](AddPnPNavigationNode.md)** |Adds an item to a navigation element|All
**[Remove&#8209;PnPNavigationNode](RemovePnPNavigationNode.md)** |Removes a menu item from either the quicklaunch or top navigation|All
**[Disable&#8209;PnPResponsiveUI](DisablePnPResponsiveUI.md)** |Deactive the PnP Response UI add-on|All
**[Enable&#8209;PnPResponsiveUI](EnablePnPResponsiveUI.md)** |Activates the PnP Response UI Add-on|All
**[Get&#8209;PnPTheme](GetPnPTheme.md)** |Returns the current theme/composed look of the current web.|All
**[Set&#8209;PnPTheme](SetPnPTheme.md)** |Sets the theme of the current web.|All


### Client-Side Pages 
Cmdlet|Description|Platform
:-----|:----------|:-------
**[Get&#8209;PnPAvailableClientSideComponents](GetPnPAvailableClientSideComponents.md)** |Gets the available client side components on a particular page|SharePoint Online
**[Add&#8209;PnPClientSidePage](AddPnPClientSidePage.md)** |Adds a Client-Side Page|SharePoint Online
**[Get&#8209;PnPClientSidePage](GetPnPClientSidePage.md)** |Gets a Client-Side Page|SharePoint Online
**[Remove&#8209;PnPClientSidePage](RemovePnPClientSidePage.md)** |Removes a Client-Side Page|SharePoint Online
**[Set&#8209;PnPClientSidePage](SetPnPClientSidePage.md)** |Sets parameters of a Client-Side Page|SharePoint Online
**[Add&#8209;PnPClientSidePageSection](AddPnPClientSidePageSection.md)** |Adds a new section to a Client-Side page|SharePoint Online
**[Add&#8209;PnPClientSideText](AddPnPClientSideText.md)** |Adds a text element to a client-side page.|SharePoint Online
**[Add&#8209;PnPClientSideWebPart](AddPnPClientSideWebPart.md)** |Adds a Client-Side Web Part to a client-side page|SharePoint Online


### Content Types 
Cmdlet|Description|Platform
:-----|:----------|:-------
**[Add&#8209;PnPContentType](AddPnPContentType.md)** |Adds a new content type|All
**[Get&#8209;PnPContentType](GetPnPContentType.md)** |Retrieves a content type|All
**[Remove&#8209;PnPContentType](RemovePnPContentType.md)** |Removes a content type from a web|All
**[Remove&#8209;PnPContentTypeFromList](RemovePnPContentTypeFromList.md)** |Removes a content type from a list|All
**[Get&#8209;PnPContentTypePublishingHubUrl](GetPnPContentTypePublishingHubUrl.md)** |Returns the url to Content Type Publishing Hub|All
**[Add&#8209;PnPContentTypeToList](AddPnPContentTypeToList.md)** |Adds a new content type to a list|All
**[Set&#8209;PnPDefaultContentTypeToList](SetPnPDefaultContentTypeToList.md)** |Sets the default content type for a list|All
**[Remove&#8209;PnPFieldFromContentType](RemovePnPFieldFromContentType.md)** |Removes a site column from a content type|All
**[Add&#8209;PnPFieldToContentType](AddPnPFieldToContentType.md)** |Adds an existing site column to a content type|All


### Document Sets 
Cmdlet|Description|Platform
:-----|:----------|:-------
**[Remove&#8209;PnPContentTypeFromDocumentSet](RemovePnPContentTypeFromDocumentSet.md)** |Removes a content type from a document set|All
**[Add&#8209;PnPContentTypeToDocumentSet](AddPnPContentTypeToDocumentSet.md)** |Adds a content type to a document set|All
**[Add&#8209;PnPDocumentSet](AddPnPDocumentSet.md)** |Creates a new document set in a library.|All
**[Set&#8209;PnPDocumentSetField](SetPnPDocumentSetField.md)** |Sets a site column from the available content types to a document set|All
**[Get&#8209;PnPDocumentSetTemplate](GetPnPDocumentSetTemplate.md)** |Retrieves a document set template|All


### Event Receivers 
Cmdlet|Description|Platform
:-----|:----------|:-------
**[Add&#8209;PnPEventReceiver](AddPnPEventReceiver.md)** |Adds a new event receiver|All
**[Get&#8209;PnPEventReceiver](GetPnPEventReceiver.md)** |Return registered eventreceivers|All
**[Remove&#8209;PnPEventReceiver](RemovePnPEventReceiver.md)** |Remove an eventreceiver|All


### Features 
Cmdlet|Description|Platform
:-----|:----------|:-------
**[New&#8209;PnPExtensbilityHandlerObject](NewPnPExtensbilityHandlerObject.md)** |Creates an ExtensibilityHandler Object, to be used by the Get-SPOProvisioningTemplate cmdlet|All
**[Disable&#8209;PnPFeature](DisablePnPFeature.md)** |Disables a feature|All
**[Enable&#8209;PnPFeature](EnablePnPFeature.md)** |Enables a feature|All
**[Get&#8209;PnPFeature](GetPnPFeature.md)** |Returns all activated or a specific activated feature|All


### Fields 
Cmdlet|Description|Platform
:-----|:----------|:-------
**[Add&#8209;PnPField](AddPnPField.md)** |Add a field|All
**[Get&#8209;PnPField](GetPnPField.md)** |Returns a field from a list or site|All
**[Remove&#8209;PnPField](RemovePnPField.md)** |Removes a field from a list or a site|All
**[Set&#8209;PnPField](SetPnPField.md)** |Changes one or more properties of a field in a specific list or for the whole web|All
**[Add&#8209;PnPFieldFromXml](AddPnPFieldFromXml.md)** |Adds a field to a list or as a site column based upon a CAML/XML field definition|All
**[Add&#8209;PnPTaxonomyField](AddPnPTaxonomyField.md)** |Add a taxonomy field|All
**[Set&#8209;PnPView](SetPnPView.md)** |Change view properties|All


### Files and Folders 
Cmdlet|Description|Platform
:-----|:----------|:-------
**[Add&#8209;PnPFile](AddPnPFile.md)** |Uploads a file to Web|All
**[Copy&#8209;PnPFile](CopyPnPFile.md)** |Copies a file or folder to a different location|All
**[Find&#8209;PnPFile](FindPnPFile.md)** |Finds a file in the virtual file system of the web.|All
**[Get&#8209;PnPFile](GetPnPFile.md)** |Downloads a file.|All
**[Move&#8209;PnPFile](MovePnPFile.md)** |Moves a file to a different location|All
**[Remove&#8209;PnPFile](RemovePnPFile.md)** |Removes a file.|All
**[Rename&#8209;PnPFile](RenamePnPFile.md)** |Renames a file in its current location|All
**[Set&#8209;PnPFileCheckedIn](SetPnPFileCheckedIn.md)** |Checks in a file|All
**[Set&#8209;PnPFileCheckedOut](SetPnPFileCheckedOut.md)** |Checks out a file|All
**[Add&#8209;PnPFolder](AddPnPFolder.md)** |Creates a folder within a parent folder|All
**[Ensure&#8209;PnPFolder](EnsurePnPFolder.md)** |Returns a folder from a given site relative path, and will create it if it does not exist.|All
**[Get&#8209;PnPFolder](GetPnPFolder.md)** |Return a folder object|All
**[Move&#8209;PnPFolder](MovePnPFolder.md)** |Move a folder to another location in the current web|All
**[Remove&#8209;PnPFolder](RemovePnPFolder.md)** |Deletes a folder within a parent folder|All
**[Rename&#8209;PnPFolder](RenamePnPFolder.md)** |Renames a folder|All
**[Get&#8209;PnPFolderItem](GetPnPFolderItem.md)** |List content in folder|All
**[Copy&#8209;PnPItemProxy](CopyPnPItemProxy.md)** |Proxy cmdlet for using Copy-Item between SharePoint provider and FileSystem provider|All
**[Move&#8209;PnPItemProxy](MovePnPItemProxy.md)** |Proxy cmdlet for using Move-Item between SharePoint provider and FileSystem provider|All


### Information Management 
Cmdlet|Description|Platform
:-----|:----------|:-------
**[Get&#8209;PnPSiteClosure](GetPnPSiteClosure.md)** |Get the site closure status of the site which has a site policy applied|All
**[Set&#8209;PnPSiteClosure](SetPnPSiteClosure.md)** |Opens or closes a site which has a site policy applied|All
**[Set&#8209;PnPSitePolicy](SetPnPSitePolicy.md)** |Sets a site policy|All
**[Get&#8209;PnPSitePolicy](GetPnPSitePolicy.md)** |Retrieves all or a specific site policy|All


### Lists 
Cmdlet|Description|Platform
:-----|:----------|:-------
**[Get&#8209;PnPDefaultColumnValues](GetPnPDefaultColumnValues.md)** |Gets the default column values for all folders in document library|All
**[Set&#8209;PnPDefaultColumnValues](SetPnPDefaultColumnValues.md)** |Sets default column values for a document library|All
**[Get&#8209;PnPList](GetPnPList.md)** |Returns a List object|All
**[New&#8209;PnPList](NewPnPList.md)** |Creates a new list|All
**[Remove&#8209;PnPList](RemovePnPList.md)** |Deletes a list|All
**[Set&#8209;PnPList](SetPnPList.md)** |Updates list settings|All
**[Add&#8209;PnPListItem](AddPnPListItem.md)** |Adds an item to a list|All
**[Get&#8209;PnPListItem](GetPnPListItem.md)** |Retrieves list items|All
**[Remove&#8209;PnPListItem](RemovePnPListItem.md)** |Deletes an item from a list|All
**[Set&#8209;PnPListItem](SetPnPListItem.md)** |Updates a list item|All
**[Set&#8209;PnPListItemPermission](SetPnPListItemPermission.md)** |Sets list item permissions|All
**[Move&#8209;PnPListItemToRecycleBin](MovePnPListItemToRecycleBin.md)** |Moves an item from a list to the Recycle Bin|All
**[Set&#8209;PnPListPermission](SetPnPListPermission.md)** |Sets list permissions|All
**[Get&#8209;PnPProvisioningTemplateFromGallery](GetPnPProvisioningTemplateFromGallery.md)** |Retrieves or searches provisioning templates from the PnP Template Gallery|All
**[Request&#8209;PnPReIndexList](RequestPnPReIndexList.md)** |Marks the list for full indexing during the next incremental crawl|All
**[Add&#8209;PnPView](AddPnPView.md)** |Adds a view to a list|All
**[Get&#8209;PnPView](GetPnPView.md)** |Returns one or all views from a list|All
**[Remove&#8209;PnPView](RemovePnPView.md)** |Deletes a view from a list|All


### Microsoft Graph 
Cmdlet|Description|Platform
:-----|:----------|:-------
**[Connect&#8209;PnPMicrosoftGraph](ConnectPnPMicrosoftGraph.md)** |Connect to the Microsoft Graph|All
**[Get&#8209;PnPUnifiedGroup](GetPnPUnifiedGroup.md)** |Gets one Office 365 Group (aka Unified Group) or a list of Office 365 Groups|All
**[New&#8209;PnPUnifiedGroup](NewPnPUnifiedGroup.md)** |Creates a new Office 365 Group (aka Unified Group)|All
**[Remove&#8209;PnPUnifiedGroup](RemovePnPUnifiedGroup.md)** |Removes one Office 365 Group (aka Unified Group) or a list of Office 365 Groups|All
**[Set&#8209;PnPUnifiedGroup](SetPnPUnifiedGroup.md)** |Sets Office 365 Group (aka Unified Group) properties|All


### Provisioning 
Cmdlet|Description|Platform
:-----|:----------|:-------
**[Add&#8209;PnPDataRowsToProvisioningTemplate](AddPnPDataRowsToProvisioningTemplate.md)** |Adds datarows to a list inside a PnP Provisioning Template|All
**[Remove&#8209;PnPFileFromProvisioningTemplate](RemovePnPFileFromProvisioningTemplate.md)** |Removes a file from a PnP Provisioning Template|All
**[Add&#8209;PnPFileToProvisioningTemplate](AddPnPFileToProvisioningTemplate.md)** |Adds a file to a PnP Provisioning Template|All
**[Convert&#8209;PnPFolderToProvisioningTemplate](ConvertPnPFolderToProvisioningTemplate.md)** |Creates a pnp package file of an existing template xml, and includes all files in the current folder|All
**[Add&#8209;PnPListFoldersToProvisioningTemplate](AddPnPListFoldersToProvisioningTemplate.md)** |Adds folders to a list in a PnP Provisioning Template|All
**[Apply&#8209;PnPProvisioningTemplate](ApplyPnPProvisioningTemplate.md)** |Applies a provisioning template to a web|All
**[Convert&#8209;PnPProvisioningTemplate](ConvertPnPProvisioningTemplate.md)** |Converts a provisioning template to an other schema version|All
**[Get&#8209;PnPProvisioningTemplate](GetPnPProvisioningTemplate.md)** |Generates a provisioning template from a web|All
**[Load&#8209;PnPProvisioningTemplate](LoadPnPProvisioningTemplate.md)** |Loads a PnP file from the file systems|All
**[New&#8209;PnPProvisioningTemplate](NewPnPProvisioningTemplate.md)** |Creates a new provisioning template object|All
**[Save&#8209;PnPProvisioningTemplate](SavePnPProvisioningTemplate.md)** |Saves a PnP file to the file systems|All
**[New&#8209;PnPProvisioningTemplateFromFolder](NewPnPProvisioningTemplateFromFolder.md)** |Generates a provisioning template from a given folder, including only files that are present in that folder|All
**[Set&#8209;PnPProvisioningTemplateMetadata](SetPnPProvisioningTemplateMetadata.md)** |Sets metadata of a provisioning template|All


### Publishing 
Cmdlet|Description|Platform
:-----|:----------|:-------
**[Set&#8209;PnPAvailablePageLayouts](SetPnPAvailablePageLayouts.md)** |Sets the available page layouts for the current site|All
**[Set&#8209;PnPDefaultPageLayout](SetPnPDefaultPageLayout.md)** |Sets a specific page layout to be the default page layout for a publishing site|All
**[Add&#8209;PnPHtmlPublishingPageLayout](AddPnPHtmlPublishingPageLayout.md)** |Adds a HTML based publishing page layout|All
**[Add&#8209;PnPMasterPage](AddPnPMasterPage.md)** |Adds a Masterpage|All
**[Add&#8209;PnPPublishingImageRendition](AddPnPPublishingImageRendition.md)** |Adds an Image Rendition if the Name of the Image Rendition does not already exist. This prevents creating two Image Renditions that share the same name.|All
**[Get&#8209;PnPPublishingImageRendition](GetPnPPublishingImageRendition.md)** |Returns all image renditions or if Identity is specified a specific one|All
**[Remove&#8209;PnPPublishingImageRendition](RemovePnPPublishingImageRendition.md)** |Removes an existing image rendition|All
**[Add&#8209;PnPPublishingPage](AddPnPPublishingPage.md)** |Adds a publishing page|All
**[Add&#8209;PnPPublishingPageLayout](AddPnPPublishingPageLayout.md)** |Adds a publishing page layout|All
**[Add&#8209;PnPWikiPage](AddPnPWikiPage.md)** |Adds a wiki page|All
**[Remove&#8209;PnPWikiPage](RemovePnPWikiPage.md)** |Removes a wiki page|All
**[Get&#8209;PnPWikiPageContent](GetPnPWikiPageContent.md)** |Gets the contents/source of a wiki page|All
**[Set&#8209;PnPWikiPageContent](SetPnPWikiPageContent.md)** |Sets the contents of a wikipage|All


### Records Management 
Cmdlet|Description|Platform
:-----|:----------|:-------
**[Set&#8209;PnPInPlaceRecordsManagement](SetPnPInPlaceRecordsManagement.md)** |Activates or deactivates in the place records management feature.|All
**[Disable&#8209;PnPInPlaceRecordsManagementForSite](DisablePnPInPlaceRecordsManagementForSite.md)** |Disables in place records management for a site.|All
**[Enable&#8209;PnPInPlaceRecordsManagementForSite](EnablePnPInPlaceRecordsManagementForSite.md)** |Enables in place records management for a site.|All
**[Clear&#8209;PnPListItemAsRecord](ClearPnPListItemAsRecord.md)** |Undeclares a list item as a record|SharePoint Online
**[Set&#8209;PnPListItemAsRecord](SetPnPListItemAsRecord.md)** |Declares a list item as a record|SharePoint Online
**[Test&#8209;PnPListItemIsRecord](TestPnPListItemIsRecord.md)** |Checks if a list item is a record|SharePoint Online


### Search 
Cmdlet|Description|Platform
:-----|:----------|:-------
**[Get&#8209;PnPSearchConfiguration](GetPnPSearchConfiguration.md)** |Returns the search configuration|All
**[Set&#8209;PnPSearchConfiguration](SetPnPSearchConfiguration.md)** |Sets the search configuration|All
**[Submit&#8209;PnPSearchQuery](SubmitPnPSearchQuery.md)** |Executes an arbitrary search query against the SharePoint search index|All
**[Get&#8209;PnPSiteSearchQueryResults](GetPnPSiteSearchQueryResults.md)** |Executes a search query to retrieve indexed site collections|All


### SharePoint Recycle Bin 
Cmdlet|Description|Platform
:-----|:----------|:-------
**[Clear&#8209;PnpRecycleBinItem](ClearPnpRecycleBinItem.md)** |Permanently deletes all or a specific recycle bin item|All
**[Move&#8209;PnpRecycleBinItem](MovePnpRecycleBinItem.md)** |Moves all items or a specific item in the first stage recycle bin of the current site collection to the second stage recycle bin|SharePoint Online
**[Restore&#8209;PnpRecycleBinItem](RestorePnpRecycleBinItem.md)** |Restores the provided recycle bin item to its original location|All
**[Get&#8209;PnPRecycleBinItem](GetPnPRecycleBinItem.md)** |Returns the items in the recycle bin from the context|All
**[Get&#8209;PnPTenantRecycleBinItem](GetPnPTenantRecycleBinItem.md)** |Returns the items in the tenant scoped recycle bin|SharePoint Online


### SharePoint WebHooks 
Cmdlet|Description|Platform
:-----|:----------|:-------
**[Add&#8209;PnPWebhookSubscription](AddPnPWebhookSubscription.md)** |Adds a new Webhook subscription|SharePoint Online
**[Remove&#8209;PnPWebhookSubscription](RemovePnPWebhookSubscription.md)** |Removes a Webhook subscription from the resource|SharePoint Online
**[Set&#8209;PnPWebhookSubscription](SetPnPWebhookSubscription.md)** |Updates a Webhook subscription|SharePoint Online
**[Get&#8209;PnPWebhookSubscriptions](GetPnPWebhookSubscriptions.md)** |Gets all the Webhook subscriptions of the resource|SharePoint Online


### Sites 
Cmdlet|Description|Platform
:-----|:----------|:-------
**[Set&#8209;PnPAppSideLoading](SetPnPAppSideLoading.md)** |Enables the App SideLoading Feature on a site|All
**[Get&#8209;PnPAuditing](GetPnPAuditing.md)** |Get the Auditing setting of a site|All
**[Set&#8209;PnPAuditing](SetPnPAuditing.md)** |Set Auditing setting for a site|All
**[Get&#8209;PnPSite](GetPnPSite.md)** |Returns the current site collection from the context.|All
**[Add&#8209;PnPSiteCollectionAdmin](AddPnPSiteCollectionAdmin.md)** |Adds one or more users as site collection administrators to the site collection in the current context|All
**[Get&#8209;PnPSiteCollectionAdmin](GetPnPSiteCollectionAdmin.md)** |Returns the current site collection administrators of the site colleciton in the current context|All
**[Remove&#8209;PnPSiteCollectionAdmin](RemovePnPSiteCollectionAdmin.md)** |Removes one or more users as site collection administrators from the site collection in the current context|All
**[Install&#8209;PnPSolution](InstallPnPSolution.md)** |Installs a sandboxed solution to a site collection. WARNING! This method can delete your composed look gallery due to the method used to activate the solution. We recommend you to only to use this cmdlet if you are okay with that.|All
**[Uninstall&#8209;PnPSolution](UninstallPnPSolution.md)** |Uninstalls a sandboxed solution from a site collection|All


### Taxonomy 
Cmdlet|Description|Platform
:-----|:----------|:-------
**[Get&#8209;PnPSiteCollectionTermStore](GetPnPSiteCollectionTermStore.md)** |Returns the site collection term store|All
**[Export&#8209;PnPTaxonomy](ExportPnPTaxonomy.md)** |Exports a taxonomy to either the output or to a file.|All
**[Import&#8209;PnPTaxonomy](ImportPnPTaxonomy.md)** |Imports a taxonomy from either a string array or a file|All
**[Set&#8209;PnPTaxonomyFieldValue](SetPnPTaxonomyFieldValue.md)** |Sets a taxonomy term value in a listitem field|All
**[Get&#8209;PnPTaxonomyItem](GetPnPTaxonomyItem.md)** |Returns a taxonomy item|All
**[Remove&#8209;PnPTaxonomyItem](RemovePnPTaxonomyItem.md)** |Removes a taxonomy item|All
**[Get&#8209;PnPTaxonomySession](GetPnPTaxonomySession.md)** |Returns a taxonomy session|All
**[Get&#8209;PnPTerm](GetPnPTerm.md)** |Returns a taxonomy term|All
**[New&#8209;PnPTerm](NewPnPTerm.md)** |Creates a taxonomy term|All
**[Get&#8209;PnPTermGroup](GetPnPTermGroup.md)** |Returns a taxonomy term group|All
**[New&#8209;PnPTermGroup](NewPnPTermGroup.md)** |Creates a taxonomy term group|All
**[Remove&#8209;PnPTermGroup](RemovePnPTermGroup.md)** |Removes a taxonomy term group and all its containing termsets|All
**[Import&#8209;PnPTermGroupFromXml](ImportPnPTermGroupFromXml.md)** |Imports a taxonomy TermGroup from either the input or from an XML file.|All
**[Export&#8209;PnPTermGroupToXml](ExportPnPTermGroupToXml.md)** |Exports a taxonomy TermGroup to either the output or to an XML file.|All
**[Get&#8209;PnPTermSet](GetPnPTermSet.md)** |Returns a taxonomy term set|All
**[Import&#8209;PnPTermSet](ImportPnPTermSet.md)** |Imports a taxonomy term set from a file in the standard format.|All
**[New&#8209;PnPTermSet](NewPnPTermSet.md)** |Creates a taxonomy term set|All


### Tenant Administration 
Cmdlet|Description|Platform
:-----|:----------|:-------
**[Get&#8209;PnPAccessToken](GetPnPAccessToken.md)** |Returns the current OAuth Access token|All
**[New&#8209;PnPSite](NewPnPSite.md)** |BETA: This cmdlet is using early release APIs. Notice that functionality and parameters can change. Creates a new site collection|SharePoint Online
**[Clear&#8209;PnPTenantRecycleBinItem](ClearPnPTenantRecycleBinItem.md)** |Permanently deletes a site collection from the tenant scoped recycle bin|All
**[Restore&#8209;PnPTenantRecycleBinItem](RestorePnPTenantRecycleBinItem.md)** |Restores a site collection from the tenant scoped recycle bin|SharePoint Online
**[Get&#8209;PnPTenantSite](GetPnPTenantSite.md)** |Retrieve site information.|SharePoint Online
**[New&#8209;PnPTenantSite](NewPnPTenantSite.md)** |Creates a new site collection for the current tenant|All
**[Remove&#8209;PnPTenantSite](RemovePnPTenantSite.md)** |Removes a site collection|SharePoint Online
**[Set&#8209;PnPTenantSite](SetPnPTenantSite.md)** |Set site information.|SharePoint Online
**[Get&#8209;PnPTimeZoneId](GetPnPTimeZoneId.md)** |Returns a time zone ID|All
**[Get&#8209;PnPWebTemplates](GetPnPWebTemplates.md)** |Returns the available web templates.|SharePoint Online


### User and group management 
Cmdlet|Description|Platform
:-----|:----------|:-------
**[Get&#8209;PnPGroup](GetPnPGroup.md)** |Returns a specific group or all groups.|All
**[New&#8209;PnPGroup](NewPnPGroup.md)** |Adds group to the Site Groups List and returns a group object|All
**[Remove&#8209;PnPGroup](RemovePnPGroup.md)** |Removes a group from a web.|All
**[Set&#8209;PnPGroup](SetPnPGroup.md)** |Updates a group|All
**[Get&#8209;PnPGroupPermissions](GetPnPGroupPermissions.md)** |Returns the permissions for a specific SharePoint group|All
**[Set&#8209;PnPGroupPermissions](SetPnPGroupPermissions.md)** |Adds and/or removes permissions of a specific SharePoint group|All
**[Get&#8209;PnPUser](GetPnPUser.md)** |Returns site users of current web|All
**[New&#8209;PnPUser](NewPnPUser.md)** |Adds a user to the built-in Site User Info List and returns a user object|All
**[Remove&#8209;PnPUser](RemovePnPUser.md)** |Removes a specific user from the site collection User Information List|All
**[Remove&#8209;PnPUserFromGroup](RemovePnPUserFromGroup.md)** |Removes a user from a group|All
**[Add&#8209;PnPUserToGroup](AddPnPUserToGroup.md)** |Adds a user to a group|All


### User Profiles 
Cmdlet|Description|Platform
:-----|:----------|:-------
**[New&#8209;PnPPersonalSite](NewPnPPersonalSite.md)** |Office365 only: Creates a personal / OneDrive For Business site|SharePoint Online
**[Get&#8209;PnPUserProfileProperty](GetPnPUserProfileProperty.md)** |You must connect to the tenant admin website (https://:<tenant>-admin.sharepoint.com) with Connect-PnPOnline in order to use this cmdlet.  |All
**[Set&#8209;PnPUserProfileProperty](SetPnPUserProfileProperty.md)** |Office365 only: Uses the tenant API to retrieve site information.  You must connect to the tenant admin website (https://:<tenant>-admin.sharepoint.com) with Connect-PnPOnline in order to use this command.  |SharePoint Online


### Utilities 
Cmdlet|Description|Platform
:-----|:----------|:-------
**[Send&#8209;PnPMail](SendPnPMail.md)** |Sends an email using the Office 365 SMTP Service or SharePoint, depending on the parameters specified. See detailed help for more information.|All


### Web Parts 
Cmdlet|Description|Platform
:-----|:----------|:-------
**[Get&#8209;PnPWebPart](GetPnPWebPart.md)** |Returns a webpart definition object|All
**[Remove&#8209;PnPWebPart](RemovePnPWebPart.md)** |Removes a webpart from a page|All
**[Get&#8209;PnPWebPartProperty](GetPnPWebPartProperty.md)** |Returns a web part property|All
**[Set&#8209;PnPWebPartProperty](SetPnPWebPartProperty.md)** |Sets a web part property|All
**[Add&#8209;PnPWebPartToWebPartPage](AddPnPWebPartToWebPartPage.md)** |Adds a webpart to a web part page in a specified zone|All
**[Add&#8209;PnPWebPartToWikiPage](AddPnPWebPartToWikiPage.md)** |Adds a webpart to a wiki page in a specified table row and column|All
**[Get&#8209;PnPWebPartXml](GetPnPWebPartXml.md)** |Returns the webpart XML of a webpart registered on a site|All


### Webs 
Cmdlet|Description|Platform
:-----|:----------|:-------
**[Set&#8209;PnPIndexedProperties](SetPnPIndexedProperties.md)** |Marks values of the propertybag to be indexed by search. Notice that this will overwrite the existing flags, i.e. only the properties you define with the cmdlet will be indexed.|All
**[Add&#8209;PnPIndexedProperty](AddPnPIndexedProperty.md)** |Marks the value of the propertybag key specified to be indexed by search.|All
**[Remove&#8209;PnPIndexedProperty](RemovePnPIndexedProperty.md)** |Removes a key from propertybag to be indexed by search. The key and it's value remain in the propertybag, however it will not be indexed anymore.|All
**[Get&#8209;PnPIndexedPropertyKeys](GetPnPIndexedPropertyKeys.md)** |Returns the keys of the property bag values that have been marked for indexing by search|All
**[Get&#8209;PnPPropertyBag](GetPnPPropertyBag.md)** |Returns the property bag values.|All
**[Remove&#8209;PnPPropertyBagValue](RemovePnPPropertyBagValue.md)** |Removes a value from the property bag|All
**[Set&#8209;PnPPropertyBagValue](SetPnPPropertyBagValue.md)** |Sets a property bag value|All
**[Request&#8209;PnPReIndexWeb](RequestPnPReIndexWeb.md)** |Marks the web for full indexing during the next incremental crawl|All
**[Get&#8209;PnPRequestAccessEmails](GetPnPRequestAccessEmails.md)** |Returns the request access e-mail addresses|SharePoint Online
**[Set&#8209;PnPRequestAccessEmails](SetPnPRequestAccessEmails.md)** |Sets Request Access Emails on a web|SharePoint Online
**[Get&#8209;PnPSubWebs](GetPnPSubWebs.md)** |Returns the subwebs of the current web|All
**[Get&#8209;PnPWeb](GetPnPWeb.md)** |Returns the current web object|All
**[New&#8209;PnPWeb](NewPnPWeb.md)** |Creates a new subweb under the current web|All
**[Remove&#8209;PnPWeb](RemovePnPWeb.md)** |Removes a subweb in the current web|All
**[Set&#8209;PnPWeb](SetPnPWeb.md)** |Sets properties on a web|All
**[Invoke&#8209;PnPWebAction](InvokePnPWebAction.md)** |Executes operations on web, lists and list items.|All
**[Set&#8209;PnPWebPermission](SetPnPWebPermission.md)** |Set permissions|All


### Workflows 
Cmdlet|Description|Platform
:-----|:----------|:-------
**[Add&#8209;PnPWorkflowDefinition](AddPnPWorkflowDefinition.md)** |Adds a workflow definition|All
**[Get&#8209;PnPWorkflowDefinition](GetPnPWorkflowDefinition.md)** |Return a workflow definition|All
**[Remove&#8209;PnPWorkflowDefinition](RemovePnPWorkflowDefinition.md)** |Removes a workflow definition|All
**[Resume&#8209;PnPWorkflowInstance](ResumePnPWorkflowInstance.md)** |Resume a workflow|All
**[Stop&#8209;PnPWorkflowInstance](StopPnPWorkflowInstance.md)** |Stops a workflow instance|All
**[Add&#8209;PnPWorkflowSubscription](AddPnPWorkflowSubscription.md)** |Adds a workflow subscription to a list|All
**[Get&#8209;PnPWorkflowSubscription](GetPnPWorkflowSubscription.md)** |Return a workflow subscription|All
**[Remove&#8209;PnPWorkflowSubscription](RemovePnPWorkflowSubscription.md)** |Remove workflow subscription|All


## Additional resources
<a name="bk_addresources"> </a>

-  [SharePoint PnP PowerShell on GitHub](https://github.com/SharePoint/PnP-PowerShell)
