#Set-PnPAuditing
Set Auditing setting for a site
##Syntax
```powershell
Set-PnPAuditing [-DisableAll [<SwitchParameter>]]
```


```powershell
Set-PnPAuditing [-EnableAll [<SwitchParameter>]]
                [-RetentionTime <Int32>]
                [-TrimAuditLog [<SwitchParameter>]]
```


```powershell
Set-PnPAuditing [-RetentionTime <Int32>]
                [-TrimAuditLog [<SwitchParameter>]]
                [-EditItems [<SwitchParameter>]]
                [-CheckOutCheckInItems [<SwitchParameter>]]
                [-MoveCopyItems [<SwitchParameter>]]
                [-DeleteRestoreItems [<SwitchParameter>]]
                [-EditContentTypesColumns [<SwitchParameter>]]
                [-SearchContent [<SwitchParameter>]]
                [-EditUsersPermissions [<SwitchParameter>]]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|CheckOutCheckInItems|SwitchParameter|False||
|DeleteRestoreItems|SwitchParameter|False||
|DisableAll|SwitchParameter|False||
|EditContentTypesColumns|SwitchParameter|False||
|EditItems|SwitchParameter|False||
|EditUsersPermissions|SwitchParameter|False||
|EnableAll|SwitchParameter|False||
|MoveCopyItems|SwitchParameter|False||
|RetentionTime|Int32|False||
|SearchContent|SwitchParameter|False||
|TrimAuditLog|SwitchParameter|False||
##Examples

###Example 1
```powershell
PS:> Set-PnPAuditing -EnableAll
```
Enables all auditing settings for the current site

###Example 2
```powershell
PS:> Set-PnPAuditing -DisableAll
```
Disables all auditing settings for the current site
                    This also disables the automatic trimming of the audit log

###Example 3
```powershell
PS:> Set-PnPAuditing -RetentionTime 7
```
Sets the audit log trimming to 7 days, this also enables the automatic trimming of the audit log

###Example 4
```powershell
PS:> Set-PnPAuditing -TrimAuditLog
```
Enables the automatic trimming of the audit log

###Example 5
```powershell
PS:> Set-PnPAuditing -RetentionTime 7 -CheckOutCheckInItems -MoveCopyItems -SearchContent
```
Sets the audit log trimming to 7 days, this also enables the automatic trimming of the audit log.
                    Do auditing for:
                    - Checking out or checking in items
                    - Moving or copying items to another location in the site
                    - Searching site content
