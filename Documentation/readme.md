# Cmdlet Documentation #
Below you can find a list of all the available cmdlets. Many commands provide built-in help and examples. Retrieve the detailed help with 

```powershell
Get-Help Connect-PnPOnline -Detailed
```

##Apps
Cmdlet|Description
:-----|:----------
**[Uninstall&#8209;PnPAppInstance](UninstallPnPAppInstance.md)** |Removes an app from a site
**[Get&#8209;PnPAppInstance](GetPnPAppInstance.md)** |Returns a SharePoint AddIn Instance in the site
**[Import&#8209;PnPAppPackage](ImportPnPAppPackage.md)** |Adds a SharePoint Addin to a site
##Base Cmdlets
Cmdlet|Description
:-----|:----------
**[Get&#8209;PnPAuthenticationRealm](GetPnPAuthenticationRealm.md)** |Gets the authentication realm for the current web
**[Get&#8209;PnPAzureADManifestKeyCredentials](GetPnPAzureADManifestKeyCredentials.md)** |Creates the JSON snippet that is required for the manifest JSON file for Azure WebApplication / WebAPI apps
**[Get&#8209;PnPContext](GetPnPContext.md)** |Returns a Client Side Object Model context
**[Set&#8209;PnPContext](SetPnPContext.md)** |Sets the Client Context to use by the cmdlets
**[Get&#8209;PnPHealthScore](GetPnPHealthScore.md)** |Retrieves the current health score value of the server
**[Disconnect&#8209;PnPOnline](DisconnectPnPOnline.md)** |Disconnects the context
**[Connect&#8209;PnPOnline](ConnectPnPOnline.md)** |Connects to a SharePoint site and creates a context that is required for the other PnP Cmdlets
**[Get&#8209;PnPProperty](GetPnPProperty.md)** |Will populate properties of an object and optionally, if needed, load the value from the server. If one property is specified its value will be returned to the output.
**[Execute&#8209;PnPQuery](ExecutePnPQuery.md)** |Executes any queued actions / changes on the SharePoint Client Side Object Model Context
**[Get&#8209;PnPStoredCredential](GetPnPStoredCredential.md)** |Returns a stored credential from the Windows Credential Manager
**[Set&#8209;PnPTraceLog](SetPnPTraceLog.md)** |Defines if tracing should be turned on. PnP Core, which is the foundation of these cmdlets uses the standard Trace functionality of .NET. With this cmdlet you can turn capturing of this trace to a log file on or off.
##Branding
Cmdlet|Description
:-----|:----------
**[Remove&#8209;PnPCustomAction](RemovePnPCustomAction.md)** |Removes a custom action
**[Get&#8209;PnPCustomAction](GetPnPCustomAction.md)** |Returns all or a specific custom action(s)
**[Add&#8209;PnPCustomAction](AddPnPCustomAction.md)** |Adds a custom action to a web
**[Get&#8209;PnPHomePage](GetPnPHomePage.md)** |Returns the URL to the home page
**[Set&#8209;PnPHomePage](SetPnPHomePage.md)** |Sets the home page of the current web.
**[Add&#8209;PnPJavaScriptBlock](AddPnPJavaScriptBlock.md)** |Adds a link to a JavaScript snippet/block to a web or site collection
**[Get&#8209;PnPJavaScriptLink](GetPnPJavaScriptLink.md)** |Returns all or a specific custom action(s) with location type ScriptLink
**[Remove&#8209;PnPJavaScriptLink](RemovePnPJavaScriptLink.md)** |Removes a JavaScript link or block from a web or sitecollection
**[Add&#8209;PnPJavaScriptLink](AddPnPJavaScriptLink.md)** |Adds a link to a JavaScript file to a web or sitecollection
**[Get&#8209;PnPMasterPage](GetPnPMasterPage.md)** |Returns the URLs of the default Master Page and the custom Master Page.
**[Set&#8209;PnPMasterPage](SetPnPMasterPage.md)** |Sets the default master page of the current web.
**[Set&#8209;PnPMinimalDownloadStrategy](SetPnPMinimalDownloadStrategy.md)** |Activates or deactivates the minimal downloading strategy.
**[Add&#8209;PnPNavigationNode](AddPnPNavigationNode.md)** |Adds a menu item to either the quicklaunch or top navigation
**[Remove&#8209;PnPNavigationNode](RemovePnPNavigationNode.md)** |Removes a menu item from either the quicklaunch or top navigation
**[Disable&#8209;PnPResponsiveUI](DisablePnPResponsiveUI.md)** |Disables the PnP Responsive UI implementation on a classic SharePoint Site
**[Enable&#8209;PnPResponsiveUI](EnablePnPResponsiveUI.md)** |Enables the PnP Responsive UI implementation on a classic SharePoint Site
**[Get&#8209;PnPTheme](GetPnPTheme.md)** |Returns the current theme/composed look of the current web.
**[Set&#8209;PnPTheme](SetPnPTheme.md)** |Sets the theme of the current web.
##Content Types
Cmdlet|Description
:-----|:----------
**[Remove&#8209;PnPContentType](RemovePnPContentType.md)** |Removes a content type from a web
**[Get&#8209;PnPContentType](GetPnPContentType.md)** |Retrieves a content type
**[Add&#8209;PnPContentType](AddPnPContentType.md)** |Adds a new content type
**[Remove&#8209;PnPContentTypeFromList](RemovePnPContentTypeFromList.md)** |Removes a content type from a list
**[Get&#8209;PnPContentTypePublishingHubUrl](GetPnPContentTypePublishingHubUrl.md)** |Returns the url to Content Type Publishing Hub
**[Add&#8209;PnPContentTypeToList](AddPnPContentTypeToList.md)** |Adds a new content type to a list
**[Set&#8209;PnPDefaultContentTypeToList](SetPnPDefaultContentTypeToList.md)** |Sets the default content type for a list
**[Remove&#8209;PnPFieldFromContentType](RemovePnPFieldFromContentType.md)** |Removes a site column from a content type
**[Add&#8209;PnPFieldToContentType](AddPnPFieldToContentType.md)** |Adds an existing site column to a content type
##Document Sets
Cmdlet|Description
:-----|:----------
**[Remove&#8209;PnPContentTypeFromDocumentSet](RemovePnPContentTypeFromDocumentSet.md)** |Removes a content type from a document set
**[Add&#8209;PnPContentTypeToDocumentSet](AddPnPContentTypeToDocumentSet.md)** |Adds a content type to a document set
**[Add&#8209;PnPDocumentSet](AddPnPDocumentSet.md)** |Creates a new document set in a library.
**[Set&#8209;PnPDocumentSetField](SetPnPDocumentSetField.md)** |Sets a site column from the available content types to a document set
**[Get&#8209;PnPDocumentSetTemplate](GetPnPDocumentSetTemplate.md)** |Retrieves a document set template
##Event Receivers
Cmdlet|Description
:-----|:----------
**[Add&#8209;PnPEventReceiver](AddPnPEventReceiver.md)** |Adds a new event receiver
**[Get&#8209;PnPEventReceiver](GetPnPEventReceiver.md)** |Returns all or a specific event receiver
**[Remove&#8209;PnPEventReceiver](RemovePnPEventReceiver.md)** |Removes/unregisters a specific event receiver
##Features
Cmdlet|Description
:-----|:----------
**[New&#8209;PnPExtensbilityHandlerObject](NewPnPExtensbilityHandlerObject.md)** |Creates an ExtensibilityHandler Object, to be used by the Get-SPOProvisioningTemplate cmdlet
**[Get&#8209;PnPFeature](GetPnPFeature.md)** |Returns all activated or a specific activated feature
**[Disable&#8209;PnPFeature](DisablePnPFeature.md)** |Disables a feature
**[Enable&#8209;PnPFeature](EnablePnPFeature.md)** |Enables a feature
##Fields
Cmdlet|Description
:-----|:----------
**[Remove&#8209;PnPField](RemovePnPField.md)** |Removes a field from a list or a site
**[Get&#8209;PnPField](GetPnPField.md)** |Returns a field from a list or site
**[Add&#8209;PnPField](AddPnPField.md)** |Adds a field to a list or as a site column
**[Add&#8209;PnPFieldFromXml](AddPnPFieldFromXml.md)** |Adds a field to a list or as a site column based upon a CAML/XML field definition
**[Add&#8209;PnPTaxonomyField](AddPnPTaxonomyField.md)** |Adds a taxonomy field to a list or as a site column.
##Files and Folders
Cmdlet|Description
:-----|:----------
**[Copy&#8209;PnPFile](CopyPnPFile.md)** |Copies a file to a different location
**[Move&#8209;PnPFile](MovePnPFile.md)** |Moves a file to a different location
**[Rename&#8209;PnPFile](RenamePnPFile.md)** |Renames a file in its current location
**[Remove&#8209;PnPFile](RemovePnPFile.md)** |Removes a file.
**[Get&#8209;PnPFile](GetPnPFile.md)** |Downloads a file.
**[Find&#8209;PnPFile](FindPnPFile.md)** |Finds a file in the virtual file system of the web.
**[Add&#8209;PnPFile](AddPnPFile.md)** |Uploads a file to Web
**[Set&#8209;PnPFileCheckedIn](SetPnPFileCheckedIn.md)** |Checks in a file
**[Set&#8209;PnPFileCheckedOut](SetPnPFileCheckedOut.md)** |Checks out a file
**[Move&#8209;PnPFolder](MovePnPFolder.md)** |Move a folder to another location in the current web
**[Rename&#8209;PnPFolder](RenamePnPFolder.md)** |Renames a folder
**[Get&#8209;PnPFolder](GetPnPFolder.md)** |Return a folder object
**[Ensure&#8209;PnPFolder](EnsurePnPFolder.md)** |Returns a folder from a given site relative path, and will create it if it does not exist.
**[Add&#8209;PnPFolder](AddPnPFolder.md)** |Creates a folder within a parent folder
**[Remove&#8209;PnPFolder](RemovePnPFolder.md)** |Deletes a folder within a parent folder
**[Get&#8209;PnPFolderItem](GetPnPFolderItem.md)** |List content in folder
##Information Management
Cmdlet|Description
:-----|:----------
**[Get&#8209;PnPSiteClosure](GetPnPSiteClosure.md)** |Get the site closure status of the site which has a site policy applied
**[Set&#8209;PnPSiteClosure](SetPnPSiteClosure.md)** |Opens or closes a site which has a site policy applied
**[Get&#8209;PnPSitePolicy](GetPnPSitePolicy.md)** |Retrieves all or a specific site policy
**[Set&#8209;PnPSitePolicy](SetPnPSitePolicy.md)** |Sets a site policy
##Lists
Cmdlet|Description
:-----|:----------
**[Set&#8209;PnPDefaultColumnValues](SetPnPDefaultColumnValues.md)** |Sets default column values for a document library
**[Remove&#8209;PnPList](RemovePnPList.md)** |Deletes a list
**[Set&#8209;PnPList](SetPnPList.md)** |Updates list settings
**[Get&#8209;PnPList](GetPnPList.md)** |Returns a List object
**[New&#8209;PnPList](NewPnPList.md)** |Creates a new list
**[Add&#8209;PnPListItem](AddPnPListItem.md)** |Adds an item to a list
**[Remove&#8209;PnPListItem](RemovePnPListItem.md)** |Deletes an item from a list
**[Set&#8209;PnPListItem](SetPnPListItem.md)** |Updates a list item
**[Get&#8209;PnPListItem](GetPnPListItem.md)** |Retrieves list items
**[Set&#8209;PnPListPermission](SetPnPListPermission.md)** |Sets list permissions
**[Get&#8209;PnPProvisioningTemplateFromGallery](GetPnPProvisioningTemplateFromGallery.md)** |Retrieves or searches provisioning templates from the PnP Template Gallery
**[Request&#8209;PnPReIndexList](RequestPnPReIndexList.md)** |Marks the list for full indexing during the next incremental crawl
**[Add&#8209;PnPView](AddPnPView.md)** |Adds a view to a list
**[Get&#8209;PnPView](GetPnPView.md)** |Returns one or all views from a list
**[Remove&#8209;PnPView](RemovePnPView.md)** |Deletes a view from a list
##Microsoft Graph
Cmdlet|Description
:-----|:----------
**[Connect&#8209;PnPMicrosoftGraph](ConnectPnPMicrosoftGraph.md)** |Uses the Microsoft Authentication Library (Preview) to connect to Azure AD and to get an OAuth 2.0 Access Token to consume the Microsoft Graph API
**[Get&#8209;PnPUnifiedGroup](GetPnPUnifiedGroup.md)** |Gets one Office 365 Group (aka Unified Group) or a list of Office 365 Groups
**[New&#8209;PnPUnifiedGroup](NewPnPUnifiedGroup.md)** |Creates a new Office 365 Group (aka Unified Group)
**[Remove&#8209;PnPUnifiedGroup](RemovePnPUnifiedGroup.md)** |Removes one Office 365 Group (aka Unified Group) or a list of Office 365 Groups
**[Set&#8209;PnPUnifiedGroup](SetPnPUnifiedGroup.md)** |Sets Office 365 Group (aka Unified Group) properties
##Provisioning
Cmdlet|Description
:-----|:----------
**[Remove&#8209;PnPFileFromProvisioningTemplate](RemovePnPFileFromProvisioningTemplate.md)** |Removes a file from an in-memory PnP Provisioning Template
**[Add&#8209;PnPFileToProvisioningTemplate](AddPnPFileToProvisioningTemplate.md)** |Adds a file to an in-memory PnP Provisioning Template
**[Convert&#8209;PnPFolderToProvisioningTemplate](ConvertPnPFolderToProvisioningTemplate.md)** |Creates a pnp package file of an existing template xml, and includes all files in the current folder
**[Load&#8209;PnPProvisioningTemplate](LoadPnPProvisioningTemplate.md)** |Loads a PnP file from the file systems
**[New&#8209;PnPProvisioningTemplate](NewPnPProvisioningTemplate.md)** |Creates a new provisioning template object
**[Save&#8209;PnPProvisioningTemplate](SavePnPProvisioningTemplate.md)** |Saves a PnP file to the file systems
**[Convert&#8209;PnPProvisioningTemplate](ConvertPnPProvisioningTemplate.md)** |Converts a provisioning template to an other schema version
**[Apply&#8209;PnPProvisioningTemplate](ApplyPnPProvisioningTemplate.md)** |Applies a provisioning template to a web
**[Get&#8209;PnPProvisioningTemplate](GetPnPProvisioningTemplate.md)** |Generates a provisioning template from a web
**[New&#8209;PnPProvisioningTemplateFromFolder](NewPnPProvisioningTemplateFromFolder.md)** |Generates a provisioning template from a given folder, including only files that are present in that folder
**[Set&#8209;PnPProvisioningTemplateMetadata](SetPnPProvisioningTemplateMetadata.md)** |Sets metadata of a provisioning template
##Publishing
Cmdlet|Description
:-----|:----------
**[Set&#8209;PnPAvailablePageLayouts](SetPnPAvailablePageLayouts.md)** |Sets the available page layouts for the current site
**[Set&#8209;PnPDefaultPageLayout](SetPnPDefaultPageLayout.md)** |Sets a specific page layout to be the default page layout for a publishing site
**[Add&#8209;PnPHtmlPublishingPageLayout](AddPnPHtmlPublishingPageLayout.md)** |Adds a HTML based publishing page layout
**[Add&#8209;PnPMasterPage](AddPnPMasterPage.md)** |Adds a Masterpage
**[Remove&#8209;PnPPublishingImageRendition](RemovePnPPublishingImageRendition.md)** |Removes an existing image rendition
**[Get&#8209;PnPPublishingImageRendition](GetPnPPublishingImageRendition.md)** |Returns all image renditions or if Identity is specified a specific one
**[Add&#8209;PnPPublishingImageRendition](AddPnPPublishingImageRendition.md)** |Adds an Image Rendition if the Name of the Image Rendition does not already exist. This prevents creating two Image Renditions that share the same name.
**[Add&#8209;PnPPublishingPage](AddPnPPublishingPage.md)** |Adds a publishing page
**[Add&#8209;PnPPublishingPageLayout](AddPnPPublishingPageLayout.md)** |Adds a publishing page layout
**[Remove&#8209;PnPWikiPage](RemovePnPWikiPage.md)** |Removes a wiki page
**[Add&#8209;PnPWikiPage](AddPnPWikiPage.md)** |Adds a wiki page
**[Get&#8209;PnPWikiPageContent](GetPnPWikiPageContent.md)** |Gets the contents/source of a wiki page
**[Set&#8209;PnPWikiPageContent](SetPnPWikiPageContent.md)** |Sets the contents of a wikipage
##Search
Cmdlet|Description
:-----|:----------
**[Set&#8209;PnPSearchConfiguration](SetPnPSearchConfiguration.md)** |Sets the search configuration
**[Get&#8209;PnPSearchConfiguration](GetPnPSearchConfiguration.md)** |Returns the search configuration
**[Submit&#8209;PnPSearchQuery](SubmitPnPSearchQuery.md)** |Executes an arbitrary search query against the SharePoint search index
**[Get&#8209;PnPSiteSearchQueryResults](GetPnPSiteSearchQueryResults.md)** |Executes a search query to retrieve indexed site collections
##SharePoint Recycle Bin
Cmdlet|Description
:-----|:----------
**[Restore&#8209;PnpRecycleBinItem](RestorePnpRecycleBinItem.md)** |Restores the provided recycle bin item to its original location
**[Clear&#8209;PnpRecycleBinItem](ClearPnpRecycleBinItem.md)** |Permanently deletes all or a specific recycle bin item
**[Move&#8209;PnpRecycleBinItem](MovePnpRecycleBinItem.md)** |Moves all items or a specific item in the first stage recycle bin of the current site collection to the second stage recycle bin
**[Get&#8209;PnPRecycleBinItem](GetPnPRecycleBinItem.md)** |Returns the items in the recycle bin from the context
**[Get&#8209;PnPTenantRecycleBinItem](GetPnPTenantRecycleBinItem.md)** |Returns the items in the tenant scoped recycle bin
##Sites
Cmdlet|Description
:-----|:----------
**[Set&#8209;PnPAppSideLoading](SetPnPAppSideLoading.md)** |Enables the App SideLoading Feature on a site
**[Get&#8209;PnPAuditing](GetPnPAuditing.md)** |Get the Auditing setting of a site
**[Set&#8209;PnPAuditing](SetPnPAuditing.md)** |Set Auditing setting for a site
**[Get&#8209;PnPSite](GetPnPSite.md)** |Returns the current site collection from the context.
**[Uninstall&#8209;PnPSolution](UninstallPnPSolution.md)** |Uninstalls a sandboxed solution from a site collection
**[Install&#8209;PnPSolution](InstallPnPSolution.md)** |Installs a sandboxed solution to a site collection. WARNING! This method can delete your composed look gallery due to the method used to activate the solution. We recommend you to only to use this cmdlet if you are okay with that.
##Taxonomy
Cmdlet|Description
:-----|:----------
**[Get&#8209;PnPSiteCollectionTermStore](GetPnPSiteCollectionTermStore.md)** |Returns the site collection term store
**[Import&#8209;PnPTaxonomy](ImportPnPTaxonomy.md)** |Imports a taxonomy from either a string array or a file
**[Export&#8209;PnPTaxonomy](ExportPnPTaxonomy.md)** |Exports a taxonomy to either the output or to a file.
**[Set&#8209;PnPTaxonomyFieldValue](SetPnPTaxonomyFieldValue.md)** |Sets a taxonomy term value in a listitem field
**[Remove&#8209;PnPTaxonomyItem](RemovePnPTaxonomyItem.md)** |Removes a taxonomy item
**[Get&#8209;PnPTaxonomyItem](GetPnPTaxonomyItem.md)** |Returns a taxonomy item
**[Get&#8209;PnPTaxonomySession](GetPnPTaxonomySession.md)** |Returns a taxonomy session
**[Remove&#8209;PnPTermGroup](RemovePnPTermGroup.md)** |Removes a taxonomy term group and all its containing termsets
**[New&#8209;PnPTermGroup](NewPnPTermGroup.md)** |Creates a taxonomy term group
**[Get&#8209;PnPTermGroup](GetPnPTermGroup.md)** |Returns a taxonomy term group
**[Import&#8209;PnPTermGroupFromXml](ImportPnPTermGroupFromXml.md)** |Imports a taxonomy TermGroup from either the input or from an XML file.
**[Export&#8209;PnPTermGroupToXml](ExportPnPTermGroupToXml.md)** |Exports a taxonomy TermGroup to either the output or to an XML file.
**[Import&#8209;PnPTermSet](ImportPnPTermSet.md)** |Imports a taxonomy term set from a file in the standard format.
##Tenant Administration
Cmdlet|Description
:-----|:----------
**[Get&#8209;PnPAccessToken](GetPnPAccessToken.md)** |Gets the OAuth 2.0 Access Token to consume the Microsoft Graph API
**[Clear&#8209;PnPTenantRecycleBinItem](ClearPnPTenantRecycleBinItem.md)** |Permanently deletes a site collection from the tenant scoped recycle bin
**[Restore&#8209;PnPTenantRecycleBinItem](RestorePnPTenantRecycleBinItem.md)** |Restores a site collection from the tenant scoped recycle bin
**[Set&#8209;PnPTenantSite](SetPnPTenantSite.md)** |Office365 only: Uses the tenant API to set site information.
**[Get&#8209;PnPTenantSite](GetPnPTenantSite.md)** |Office365 only: Uses the tenant API to retrieve site information.
**[Remove&#8209;PnPTenantSite](RemovePnPTenantSite.md)** |Office365 only: Removes a site collection from the current tenant
**[New&#8209;PnPTenantSite](NewPnPTenantSite.md)** |Creates a new site collection for the current tenant
**[Get&#8209;PnPTimeZoneId](GetPnPTimeZoneId.md)** |Returns a time zone ID
**[Get&#8209;PnPWebTemplates](GetPnPWebTemplates.md)** |Office365 only: Returns the available web templates.
##User and group management
Cmdlet|Description
:-----|:----------
**[Remove&#8209;PnPGroup](RemovePnPGroup.md)** |Removes a group from a web.
**[Set&#8209;PnPGroup](SetPnPGroup.md)** |Updates a group
**[New&#8209;PnPGroup](NewPnPGroup.md)** |Adds group to the Site Groups List and returns a group object
**[Get&#8209;PnPGroup](GetPnPGroup.md)** |Returns a specific group or all groups.
**[Set&#8209;PnPGroupPermissions](SetPnPGroupPermissions.md)** |Adds and/or removes permissions of a specific SharePoint group
**[Get&#8209;PnPGroupPermissions](GetPnPGroupPermissions.md)** |Returns the permissions for a specific SharePoint group
**[New&#8209;PnPUser](NewPnPUser.md)** |Adds a user to the built-in Site User Info List and returns a user object
**[Remove&#8209;PnPUserFromGroup](RemovePnPUserFromGroup.md)** |Removes a user from a group
**[Add&#8209;PnPUserToGroup](AddPnPUserToGroup.md)** |Adds a user to a group
##User Profiles
Cmdlet|Description
:-----|:----------
**[New&#8209;PnPPersonalSite](NewPnPPersonalSite.md)** |Office365 only: Creates a personal / OneDrive For Business site
**[Set&#8209;PnPUserProfileProperty](SetPnPUserProfileProperty.md)** |Office365 only: Uses the tenant API to retrieve site information.  You must connect to the tenant admin website (https://:<tenant>-admin.sharepoint.com) with Connect-PnPOnline in order to use this command.  
**[Get&#8209;PnPUserProfileProperty](GetPnPUserProfileProperty.md)** |You must connect to the tenant admin website (https://:<tenant>-admin.sharepoint.com) with Connect-PnPOnline in order to use this cmdlet.  
##Utilities
Cmdlet|Description
:-----|:----------
**[Send&#8209;PnPMail](SendPnPMail.md)** |Sends an email using the Office 365 SMTP Service or SharePoint, depending on the parameters specified. See detailed help for more information.
##Web Parts
Cmdlet|Description
:-----|:----------
**[Remove&#8209;PnPWebPart](RemovePnPWebPart.md)** |Removes a webpart from a page
**[Get&#8209;PnPWebPart](GetPnPWebPart.md)** |Returns a webpart definition object
**[Get&#8209;PnPWebPartProperty](GetPnPWebPartProperty.md)** |Returns a web part property
**[Set&#8209;PnPWebPartProperty](SetPnPWebPartProperty.md)** |Sets a web part property
**[Add&#8209;PnPWebPartToWebPartPage](AddPnPWebPartToWebPartPage.md)** |Adds a webpart to a web part page in a specified zone
**[Add&#8209;PnPWebPartToWikiPage](AddPnPWebPartToWikiPage.md)** |Adds a webpart to a wiki page in a specified table row and column
**[Get&#8209;PnPWebPartXml](GetPnPWebPartXml.md)** |Returns the webpart XML of a webpart registered on a site
##Webs
Cmdlet|Description
:-----|:----------
**[Set&#8209;PnPIndexedProperties](SetPnPIndexedProperties.md)** |Marks values of the propertybag to be indexed by search. Notice that this will overwrite the existing flags, i.e. only the properties you define with the cmdlet will be indexed.
**[Remove&#8209;PnPIndexedProperty](RemovePnPIndexedProperty.md)** |Removes a key from propertybag to be indexed by search. The key and it's value remain in the propertybag, however it will not be indexed anymore.
**[Add&#8209;PnPIndexedProperty](AddPnPIndexedProperty.md)** |Marks the value of the propertybag key specified to be indexed by search.
**[Get&#8209;PnPIndexedPropertyKeys](GetPnPIndexedPropertyKeys.md)** |Returns the keys of the property bag values that have been marked for indexing by search
**[Get&#8209;PnPPropertyBag](GetPnPPropertyBag.md)** |Returns the property bag values.
**[Remove&#8209;PnPPropertyBagValue](RemovePnPPropertyBagValue.md)** |Removes a value from the property bag
**[Set&#8209;PnPPropertyBagValue](SetPnPPropertyBagValue.md)** |Sets a property bag value
**[Request&#8209;PnPReIndexWeb](RequestPnPReIndexWeb.md)** |Marks the web for full indexing during the next incremental crawl
**[Get&#8209;PnPRequestAccessEmails](GetPnPRequestAccessEmails.md)** |Returns the request access e-mail addresses
**[Set&#8209;PnPRequestAccessEmails](SetPnPRequestAccessEmails.md)** |Sets Request Access Emails on a web
**[Get&#8209;PnPSubWebs](GetPnPSubWebs.md)** |Returns the subwebs of the current web
**[Remove&#8209;PnPWeb](RemovePnPWeb.md)** |Removes a subweb in the current web
**[Get&#8209;PnPWeb](GetPnPWeb.md)** |Returns the current web object
**[New&#8209;PnPWeb](NewPnPWeb.md)** |Creates a new subweb under the current web
**[Set&#8209;PnPWeb](SetPnPWeb.md)** |Sets properties on a web
**[Invoke&#8209;PnPWebAction](InvokePnPWebAction.md)** |Executes operations on web, lists and list items.
##Workflows
Cmdlet|Description
:-----|:----------
**[Add&#8209;PnPWorkflowDefinition](AddPnPWorkflowDefinition.md)** |Adds a workflow definition
**[Remove&#8209;PnPWorkflowDefinition](RemovePnPWorkflowDefinition.md)** |Removes a workflow definition
**[Get&#8209;PnPWorkflowDefinition](GetPnPWorkflowDefinition.md)** |Returns a workflow definition
**[Resume&#8209;PnPWorkflowInstance](ResumePnPWorkflowInstance.md)** |Resumes a previously stopped workflow instance
**[Stop&#8209;PnPWorkflowInstance](StopPnPWorkflowInstance.md)** |Stops a workflow instance
**[Remove&#8209;PnPWorkflowSubscription](RemovePnPWorkflowSubscription.md)** |Removes a workflow subscription
**[Add&#8209;PnPWorkflowSubscription](AddPnPWorkflowSubscription.md)** |Adds a workflow subscription to a list
**[Get&#8209;PnPWorkflowSubscription](GetPnPWorkflowSubscription.md)** |Returns a workflow subscriptions from a list
