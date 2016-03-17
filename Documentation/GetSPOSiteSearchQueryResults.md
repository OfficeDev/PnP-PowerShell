#Get-SPOSiteSearchQueryResults
Executes a search query to retrieve indexed site collections
##Syntax
```powershell
Get-SPOSiteSearchQueryResults [-StartRow <Int32>] [-MaxResults <Int32>] [-Web <WebPipeBind>] [-Query <String>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|MaxResults|Int32|False|Maximum amount of search results to return. Default is 500 search results.|
|Query|String|False|Search query in Keyword Query Language (KQL) to execute to refine the returned sites. If omited, all indexed sites will be returned.|
|StartRow|Int32|False|Search result item to start returning the results from. Useful for paging. Leave at 0 to return all results.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Get-SPOSiteSearchQueryResults
```
Returns all site collections indexed by SharePoint Search

###Example 2
```powershell
PS:> Get-SPOSiteSearchQueryResults -Query "WebTemplate:STS"
```
Returns all site collections indexed by SharePoint Search which have are based on the STS (Team Site) template

###Example 3
```powershell
PS:> Get-SPOSiteSearchQueryResults -Query "WebTemplate:SPSPERS"
```
Returns all site collections indexed by SharePoint Search which have are based on the SPSPERS (MySite) template

###Example 4
```powershell
PS:> Get-SPOSiteSearchQueryResults -Query "Title:Intranet*"
```
Returns all site collections indexed by SharePoint Search of which the title starts with the word Intranet
