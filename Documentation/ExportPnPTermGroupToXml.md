#Export-PnPTermGroupToXml
Exports a taxonomy TermGroup to either the output or to an XML file.
##Syntax
```powershell
Export-PnPTermGroupToXml [-Identity <TermGroupPipeBind>]
                         [-Out <String>]
                         [-FullTemplate [<SwitchParameter>]]
                         [-Encoding <Encoding>]
                         [-Force [<SwitchParameter>]]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Encoding|Encoding|False|Defaults to Unicode|
|Force|SwitchParameter|False|Overwrites the output file if it exists.|
|FullTemplate|SwitchParameter|False|If specified, a full provisioning template structure will be returned|
|Identity|TermGroupPipeBind|False|The ID or name of the termgroup|
|Out|String|False|File to export the data to.|
##Examples

###Example 1
```powershell
PS:> Export-PnPTermGroupToXml
```
Exports all term groups in the default site collection term store to the standard output

###Example 2
```powershell
PS:> Export-PnPTermGroupToXml -Out output.xml
```
Exports all term groups in the default site collection term store to the file 'output.xml' in the current folder

###Example 3
```powershell
PS:> Export-PnPTermGroupToXml -Out c:\output.xml -Identity "Test Group"
```
Exports the term group with the specified name to the file 'output.xml' located in the root folder of the C: drive.

###Example 4
```powershell
PS:> $termgroup = Get-PnPTermGroup -GroupName Test
PS:> $termgroup | Export-PnPTermGroupToXml -Out c:\output.xml
```
Retrieves a termgroup and subsequently exports that term group to a the file named 'output.xml'
