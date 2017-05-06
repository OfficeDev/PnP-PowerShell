# Submit-PnPSearchQuery
Executes an arbitrary search query against the SharePoint search index
## Syntax
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
```


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
```


## Returns
>List<System.Object>

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Query|String|True|Search query in Keyword Query Language (KQL).|
|All|SwitchParameter|False|Automatically page results until the end to get more than 500. Use with caution!|
|ClientType|String|False|Specifies the name of the client which issued the query.|
|Culture|Int|False|The locale for the query.|
|EnablePhonetic|Boolean|False|Specifies whether the phonetic forms of the query terms are used to find matches.|
|EnableQueryRules|Boolean|False|Specifies whether Query Rules are enabled for this query.|
|EnableStemming|Boolean|False|Specifies whether stemming is enabled.|
|HiddenConstraints|String|False|The keyword query’s hidden constraints.|
|MaxResults|Int|False|Maximum amount of search results to return. Default and max per page is 500 search results.|
|ProcessBestBets|Boolean|False|Determines whether Best Bets are enabled.|
|ProcessPersonalFavorites|Boolean|False|Determines whether personal favorites data is processed or not.|
|Properties|Hashtable|False|Extra query properties. Can for example be used for Office Graph queries.|
|QueryTemplate|String|False|Specifies the query template that is used at run time to transform the query based on user input.|
|RankingModelId|String|False|The identifier (ID) of the ranking model to use for the query.|
|RefinementFilters|String[]|False|The set of refinement filters used.|
|Refiners|String|False|The list of refiners to be returned in a search result.|
|RelevantResults|SwitchParameter|False|Specifies whether only relevant results are returned|
|SelectProperties|String[]|False|The list of properties to return in the search results.|
|SortList|Hashtable|False|The list of properties by which the search results are ordered.|
|SourceId|Guid|False|Specifies the identifier (ID or name) of the result source to be used to run the query.|
|StartRow|Int|False|Search result item to start returning the results from. Useful for paging. Leave at 0 to return all results.|
|TimeZoneId|Int|False|The identifier for the search query time zone.|
|TrimDuplicates|Boolean|False|Specifies whether near duplicate items should be removed from the search results.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Submit-PnPSearchQuery -Query "finance"
```
Returns the top 500 items with the term finance

### Example 2
```powershell
PS:> Submit-PnPSearchQuery -Query "Title:Intranet*" -MaxResults 10
```
Returns the top 10 items indexed by SharePoint Search of which the title starts with the word Intranet

### Example 3
```powershell
PS:> Submit-PnPSearchQuery -Query "Title:Intranet*" -All
```
Returns absolutely all items indexed by SharePoint Search of which the title starts with the word Intranet

### Example 4
```powershell
PS:> Submit-PnPSearchQuery -Query "Title:Intranet*" -Refiners "contentclass,FileType(filter=6/0/*)"
```
Returns absolutely all items indexed by SharePoint Search of which the title starts with the word Intranet, and return refiners for contentclass and FileType managed properties
