#Remove-PnPWebPart
Removes a webpart from a page
##Syntax
```powershell
Remove-PnPWebPart -Identity <GuidPipeBind>
                  -ServerRelativePageUrl <String>
                  [-Web <WebPipeBind>]
```


```powershell
Remove-PnPWebPart -Title <String>
                  -ServerRelativePageUrl <String>
                  [-Web <WebPipeBind>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|GuidPipeBind|True||
|ServerRelativePageUrl|String|True||
|Title|String|True||
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
