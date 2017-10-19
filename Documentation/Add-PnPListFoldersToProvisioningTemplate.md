---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Add-PnPListFoldersToProvisioningTemplate

## SYNOPSIS
Adds folders to a list in a PnP Provisioning Template

## SYNTAX 

```powershell
Add-PnPListFoldersToProvisioningTemplate -Path <String>
                                         -List <ListPipeBind>
                                         [-Web <WebPipeBind>]
                                         [-Recursive [<SwitchParameter>]]
                                         [-IncludeSecurity [<SwitchParameter>]]
                                         [-TemplateProviderExtensions <ITemplateProviderExtension[]>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Add-PnPListFoldersToProvisioningTemplate -Path template.pnp -List 'PnPTestList'
```

Adds top level folders from a list to an existing template and returns an in-memory PnP Provisioning Template

### ------------------EXAMPLE 2------------------
```powershell
PS:> Add-PnPListFoldersToProvisioningTemplate -Path template.pnp -List 'PnPTestList' -Recursive
```

Adds all folders from a list to an existing template and returns an in-memory PnP Provisioning Template

### ------------------EXAMPLE 3------------------
```powershell
PS:> Add-PnPListFoldersToProvisioningTemplate -Path template.pnp -List 'PnPTestList' -Recursive -IncludeSecurity
```

Adds all folders from a list with unique permissions to an in-memory PnP Provisioning Template

## PARAMETERS

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
Position: 2
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

### -Recursive
A switch parameter to include all folders in the list, or just top level folders.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: 4
Accept pipeline input: False
```

### -TemplateProviderExtensions
Allows you to specify ITemplateProviderExtension to execute while loading the template.

```yaml
Type: ITemplateProviderExtension[]
Parameter Sets: (All)

Required: False
Position: 6
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