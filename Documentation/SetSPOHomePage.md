#Set-SPOHomePage
*Topic automatically generated on: 2015-10-13*

Sets the home page of the current web.
##Syntax
```powershell
Set-SPOHomePage [-Web <WebPipeBind>] -RootFolderRelativeUrl <String>
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|RootFolderRelativeUrl|String|True|The root folder relative url of the homepage, e.g. 'sitepages/home.aspx'|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell

    PS:> Set-SPOHomePage -RootFolderRelativeUrl SitePages/Home.aspx

```
Sets the home page to the home.aspx file which resides in the SitePages library
