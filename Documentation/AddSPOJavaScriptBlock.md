#Add-SPOJavaScriptBlock
*Topic automatically generated on: 2015-10-19*

Adds a link to a JavaScript snippet/block to a web or site collection
##Syntax
```powershell
Add-SPOJavaScriptBlock -Name <String> -Script <String> [-Sequence <Int32>] [-Scope <CustomActionScope>] [-Web <WebPipeBind>]
```


##Detailed Description
Specify a scope as 'Site' to add the custom action to all sites in a site collection.

##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Name|String|True|The name of the script block. Can be used to identiy the script with other cmdlets or coded solutions|
|Scope|CustomActionScope|False|The scope of the script to add to. Either Web or Site, defaults to Web.|
|Script|String|True|The javascript block to add|
|Sequence|Int32|False||
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
Adds a link to a JavaScript snippet/block to a web or site collection
##Syntax
```powershell
Add-SPOJavaScriptBlock -Name <String> -Script <String> [-Sequence <Int32>] [-Scope <CustomActionScope>] [-Web <WebPipeBind>]
```


##Detailed Description
Specify a scope as 'Site' to add the custom action to all sites in a site collection.

##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Name|String|True|The name of the script block. Can be used to identiy the script with other cmdlets or coded solutions|
|Scope|CustomActionScope|False|The scope of the script to add to. Either Web or Site, defaults to Web.|
|Script|String|True|The javascript block to add|
|Sequence|Int32|False||
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
