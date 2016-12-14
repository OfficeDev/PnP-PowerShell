#Set-PnPMasterPage
Sets the default master page of the current web.
##Syntax
```powershell
Set-PnPMasterPage [-MasterPageServerRelativeUrl <String>]
                  [-CustomMasterPageServerRelativeUrl <String>]
                  [-Web <WebPipeBind>]
```


```powershell
Set-PnPMasterPage [-MasterPageSiteRelativeUrl <String>]
                  [-CustomMasterPageSiteRelativeUrl <String>]
                  [-Web <WebPipeBind>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|CustomMasterPageServerRelativeUrl|String|False|Specifies the custom Master page URL based on the server relative URL|
|CustomMasterPageSiteRelativeUrl|String|False|Specifies the custom Master page URL based on the site relative URL|
|MasterPageServerRelativeUrl|String|False|Specifies the Master page URL based on the server relative URL|
|MasterPageSiteRelativeUrl|String|False|Specifies the Master page URL based on the site relative URL|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Set-PnPMasterPage -MasterPageServerRelativeUrl /sites/projects/_catalogs/masterpage/oslo.master
```
Sets the master page based on a server relative URL

###Example 2
```powershell
PS:> Set-PnPMasterPage -MasterPageServerRelativeUrl /sites/projects/_catalogs/masterpage/oslo.master -CustomMasterPageServerRelativeUrl /sites/projects/_catalogs/masterpage/oslo.master
```
Sets the master page and custom master page based on a server relative URL

###Example 3
```powershell
PS:> Set-PnPMasterPage -MasterPageSiteRelativeUrl _catalogs/masterpage/oslo.master
```
Sets the master page based on a site relative URL

###Example 4
```powershell
PS:> Set-PnPMasterPage -MasterPageSiteRelativeUrl _catalogs/masterpage/oslo.master -CustomMasterPageSiteRelativeUrl _catalogs/masterpage/oslo.master
```
Sets the master page and custom master page based on a site relative URL
