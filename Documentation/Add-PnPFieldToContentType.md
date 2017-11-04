---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Add-PnPFieldToContentType

## SYNOPSIS
Adds an existing site column to a content type

## SYNTAX 

```powershell
Add-PnPFieldToContentType -Field <FieldPipeBind>
                          -ContentType <ContentTypePipeBind>
                          [-Required [<SwitchParameter>]]
                          [-Hidden [<SwitchParameter>]]
                          [-Web <WebPipeBind>]
                          [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Add-PnPFieldToContentType -Field "Project_Name" -ContentType "Project Document"
```

This will add an existing site column with an internal name of "Project_Name" to a content type called "Project Document"

## PARAMETERS

### -ContentType
Specifies which content type a field needs to be added to

```yaml
Type: ContentTypePipeBind
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -Field
Specifies the field that needs to be added to the content type

```yaml
Type: FieldPipeBind
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -Hidden
Specifies whether the field should be hidden or not

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Required
Specifies whether the field is required or not

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