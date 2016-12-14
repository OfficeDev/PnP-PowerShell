#Set-PnPContext
Sets the Client Context to use by the cmdlets
##Syntax
```powershell
Set-PnPContext -Context <ClientContext>
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Context|ClientContext|True|The ClientContext to set|
##Examples

###Example 1
```powershell
PS:> Connect-PnPOnline -Url $siteAurl -Credentials $credentials
PS:> $ctx = Get-PnPContext
PS:> Get-PnPList # returns the lists from site specified with $siteAurl
PS:> Connect-PnPOnline -Url $siteBurl -Credentials $credentials
PS:> Get-PnPList # returns the lists from the site specified with $siteBurl
PS:> Set-PnPContext -Context $ctx # switch back to site A
PS:> Get-PnPList # returns the lists from site A
```

