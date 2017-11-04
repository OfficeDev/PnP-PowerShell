---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Submit-PnPSearchQuery

## SYNOPSIS
Executes an arbitrary search query against the SharePoint search index

## SYNTAX 

### Limit
```powershell
Submit-PnPSearchQuery -Query <String>
                      [-StartRow <Int>]
                      [-MaxResults <Int>]
                      [-TrimDuplicates <Boolean>]
                      [-Properties <Hashtable>]
                      [-Refiners <String>]
                      [-Culture <Int>]
                      [-QueryTemplate <String>]
                      [-SelectProperties <String[]>]
                      [-RefinementFilters <String[]>]
                      [-SortList <Hashtable>]
                      [-RankingModelId <String>]
                      [-ClientType <String>]
                      [-HiddenConstraints <String>]
                      [-TimeZoneId <Int>]
                      [-EnablePhonetic <Boolean>]
                      [-EnableStemming <Boolean>]
                      [-EnableQueryRules <Boolean>]
                      [-SourceId <Guid>]
                      [-ProcessBestBets <Boolean>]
                      [-ProcessPersonalFavorites <Boolean>]
                      [-RelevantResults [<SwitchParameter>]]
                      [-Web <WebPipeBind>]
                      [-Connection <SPOnlineConnection>]
```

### All
```powershell
Submit-PnPSearchQuery -Query <String>
                      [-All [<SwitchParameter>]]
                      [-TrimDuplicates <Boolean>]
                      [-Properties <Hashtable>]
                      [-Refiners <String>]
                      [-Culture <Int>]
                      [-QueryTemplate <String>]
                      [-SelectProperties <String[]>]
                      [-RefinementFilters <String[]>]
                      [-SortList <Hashtable>]
                      [-RankingModelId <String>]
                      [-ClientType <String>]
                      [-HiddenConstraints <String>]
                      [-TimeZoneId <Int>]
                      [-EnablePhonetic <Boolean>]
                      [-EnableStemming <Boolean>]
                      [-EnableQueryRules <Boolean>]
                      [-SourceId <Guid>]
                      [-ProcessBestBets <Boolean>]
                      [-ProcessPersonalFavorites <Boolean>]
                      [-RelevantResults [<SwitchParameter>]]
                      [-Web <WebPipeBind>]
                      [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Submit-PnPSearchQuery -Query "finance"
```

Returns the top 500 items with the term finance

### ------------------EXAMPLE 2------------------
```powershell
PS:> Submit-PnPSearchQuery -Query "Title:Intranet*" -MaxResults 10
```

Returns the top 10 items indexed by SharePoint Search of which the title starts with the word Intranet

### ------------------EXAMPLE 3------------------
```powershell
PS:> Submit-PnPSearchQuery -Query "Title:Intranet*" -All
```

Returns absolutely all items indexed by SharePoint Search of which the title starts with the word Intranet

### ------------------EXAMPLE 4------------------
```powershell
PS:> Submit-PnPSearchQuery -Query "Title:Intranet*" -Refiners "contentclass,FileType(filter=6/0/*)"
```

Returns absolutely all items indexed by SharePoint Search of which the title starts with the word Intranet, and return refiners for contentclass and FileType managed properties

## PARAMETERS

### -All
Automatically page results until the end to get more than 500. Use with caution!

```yaml
Type: SwitchParameter
Parameter Sets: All

Required: False
Position: Named
Accept pipeline input: False
```

### -ClientType
Specifies the name of the client which issued the query.

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Culture
The locale for the query.

```yaml
Type: Int
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -EnablePhonetic
Specifies whether the phonetic forms of the query terms are used to find matches.

```yaml
Type: Boolean
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -EnableQueryRules
Specifies whether Query Rules are enabled for this query.

```yaml
Type: Boolean
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -EnableStemming
Specifies whether stemming is enabled.

```yaml
Type: Boolean
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -HiddenConstraints
The keyword query’s hidden constraints.

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -MaxResults
Maximum amount of search results to return. Default and max per page is 500 search results.

```yaml
Type: Int
Parameter Sets: Limit

Required: False
Position: Named
Accept pipeline input: False
```

### -ProcessBestBets
Determines whether Best Bets are enabled.

```yaml
Type: Boolean
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -ProcessPersonalFavorites
Determines whether personal favorites data is processed or not.

```yaml
Type: Boolean
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Properties
Extra query properties. Can for example be used for Office Graph queries.

```yaml
Type: Hashtable
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Query
Search query in Keyword Query Language (KQL).

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: 0
Accept pipeline input: True
```

### -QueryTemplate
Specifies the query template that is used at run time to transform the query based on user input.

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -RankingModelId
The identifier (ID) of the ranking model to use for the query.

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -RefinementFilters
The set of refinement filters used.

```yaml
Type: String[]
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Refiners
The list of refiners to be returned in a search result.

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -RelevantResults
Specifies whether only relevant results are returned

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -SelectProperties
The list of properties to return in the search results.

```yaml
Type: String[]
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -SortList
The list of properties by which the search results are ordered.

```yaml
Type: Hashtable
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -SourceId
Specifies the identifier (ID or name) of the result source to be used to run the query.

```yaml
Type: Guid
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -StartRow
Search result item to start returning the results from. Useful for paging. Leave at 0 to return all results.

```yaml
Type: Int
Parameter Sets: Limit

Required: False
Position: Named
Accept pipeline input: False
```

### -TimeZoneId
The identifier for the search query time zone.

```yaml
Type: Int
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -TrimDuplicates
Specifies whether near duplicate items should be removed from the search results.

```yaml
Type: Boolean
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

## OUTPUTS

### List<System.Object>

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)