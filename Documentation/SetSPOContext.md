#Set-SPOContext
Sets the Client Context to use by the cmdlets
##Syntax
```powershell
Set-SPOContext -Context <ClientContext>
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Context|ClientContext|True|The ClientContext to set|
##Examples

###Example 1
```powershell
PS:> Connect-SPOnline -Url $siteAurl -Credentials $credentials
PS:> $ctx = Get-SPOContext
PS:> Get-SPOList # returns the lists from site specified with $siteAurl
PS:> Connect-SPOnline -Url $siteBurl -Credentials $credentials
PS:> Get-SPOList # returns the lists from the site specified with $siteBurl
PS:> Set-SPOContext -Context $ctx # switch back to site A
PS:> Get-SPOList # returns the lists from site A
```

