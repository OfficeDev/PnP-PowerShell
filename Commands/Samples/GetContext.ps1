$siteUrl = "https://<yourtenant>.sharepoint.com"
$credentials = Get-Credential
Connect-PnPOnline -Url $siteUrl -Credentials $credentials
$ctx = Get-PnPContext
$w = $ctx.Web
$w.Lists.GetByTitle("TestList")
$ctx.Load($w)
Execute-PnPQuery # Or use $ctx.ExecuteQuery()
