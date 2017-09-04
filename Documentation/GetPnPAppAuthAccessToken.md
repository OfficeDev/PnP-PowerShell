# Get-PnPAppAuthAccessToken
Returns the access token from the current client context (In App authentication mode only)
## Returns
>[System.String](https://msdn.microsoft.com/en-us/library/system.string.aspx)

## Examples

### Example 1
```powershell
PS:> $accessToken = Get-PnPAppAuthAccessToken
```
This will put the access token from current context in the $accessToken variable. Will only work in App authentication flow (App+user or App-Only)
