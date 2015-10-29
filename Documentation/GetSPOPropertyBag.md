#Get-SPOPropertyBag
*Topic automatically generated on: 2015-10-13*

Returns the property bag values.
##Syntax
```powershell
Get-SPOPropertyBag [-Folder <String>] [-Web <WebPipeBind>] [-Key <String>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Folder|String|False|Site relative url of the folder. See examples for use.|
|Key|String|False||
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Get-SPOPropertyBag
```
This will return all web property bag values

###Example 2
```powershell
PS:> Get-SPOPropertyBag -Key MyKey
```
This will return the value of the key MyKey from the web property bag

###Example 3
```powershell
PS:> Get-SPOPropertyBag -Folder /MyFolder
```
This will return all property bag values for the folder MyFolder which is located in the root of the current web

###Example 4
```powershell
PS:> Get-SPOPropertyBag -Folder /MyFolder -Key vti_mykey
```
This will return the value of the key vti_mykey from the folder MyFolder which is located in the root of the current web

###Example 5
```powershell
PS:> Get-SPOPropertyBag -Folder / -Key vti_mykey
```
This will return the value of the key vti_mykey from the root folder of the current web
