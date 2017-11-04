---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Add-PnPJavaScriptLink

## SYNOPSIS
Adds a link to a JavaScript file to a web or sitecollection

## SYNTAX 

```powershell
Add-PnPJavaScriptLink -Name <String>
                      -Url <String[]>
                      [-Sequence <Int>]
                      [-Scope <CustomActionScope>]
                      [-Web <WebPipeBind>]
                      [-Connection <SPOnlineConnection>]
```

## DESCRIPTION
Creates a custom action that refers to a JavaScript file

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Add-PnPJavaScriptLink -Name jQuery -Url https://code.jquery.com/jquery.min.js -Sequence 9999 -Scope Site
```

Injects a reference to the latest v1 series jQuery library to all pages within the current site collection under the name jQuery and at order 9999

### ------------------EXAMPLE 2------------------
```powershell
PS:> Add-PnPJavaScriptLink -Name jQuery -Url https://code.jquery.com/jquery.min.js
```

Injects a reference to the latest v1 series jQuery library to all pages within the current web under the name jQuery

## PARAMETERS

### -Name
Name under which to register the JavaScriptLink

```yaml
Type: String
Parameter Sets: (All)
Aliases: Key

Required: True
Position: Named
Accept pipeline input: False
```

### -Scope
Defines if this JavaScript file will be injected to every page within the current site collection or web. All is not allowed in for this command. Default is web.

```yaml
Type: CustomActionScope
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Sequence
Sequence of this JavaScript being injected. Use when you have a specific sequence with which to have JavaScript files being added to the page. I.e. jQuery library first and then jQueryUI.

```yaml
Type: Int
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Url
URL to the JavaScript file to inject

```yaml
Type: String[]
Parameter Sets: (All)

Required: True
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