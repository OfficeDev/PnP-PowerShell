---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Set-PnPDocumentSetField

## SYNOPSIS
Sets a site column from the available content types to a document set

## SYNTAX 

```powershell
Set-PnPDocumentSetField -DocumentSet <DocumentSetPipeBind>
                        -Field <FieldPipeBind>
                        [-SetSharedField [<SwitchParameter>]]
                        [-SetWelcomePageField [<SwitchParameter>]]
                        [-RemoveSharedField [<SwitchParameter>]]
                        [-RemoveWelcomePageField [<SwitchParameter>]]
                        [-Web <WebPipeBind>]
                        [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Set-PnPDocumentSetField -Field "Test Field" -DocumentSet "Test Document Set" -SetSharedField -SetWelcomePageField
```

This will set the field, available in one of the available content types, as a Shared Field and as a Welcome Page Field.

### ------------------EXAMPLE 2------------------
```powershell
PS:> Set-PnPDocumentSetField -Field "Test Field" -DocumentSet "Test Document Set" -RemoveSharedField -RemoveWelcomePageField
```

This will remove the field, available in one of the available content types, as a Shared Field and as a Welcome Page Field.

## PARAMETERS

### -DocumentSet
The document set in which to set the field. Either specify a name, a document set template object, an id, or a content type object

```yaml
Type: DocumentSetPipeBind
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -Field
The field to set. The field needs to be available in one of the available content types. Either specify a name, an id or a field object

```yaml
Type: FieldPipeBind
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -RemoveSharedField
Removes the field as a Shared Field

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -RemoveWelcomePageField
Removes the field as a Welcome Page Field

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -SetSharedField
Set the field as a Shared Field

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -SetWelcomePageField
Set the field as a Welcome Page field

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
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

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)