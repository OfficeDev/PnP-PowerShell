---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Add-PnPDataRowsToProvisioningTemplate

## SYNOPSIS
Adds datarows to a list inside a PnP Provisioning Template

## SYNTAX 

```powershell
Add-PnPDataRowsToProvisioningTemplate -List <ListPipeBind>
                                      -Query <String>
                                      -Path <String>
                                      [-Fields <String[]>]
                                      [-TemplateProviderExtensions <ITemplateProviderExtension[]>]
                                      [-IncludeSecurity [<SwitchParameter>]]
                                      [-Web <WebPipeBind>]
                                      [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Add-PnPDataRowsToProvisioningTemplate -Path template.pnp -List 'PnPTestList' -Query '<View></View>' -Fields 'Title','Choice'
```

Adds datarows to a list in an in-memory PnP Provisioning Template

### ------------------EXAMPLE 2------------------
```powershell
PS:> Add-PnPDataRowsToProvisioningTemplate -Path template.pnp -List 'PnPTestList' -Query '<View></View>' -Fields 'Title','Choice' -IncludeSecurity
```

Adds datarows to a list in an in-memory PnP Provisioning Template

## PARAMETERS

### -Fields
The fields to retrieve. If not specified all fields will be loaded in the returned list object.

```yaml
Type: String[]
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -IncludeSecurity
A switch to include ObjectSecurity information.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: 5
Accept pipeline input: False
```

### -List
The list to query

```yaml
Type: ListPipeBind
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -Path
Filename of the .PNP Open XML provisioning template to read from, optionally including full path.

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: 0
Accept pipeline input: False
```

### -Query
The CAML query to execute against the list

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -TemplateProviderExtensions
Allows you to specify ITemplateProviderExtension to execute while loading the template.

```yaml
Type: ITemplateProviderExtension[]
Parameter Sets: (All)

Required: False
Position: 4
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