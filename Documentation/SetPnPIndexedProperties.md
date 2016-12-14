#Set-PnPIndexedProperties
Marks values of the propertybag to be indexed by search. Notice that this will overwrite the existing flags, i.e. only the properties you define with the cmdlet will be indexed.
##Syntax
```powershell
Set-PnPIndexedProperties -Keys <List`1>
                         [-Web <WebPipeBind>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Keys|List`1|True||
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
