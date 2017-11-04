---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Set-PnPMasterPage

## SYNOPSIS
Set the masterpage

## SYNTAX 

### Server Relative
```powershell
Set-PnPMasterPage [-MasterPageServerRelativeUrl <String>]
                  [-CustomMasterPageServerRelativeUrl <String>]
                  [-Web <WebPipeBind>]
                  [-Connection <SPOnlineConnection>]
```

### Site Relative
```powershell
Set-PnPMasterPage [-MasterPageSiteRelativeUrl <String>]
                  [-CustomMasterPageSiteRelativeUrl <String>]
                  [-Web <WebPipeBind>]
                  [-Connection <SPOnlineConnection>]
```

## DESCRIPTION
Sets the default master page of the current web.

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Set-PnPMasterPage -MasterPageServerRelativeUrl /sites/projects/_catalogs/masterpage/oslo.master
```

Sets the master page based on a server relative URL

### ------------------EXAMPLE 2------------------
```powershell
PS:> Set-PnPMasterPage -MasterPageServerRelativeUrl /sites/projects/_catalogs/masterpage/oslo.master -CustomMasterPageServerRelativeUrl /sites/projects/_catalogs/masterpage/oslo.master
```

Sets the master page and custom master page based on a server relative URL

### ------------------EXAMPLE 3------------------
```powershell
PS:> Set-PnPMasterPage -MasterPageSiteRelativeUrl _catalogs/masterpage/oslo.master
```

Sets the master page based on a site relative URL

### ------------------EXAMPLE 4------------------
```powershell
PS:> Set-PnPMasterPage -MasterPageSiteRelativeUrl _catalogs/masterpage/oslo.master -CustomMasterPageSiteRelativeUrl _catalogs/masterpage/oslo.master
```

Sets the master page and custom master page based on a site relative URL

## PARAMETERS

### -CustomMasterPageServerRelativeUrl
Specifies the custom Master page URL based on the server relative URL

```yaml
Type: String
Parameter Sets: Server Relative
Aliases: CustomMasterPageUrl

Required: False
Position: Named
Accept pipeline input: False
```

### -CustomMasterPageSiteRelativeUrl
Specifies the custom Master page URL based on the site relative URL

```yaml
Type: String
Parameter Sets: Site Relative

Required: False
Position: Named
Accept pipeline input: False
```

### -MasterPageServerRelativeUrl
Specifies the Master page URL based on the server relative URL

```yaml
Type: String
Parameter Sets: Server Relative
Aliases: MasterPageUrl

Required: False
Position: Named
Accept pipeline input: False
```

### -MasterPageSiteRelativeUrl
Specifies the Master page URL based on the site relative URL

```yaml
Type: String
Parameter Sets: Site Relative

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