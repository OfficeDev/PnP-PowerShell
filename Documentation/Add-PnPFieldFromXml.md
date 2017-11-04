---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Add-PnPFieldFromXml

## SYNOPSIS
Adds a field to a list or as a site column based upon a CAML/XML field definition

## SYNTAX 

```powershell
Add-PnPFieldFromXml -FieldXml <String>
                    [-List <ListPipeBind>]
                    [-Web <WebPipeBind>]
                    [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> $xml = '<Field Type="Text" Name="PSCmdletTest" DisplayName="PSCmdletTest" ID="{27d81055-f208-41c9-a976-61c5473eed4a}" Group="Test" Required="FALSE" StaticName="PSCmdletTest" />'
PS:> Add-PnPFieldFromXml -FieldXml $xml
```

Adds a field with the specified field CAML code to the site.

### ------------------EXAMPLE 2------------------
```powershell
PS:> $xml = '<Field Type="Text" Name="PSCmdletTest" DisplayName="PSCmdletTest" ID="{27d81055-f208-41c9-a976-61c5473eed4a}" Group="Test" Required="FALSE" StaticName="PSCmdletTest" />'
PS:> Add-PnPFieldFromXml -List "Demo List" -FieldXml $xml
```

Adds a field with the specified field CAML code to the list "Demo List".

## PARAMETERS

### -FieldXml
CAML snippet containing the field definition. See http://msdn.microsoft.com/en-us/library/office/ms437580(v=office.15).aspx

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: 0
Accept pipeline input: False
```

### -List
The name of the list, its ID or an actual list object where this field needs to be added

```yaml
Type: ListPipeBind
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: True
```

### -Connection
Optional connection to be used by cmdlet. Retrieve the value for this parameter by eiter specifying -ReturnConnection on Connect-PnPOnline or by executing Get-PnPConnection.

```yaml
Type: SPOnlineConnection
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Web
The GUID, server relative url (i.e. /sites/team1) or web instance of the web to apply the command to. Omit this parameter to use the current web.

```yaml
Type: WebPipeBind
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

## OUTPUTS

### [Microsoft.SharePoint.Client.Field](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.field.aspx)

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)[Field CAML](http://msdn.microsoft.com/en-us/library/office/ms437580(v=office.15).aspx)