#Set-SPOWebPartProperty
*Topic automatically generated on: 2015-10-13*

Sets a web part property
##Syntax
```powershell
Set-SPOWebPartProperty -ServerRelativePageUrl <String> -Identity <GuidPipeBind> -Key <String> -Value <PSObject> [-Web <WebPipeBind>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|GuidPipeBind|True||
|Key|String|True||
|ServerRelativePageUrl|String|True||
|Value|PSObject|True||
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
