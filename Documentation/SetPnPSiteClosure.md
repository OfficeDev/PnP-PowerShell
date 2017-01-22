#Set-PnPSiteClosure
Opens or closes a site which has a site policy applied
##Syntax
```powershell
Set-PnPSiteClosure -State <ClosureState>
                   [-Web <WebPipeBind>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|State|ClosureState|True|The state of the site|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Set-PnPSiteClosure -State Open
```
This opens a site which has been closed and has a site policy applied.

###Example 2
```powershell
PS:> Set-PnPSiteClosure -State Closed
```
This closes a site which is open and has a site policy applied.
