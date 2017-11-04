---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Remove-PnPFolder

## SYNOPSIS
Deletes a folder within a parent folder

## SYNTAX 

```powershell
Remove-PnPFolder -Name <String>
                 -Folder <String>
                 [-Recycle [<SwitchParameter>]]
                 [-Force [<SwitchParameter>]]
                 [-Web <WebPipeBind>]
                 [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Remove-PnPFolder -Name NewFolder -Folder _catalogs/masterpage
```

Removes the folder 'NewFolder' from '_catalogsmasterpage'

### ------------------EXAMPLE 2------------------
```powershell
PS:> Remove-PnPFolder -Name NewFolder -Folder _catalogs/masterpage -Recycle
```

Removes the folder 'NewFolder' from '_catalogsmasterpage' and is saved in the Recycle Bin

## PARAMETERS

### -Folder
The parent folder in the site

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -Force


```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Name
The folder name

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -Recycle


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