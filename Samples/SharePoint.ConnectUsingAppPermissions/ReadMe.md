# Connect to the SharePoint Online using Application Permissions

This PowerShell sample demonstrates how to use the Office Dev PnP PowerShell to connect to SharePoint Online
using Application Permissions. Using application permissions is useful for automated tasks
and service scenarios where you don't have an end-user logging in.

Applies to


- Office 365 Multi-Tenant (MT)

## Prerequisites ##
	Create a self-signed certificate
    Register an Application	
	PnP PowerShell Commands

## GETTING STARTED ##
The PnP commandlets can use the Active Directory Authentication Library (ADAL) to connect with SharePoint Online, but in doing so you need a certificate when talking to SharePoint Online as using application id and key is not sufficient.

## Create the self signed certificate

You are now ready to configure the Azure AD Application for invoking SharePoint Online with an App Only access token. In order to do that, you have to create and configure a self-signed X.509 certificate, which will be used to authenticate your Application against Azure AD, while requesting the App Only access token. 

First of all, you have to create the self-signed X.509 Certificate, which can be created using the `New-PnPAzureCertificate` commandlet.

You may also add the `-Out` parameter if you want to save the certificate as a local .pfx file.

Create a self signed certificate using `New-PnPAzureCertificate` (output truncated for readability):

```PowerShell
$cert = New-PnPAzureCertificate
$cert

Subject        : CN=pnp.contoso.com
ValidFrom      : 22.02.2018 00:00:00
ValidTo        : 22.02.2028 00:00:00
KeyCredentials :
                 {
                     "customKeyIdentifier": "NYNvV+Q0zrXcehnvPJwaQVuWrCw=",
                     "keyId": "87abb85b-7bf2-482b-b4d4-8c2d1d869f72",
                     "type": "AsymmetricX509Cert",
                     "usage": "Verify",
                     "value":  "MIICv...iqzrk="
                 }

Certificate    : -----BEGIN CERTIFICATE-----MIICv...iqzrk=-----END CERTIFICATE-----
PrivateKey     : -----BEGIN RSA PRIVATE KEY-----MIIEp...4W6g==-----END
                 RSA PRIVATE KEY-----
```

>For further details about the `New-PnPAzureCertificate` syntax and command line parameters you can read the documentation with `Get-Help New-PnPAzureCertificate -Detailed`. If you have an existing .pfx file you can get the PEM values and key credential manifest settings using `Get-PnPAzureCertificate`.

Both the certificate and the private key is outputted as PEM encoded strings. This is useful in automation where you can store the values as environmental variables or as strings in Azure Keyvault, instead of bundling the actual .pfx file itself.

## Azure Active Directory Application registration
In order to connect to SharePoint Online using app-only permissions you have to **register an application in Azure Active Directory** linked to your Office 365 tenant. In order to do that, open Azure Active Directory Admin portal (https://aad.portal.azure.com) using the account of a user member of the Tenant Global Admins group.

Select the "Active Directory" section, by clicking on the icon highlighted in the following screen shot:

![Azure AD Button](./Fig-01-Azure-AD-Button.png)

You'll see on the left side of the blade that you opened the Azure AD tenant corresponding to your Office 365 tenant. Locate and select the option "App Registrations". See the next figure for further details.

![Azure AD Main Page](./Fig-02-Azure-AD-Main-Page.png)

In the "App Registrations" tab you will find the list of Azure AD applications registered in 
your tenant. Click the "New application registraion" button in the upper left part of the blade, this will show you the following screen.

![Azure AD - Add an Application - First Step](./Fig-03-Azure-AD-Add-Application-Step-01.png)

Provide a **name** for your application (we suggest to name it "SharePoint PnP CSOM Access"), select the option **"Web app / API"**, and fill in the **"Sign-on URL"** with the with a valid **URL**, for example _https://&lt;tenant&gt;.sharepoint.com/pnpcsom_. The URL does not have to exist, but it has to be valid. Click create when done.

The newly created app registration will now be listed in your "App Registrations" list.
Open it and then click into settings and then Properties.  You should now be at the following screen: 

![Azure AD - Add an Application - Third Step](./Fig-04-Azure-AD-Add-Application-Step-02.png)

Please make sure you :
- Copy the **Application ID** value as you'll need it later in the `ClientId` parameter when connecting to SharePoint Online.

Now, you should go back to the settings blade. Go into **Keys** where you'll create a Client Secret (used for app-only authentication). In order to do that, add a new security key (selecting 1 year, 2 years or never expires for key duration). Press the "Save" button in the lower part of the screen to generate the key value. After saving, you will see the key value. **Copy it in a safe place**, because you will not see it anymore.

>This key is not really needed when accessing SharePoint Online via CSOM, but if you access other API's using the same ADAL application you will need it. For example if you are creating Office 365 Groups using PnP PowerShell.

![Azure AD - Create a client Secret](./Fig-05-Azure-AD-Add-AplicationSecret.png)

Now click on "Required Permissions", and click on the "Add" button, a new blade will appear.

![Azure AD - Application - Required Permissions ](./Fig-06-Azure-AD-App-Config-02.png)

You need to configure the following permissions in order to get access to all resources:
* Office 365 SharePoint Online (**Application Permission**)
  * **Have full control of all site collections**
  * **Read and write managed metadata**
  * **Read and write user profiles**

  You may of course opt in with less access as well. For further details, see the following figure.

![Azure AD - Application Configuration - Permissions Blade](./Fig-07-Azure-AD-App-Config-03.png)

The "Application Permissions" are those granted to the application when running as App Only. The other set of permissions, called "Delegated Permissions", defines the permissions granted to the application when running under a specific user's account delegation (using an app and user access token, from an OAuth 2.0 perspective).

Click the Grant Permission button on the 'Required Permissions' tab in order make the permissions effective. If you forget this, you will not be able to connect to SharePoint Online using the ADAL application.

<a name="apponlyazuread"></a>
### Update Azure AD Application manifest

From your generated certificate copy the key credentials which are to be added to the manifest of your ADAL application.


```PowerShell
$cert.KeyCredentials | clip
```

Go back to the Azure AD Application that you created in the previous step and click the **"Manifest"** button at the top of the blade, then click **Edit'**.

![Azure AD - Application Configuration - Manifest](./Fig-08-Azure-AD-App-Config-04.png)

Search for the **keyCredentials** property and replace it with the snippet you generated before, this will be similar to as seen on the figure above:

```JSON
  "keyCredentials": [
    {
      "customKeyIdentifier": "<base64CertHash>",
      "keyId": "<KeyId>",
      "type": "AsymmetricX509Cert",
      "usage": "Verify",
      "value":  "<base64Cert>"
     }
  ],
```

Click **Save** when you complete this step.

## Test the application using PnP
Using the application id and application password from the application registration you can
connect to the SharePoint Online using:

```PowerShell
> Connect-PnPOnline -PEMCertificate $cert.Certificate -PEMPrivateKey $cert.PrivateKey -Tenant contoso.onmicrosoft.com -ClientId e3b084e2-5b69-44ef-8bac-0592abfd123f -Url https://contoso.sharepoint.com

```

If you opted to save the certificate as a .pfx file you can connect using:

```PowerShell
> Connect-PnPOnline -CertificatePath .\mycert.pfx -Tenant contoso.onmicrosoft.com -ClientId e3b084e2-5b69-44ef-8bac-0
592abfd123f -Url https://contoso.sharepoint.com 
```

If all went as expected you should now be able get data from your site.

```PowerShell
> Get-PnPList

Title                             Id                                   Url
-----                             --                                   ---
appdata                           c0d1659f-bc88-4808-a625-9b8969ff74f4 /_catalogs/appdata
appfiles                          0a4313b9-6d75-417c-a90f-5c9b557a444f /_catalogs/appfiles
Composed Looks                    1cc3545b-0b02-4bf3-9036-c07e83414dfc /_catalogs/design
Content type publishing error log f33c40a2-69b6-47d8-93fe-e66aecd3a085 /Lists/ContentTypeSyncLog
Converted Forms                   b03aa47d-560d-4fa6-95f1-4ad3b42a20cf /IWConvertedForms
Documents                         1009a8ee-40a5-4700-90c5-a186a3e64fef /Shared Documents
Form Templates                    cb166482-eeaf-43fe-a1a5-cc6b4ba8b716 /FormServerTemplates
List Template Gallery             0023e844-dd0b-42b6-8f9c-3b53cc2829d5 /_catalogs/lt
Maintenance Log Library           0edbb1a0-a543-449e-8344-5ac86fa7b455 /_catalogs/MaintenanceLogs
Master Page Gallery               26c2a29c-7d83-4a52-a20a-bb3381814b14 /_catalogs/masterpage
MicroFeed                         99d39803-91d0-4b9c-a204-b2999ac4d9f5 /Lists/PublishedFeed
Project Policy Item List          46bc7a90-3897-47d2-85fe-e64e3a014fe9 /ProjectPolicyItemList
SharePointHomeOrgLinks            4c437943-8507-4f5f-91c9-6473e2347274 /Lists/SharePointHomeOrgLinks
Site Assets                       356439d3-3bdd-431a-be9d-27941571adf1 /SiteAssets
Site Pages                        29632f91-ef58-4dc0-9e53-56338e4e3090 /SitePages
Solution Gallery                  7ccda759-d87e-413f-9214-76b0dd046749 /_catalogs/solutions
Style Library                     066f5cbd-eae2-4db7-9834-6c0d84b537d9 /Style Library
TaxonomyHiddenList                a3e0ee6a-7e35-4d1e-960a-2a344279a8fe /Lists/TaxonomyHiddenList
Theme Gallery                     cc0ca5b4-b978-4f09-93dd-06eb1a4d16a2 /_catalogs/theme
User Information List             1278a6e0-50f2-4d19-b8d5-3ce9ed484fd6 /_catalogs/users
Web Part Gallery                  cc3335b6-95e5-4aaa-8091-8fa827fdf880 /_catalogs/wp
wfpub                             7c1c98b8-e68c-49b8-8fc4-1789b088b516 /_catalogs/wfpub
```


## Solution ##
Author(s)</br>
Mikael Svenson (Puzzlepart)

## Version history ##
Version  | Date | Comments
---------| -----| --------
1.0  | Feb 23 2018 | Initial release

## **Disclaimer** 
THIS CODE IS PROVIDED AS IS WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
________________________________________

