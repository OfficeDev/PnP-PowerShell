---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Uninstall-PnPAppInstance

## SYNOPSIS
Removes an app from a site

## SYNTAX 

```powershell
Uninstall-PnPAppInstance -Identity <AppPipeBind>
                         [-Force [<SwitchParameter>]]
                         [-Web <WebPipeBind>]
                         [-Connection <SPOnlineConnection>]
```

## DESCRIPTION
Removes an add-in/app that has been installed to a site.

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Uninstall-PnPAppInstance -Identity $appinstance
```

Uninstalls the app instance which was retrieved with the command Get-PnPAppInstance

### ------------------EXAMPLE 2------------------
```powershell
PS:> Uninstall-PnPAppInstance -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe
```

Uninstalls the app instance with the ID '99a00f6e-fb81-4dc7-8eac-e09c6f9132fe'

### ------------------EXAMPLE 3------------------
```powershell
PS:> Uninstall-PnPAppInstance -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe -force
```

Uninstalls the app instance with the ID '99a00f6e-fb81-4dc7-8eac-e09c6f9132fe' and do not ask for confirmation

## PARAMETERS

### -Force
Do not ask for confirmation.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Identity
Appinstance or Id of the addin to remove.

```yaml
Type: AppPipeBind
Parameter Sets: (All)

Required: True
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

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)