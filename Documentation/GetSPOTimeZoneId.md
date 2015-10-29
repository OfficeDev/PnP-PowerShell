#Get-SPOTimeZoneId
*Topic automatically generated on: 2015-10-13*

Returns a time zone ID
##Syntax
```powershell
Get-SPOTimeZoneId [-Match <String>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Match|String|False||
##Examples

###Example 1
```powershell
PS:> Get-SPOTimeZoneId
```
This will return all time zone IDs in use by Office 365.
 

###Example 2
```powershell
PS:> Get-SPOTimeZoneId -Match Stockholm
```
This will return the time zone IDs for Stockholm
    
