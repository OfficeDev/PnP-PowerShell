#Get-PnPJavaScriptLink
Returns all or a specific custom action(s) with location type ScriptLink
##Syntax
```powershell
Get-PnPJavaScriptLink [-Scope <CustomActionScope>]
                      [-Web <WebPipeBind>]
                      [-Name <String>]
```


##Returns
>[Microsoft.SharePoint.Client.UserCustomAction](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.usercustomaction.aspx)

##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Name|String|False|Name of the Javascript link. Omit this parameter to retrieve all script links|
|Scope|CustomActionScope|False|Scope of the action, either Web, Site or All to return both, defaults to Web|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Get-PnPJavaScriptLink
```
Returns all web scoped JavaScript links

###Example 2
```powershell
PS:> Get-PnPJavaScriptLink -Scope All
```
Returns all web and site scoped JavaScript links

###Example 3
```powershell
PS:> Get-PnPJavaScriptLink -Scope Web
```
Returns all Web scoped JavaScript links

###Example 4
```powershell
PS:> Get-PnPJavaScriptLink -Scope Site
```
Returns all Site scoped JavaScript links

###Example 5
```powershell
PS:> Get-PnPJavaScriptLink -Name Test
```
Returns the web scoped JavaScript link named Test
