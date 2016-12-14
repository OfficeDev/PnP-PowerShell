#Set-PnPDefaultPageLayout
Sets a specific page layout to be the default page layout for a publishing site
##Syntax
```powershell
Set-PnPDefaultPageLayout -InheritFromParentSite [<SwitchParameter>]
                         [-Web <WebPipeBind>]
```


```powershell
Set-PnPDefaultPageLayout -Title <String>
                         [-Web <WebPipeBind>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|InheritFromParentSite|SwitchParameter|True|Set the default page layout to be inherited from the parent site.|
|Title|String|True|Title of the page layout|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Set-PnPDefaultPageLayout -Title projectpage.aspx
```
Sets projectpage.aspx to be the default page layout for the current web

###Example 2
```powershell
PS:> Set-PnPDefaultPageLayout -Title test/testpage.aspx
```
Sets a page layout in a folder in the Master Page & Page Layout gallery, such as _catalog/masterpage/test/testpage.aspx, to be the default page layout for the current web

###Example 3
```powershell
PS:> Set-PnPDefaultPageLayout -InheritFromParentSite
```
Sets the default page layout to be inherited from the parent site
