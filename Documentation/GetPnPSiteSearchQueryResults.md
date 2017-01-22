#Get-PnPSiteSearchQueryResults
Executes a search query to retrieve indexed site collections
##Syntax
```powershell
Get-PnPSiteSearchQueryResults [-All [<SwitchParameter>]]
                              [-Web <WebPipeBind>]
                              [-Query <String>]
```


```powershell
Get-PnPSiteSearchQueryResults [-StartRow <Int32>]
                              [-MaxResults <Int32>]
                              [-Web <WebPipeBind>]
                              [-Query <String>]
```


##Returns
>System.Collections.Generic.List`1[System.Object]

##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|All|SwitchParameter|False|Automatically page results until the end to get more than 500 sites. Use with caution!|
|MaxResults|Int32|False|Maximum amount of search results to return. Default and max is 500 search results.|
|Query|String|False|Search query in Keyword Query Language (KQL) to execute to refine the returned sites. If omitted, all indexed sites will be returned.|
|StartRow|Int32|False|Search result item to start returning the results from. Useful for paging. Leave at 0 to return all results.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Get-PnPSiteSearchQueryResults
```
Returns the top 500 site collections indexed by SharePoint Search

###Example 2
```powershell
PS:> Get-PnPSiteSearchQueryResults -Query "WebTemplate:STS"
```
Returns the top 500 site collections indexed by SharePoint Search which have are based on the STS (Team Site) template

###Example 3
```powershell
PS:> Get-PnPSiteSearchQueryResults -Query "WebTemplate:SPSPERS"
```
Returns the top 500 site collections indexed by SharePoint Search which have are based on the SPSPERS (MySite) template, up to the MaxResult limit

###Example 4
```powershell
PS:> Get-PnPSiteSearchQueryResults -Query "Title:Intranet*"
```
Returns the top 500 site collections indexed by SharePoint Search of which the title starts with the word Intranet

###Example 5
```powershell
PS:> Get-PnPSiteSearchQueryResults -MaxResults 10
```
Returns the top 10 site collections indexed by SharePoint Search

###Example 6
```powershell
PS:> Get-PnPSiteSearchQueryResults -All
```
Returns absolutely all site collections indexed by SharePoint Search
