# Execute-PnPQuery
Executes any queued actions / changes on the SharePoint Client Side Object Model Context
## Syntax
```powershell
Execute-PnPQuery [-RetryCount <Int>]
                 [-RetryWait <Int>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|RetryCount|Int|False|Number of times to retry in case of throttling. Defaults to 10.|
|RetryWait|Int|False|Delay in seconds. Defaults to 1.|
## Examples

### Example 1
```powershell
PS:> Execute-PnPQuery -RetryCount 5
```
This will execute any queued actions / changes on the SharePoint Client Side Object Model Context and will retry 5 times in case of throttling.

### Example 2
```powershell
PS:> Execute-PnPQuery -RetryWait 10
```
This will execute any queued actions / changes on the SharePoint Client Side Object Model Context and delay the execution for 10 seconds before it retries the execution.
