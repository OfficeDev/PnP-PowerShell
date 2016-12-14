#Request-PnPReIndexList
Marks the list for full indexing during the next incremental crawl
##Syntax
```powershell
Request-PnPReIndexList [-Web <WebPipeBind>]
                       -Identity <ListPipeBind>
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|ListPipeBind|True|The ID, Title or Url of the list.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Request-PnPReIndexList -Identity "Demo List"
```

