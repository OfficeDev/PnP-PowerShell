# Site order and provisioning using PowerShell and Azure Web Job

This PowerShell sample demonstrates how to use the Office Dev PnP PowerShell in an Azure Web Job
to order and provision Team Sites. The solution is hosted in SharePoint, and site configurations
are stores as provisioning .pnp files in SharePoint libraries.

The sample solution is set up so that Owners, Members and Visitors are set at item create time, but the fields are hidden for modifiction. This means the site will be master for permissions once created.

You can easily add more logic to the site creation or governance process to perform tasks specified by your own business requirements.


Applies to


- Office 365 Multi-Tenant (MT)
- Office 365 Dedicated (D)

## Prerequisites ##
	Office Dev PnP PowerShell Commands
	Azure Subscription with an App Service

# GETTING STARTED ##

## Set up the site directory

1. Create a new site collection to host the site directory, or use an existing one
2. Connect-PnPOnline -Url https://&lt;tenant&gt;.sharepoint.com(/sites/sitecollection)
3. Apply-PnPProvisioningTemplate -Path .\Templates\SiteDirectory\sitedirectory.xml

In your site you should now see the following:

List/Library | Description
--- | ---
Sites | list to order sites
Site templates | templates - functional template definitions with reference to modules and apps
Modules | library to store larger PnP template files
Apps |library to store functional scoped PnP template files

* Upload .\Templates\teamsite.pnp to *Modules*, and set *Default Team Site* as the title
* Upload .\Templates\pictures.pnp to *Apps*, and set *Picture library* as the title
* Create an item in *Site templates* and link up *Default Team Site* as a module
  * optionally link up *Picture library* as an app
* Create an item in *Sites*
  * pick the template you just created
  * optionally pick an app

## SharePoint App Registration

* Register a SharePoint app id/secret to be used to provision sites - record id and secret
  * https://&lt;tenant&gt;.sharepoint.com/_layouts/15/AppRegNew.aspx
* Add permissions to the registered app
  * https://&lt;tenant&gt;-admin.sharepoint.com/_layouts/15/AppInv.aspx

        Permission XML
        --------------
        <AppPermissionRequests AllowAppOnlyPolicy="true">
                <AppPermissionRequest Scope="http://sharepoint/content/tenant" Right="FullControl" />
                <AppPermissionRequest Scope="http://sharepoint/social/tenant" Right="Read" />
                <AppPermissionRequest Scope="http://sharepoint/taxonomy " Right="Read" />
        </AppPermissionRequests>

## Test it out

* Set the following environmental variables in PowerShell, 

```powershell
# E-mail address of a real user to be the primary site collection administrator
$env:APPSETTING_PrimarySiteCollectionOwnerEmail = "admin@<tenant>.onmicrosoft.com" 
# URL where you installed the directory
$env:APPSETTING_SiteDirectoryUrl = "/sites/sitedirectory"
$env:APPSETTING_TenantURL = "https://<tenant>.sharepoint.com"
$env:APPSETTING_AppId = "<your id>"
$env:APPSETTING_AppSecret = "<your secret>"
```

* Edit .\Engine\shared.ps1 to match your environment if you have renamed column prefix or similar, or if you want to provision sites on the managed path */sites* and not */teams*
* Run .\Engine\mrprovision.ps1
* Once done visit the newly created site (link from Site Directory, or via e-mail)
	* The requestor and site owner will get e-mails when the site is ready
	* The site owners are set as site request recipients
* Add some more owners, members or visitors to the site
* Try out the governance script which syncs users back to the site directory
* Run .\Engine\mrgovernance.ps -syncPermissions

## Package and Deploy Azure web jobs

1. Run .\Package-WebJobs.ps1
2. Create new Azure web jobs for each row in table below

| Name                        | File Upload            | Type      | Triggers  | Cron Expression |
--------------------------- | ---------------------- | --------- | --------- | ---------------  |
| Pzl-O365-Site-Provisioning | provisioning.zip       | Triggered | Scheduled | 0 0/15 * * * *  |
| Pzl-O365-Site-Gov-Daily    | governance-daily.zip   | Triggered | Scheduled | 0 0 5 * * *     |

3. Add the following app settings to the app service
![app settings](azure-webjob.png)

Key | Value
--- | ---
SiteDirectoryUrl | /sites/sitedirectory
TenantURL | https://&lt;tenant&gt;.sharepoint.com
AppId | &lt;your id&gt;
AppSecret | &lt;your secret&gt;
PrimarySiteCollectionOwnerEmail | admin@&lt;tenant&gt;.sharepoint.com

4. Remember to toggle Always On for the web job in a production

## Solution
.\Engine folder - scripts for provisioning and governance</br>
.\Engine\bundle - copy of all PnP PowerShell files - update with latest version</br>
.\Engine\resources - e-mail templates</br>
.\Templates - site directory .pnp template and sample templates</br>
Package-WebJobs.ps1 - script to create web job zip files</br>

## Author(s)
Mikael Svenson (Puzzlepart)</br>
Tarjei Ormestøyl (Puzzlepart)</br>
Petter Skodvin-Hvammen (Puzzlepart)</br>

## Version history ##
Version:	1.0	</br>
Version	Date:  3/22/2017<br>
Comments:		Initial release</br>


## **Disclaimer** 
THIS CODE IS PROVIDED AS IS WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR PURPOSE, MERCHANT ABILITY, OR NON-INFRINGEMENT.
________________________________________
## General ##
- Replace &lt;tenant&gt; tag with actual tenant name



