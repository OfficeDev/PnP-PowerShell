#Set-SPOMasterPage
*Topic automatically generated on: 2015-10-13*

Sets the default master page of the current web.
##Syntax
```powershell
Set-SPOMasterPage [-MasterPageServerRelativeUrl <String>] [-CustomMasterPageServerRelativeUrl <String>] [-Web <WebPipeBind>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|CustomMasterPageServerRelativeUrl|String|False||
|MasterPageServerRelativeUrl|String|False||
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell

    PS:> Set-SPOMasterPage -MasterPageServerRelativeUrl /sites/projects/_catalogs/masterpage/oslo.master

```

