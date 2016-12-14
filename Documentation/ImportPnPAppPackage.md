#Import-PnPAppPackage
Adds a SharePoint Addin to a site
##Syntax
```powershell
Import-PnPAppPackage -Path <String>
                     [-Force [<SwitchParameter>]]
                     [-LoadOnly [<SwitchParameter>]]
                     [-Locale <Int32>]
                     [-Web <WebPipeBind>]
```


##Detailed Description
This commands requires that you have an addin package to deploy

##Returns
>[Microsoft.SharePoint.Client.AppInstance](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.appinstance.aspx)

##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Force|SwitchParameter|False|Will forcibly install the app by activating the addin sideloading feature, installing the addin, and deactivating the sideloading feature|
|LoadOnly|SwitchParameter|False|Will only upload the addin, but not install it|
|Locale|Int32|False|Will install the addin for the specified locale|
|Path|String|True|Path pointing to the .app file|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Import-PnPAppPackage -Path c:\files\demo.app -LoadOnly
```
This will load the addin in the demo.app package, but will not install it to the site.
 

###Example 2
```powershell
PS:> Import-PnPAppPackage -Path c:\files\demo.app -Force
```
This load first activate the addin sideloading feature, upload and install the addin, and deactivate the addin sideloading feature.
    
