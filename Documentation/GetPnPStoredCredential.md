#Get-PnPStoredCredential
Returns a stored credential from the Windows Credential Manager
##Syntax
```powershell
Get-PnPStoredCredential -Name <String>
                        [-Type <CredentialType>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Name|String|True|The credential to retrieve.|
|Type|CredentialType|False|The object type of the credential to return from the Credential Manager. Possible valus are 'O365', 'OnPrem' or 'PSCredential'|
##Examples

###Example 1
```powershell
PS:> Get-SPOnlineStoredCredential -Name O365
```
Returns the credential associated with the specified identifier

###Example 2
```powershell
PS:> Get-SPOnlineStoredCredential -Name testEnvironment -Type OnPrem
```
Gets the credential associated with the specified identifier from the credential manager and then will return a credential that can be used for on-premises authentication
