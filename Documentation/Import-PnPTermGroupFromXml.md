---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Import-PnPTermGroupFromXml

## SYNOPSIS
Imports a taxonomy TermGroup from either the input or from an XML file.

## SYNTAX 

### XML
```powershell
Import-PnPTermGroupFromXml [-Xml <String>]
                           [-Connection <SPOnlineConnection>]
```

### File
```powershell
Import-PnPTermGroupFromXml [-Path <String>]
                           [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Import-PnPTermGroupFromXml -Xml $xml
```

Imports the XML based termgroup information located in the $xml variable

### ------------------EXAMPLE 2------------------
```powershell
PS:> Import-PnPTermGroupFromXml -Path input.xml
```

Imports the XML file specified by the path.

## PARAMETERS

### -Path
The XML File to import the data from

```yaml
Type: String
Parameter Sets: File

Required: False
Position: Named
Accept pipeline input: False
```

### -Xml
The XML to process

```yaml
Type: String
Parameter Sets: XML

Required: False
Position: 0
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

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)