#Execute-SPOQuery
Executes any queued actions / changes on the SharePoint Client Side Object Model Context
##Syntax
```powershell
Execute-SPOQuery [-RetryCount <Int32>] [-RetryWait <Int32>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|RetryCount|Int32|False|Number to times to retry in case of throttling. Defaults to 10.|
|RetryWait|Int32|False|Delay in seconds. Defaults to 1.|
