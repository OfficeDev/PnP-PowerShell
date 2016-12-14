#Add-PnPFieldFromXml
Adds a field to a list or as a site column based upon a CAML/XML field definition
##Syntax
```powershell
Add-PnPFieldFromXml [-List <ListPipeBind>]
                    [-Web <WebPipeBind>]
                    -FieldXml <String>
```


##Returns
>[Microsoft.SharePoint.Client.Field](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.field.aspx)

##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|FieldXml|String|True|CAML snippet containing the field definition. See http://msdn.microsoft.com/en-us/library/office/ms437580(v=office.15).aspx|
|List|ListPipeBind|False|The name of the list, its ID or an actual list object where this field needs to be added|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> $xml = '<Field Type="Text" Name="PSCmdletTest" DisplayName="PSCmdletTest" ID="{27d81055-f208-41c9-a976-61c5473eed4a}" Group="Test" Required="FALSE" StaticName="PSCmdletTest" />'
PS:> Add-PnPFieldFromXml -FieldXml $xml
```
Adds a field with the specified field CAML code to the site.

###Example 2
```powershell
PS:> $xml = '<Field Type="Text" Name="PSCmdletTest" DisplayName="PSCmdletTest" ID="{27d81055-f208-41c9-a976-61c5473eed4a}" Group="Test" Required="FALSE" StaticName="PSCmdletTest" />'
PS:> Add-PnPFieldFromXml -List "Demo List" -FieldXml $xml
```
Adds a field with the specified field CAML code to the list "Demo List".
