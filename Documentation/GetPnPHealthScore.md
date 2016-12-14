#Get-PnPHealthScore
Retrieves the current health score value of the server
##Syntax
##Returns
>System.Int32

Returns a int value representing the current health score value of the server.

##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
##Examples

###Example 1
```powershell
PS:> Get-PnPHealthScore
```
This will retrieve the current health score of the server.

###Example 2
```powershell
PS:> Get-PnPHealthScore -Url https://contoso.sharepoint.com
```
This will retrieve the current health score for the url https://contoso.sharepoint.com.
