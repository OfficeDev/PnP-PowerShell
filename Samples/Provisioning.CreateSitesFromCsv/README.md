# Sample that creates SharePoint sites from csv file.

## Summary
This script creates site collections and/or sub webs from CSV configuration file (csv template included).

## Provision complex SharePoint site hierarchies

This is a quick example to shows how SharePoint sites hierarchy can be created from csv template. Let's assume that part of our Information Architecture is to provision three site collections, but we have more sub sites in the third site collection as follows:

```
site collection 1
site collection 2
site collection 3
	|- sub site 1
		|- sub site 1
	|- sub site 2
		|- sub site 1
```
Then csv configuration file could look like:

RootUrl|SiteUrl|Type|..
-------|-------|----|-----|-----|--------|--------|------
https://<'your_site'>.sharepoint.com/sites|SiteCol1|SiteCollection|..
https://<'your_site'>.sharepoint.com/sites|SiteCol2|SiteCollection|..
https://<'your_site'>.sharepoint.com/sites|SiteCol3|SiteCollection|..
https://<'your_site'>.sharepoint.com/sites/SiteCol3|SubSite1|SubWeb|..
https://<'your_site'>.sharepoint.com/sites/SiteCol3|SubSite2|SubWeb|..
https://<'your_site'>.sharepoint.com/sites/SiteCol3/SubSite1|SubSite1|SubWeb|..
https://<'your_site'>.sharepoint.com/sites/SiteCol3/SubSite2|SubSite1|SubWeb|..
---

Please have a look at the sample and the included csv template for more information.

## Windows Credential Manager can be used for automation scenarios

The sample uses [Windows Credential Manager](https://github.com/SharePoint/PnP-PowerShell/wiki/How-to-use-the-Windows-Credential-Manager-to-ease-authentication-with-PnP-PowerShell) for getting a credential in scenarios when full automation is needed. If label is not found in the Windows Credential Manager then prompt would ask for tenant admin email and password.

## Used SharePointPnPPowerShellOnline Version
![SharePointPnPPowerShellOnline](https://img.shields.io/badge/SharePointPnPPowerShellOnline-2.18.1709.1-green.svg)
![SharePointPnPPowerShellOnline](https://img.shields.io/badge/PSVersion-5.0.10586.117-green.svg)

## Applies to

- Office 365 Multi-Tenant (MT)
- Office 365 Dedicated (D)

## Prerequisites
- SharePoint Online Management Shell
- [Office Dev PnP PowerShell Commands](https://github.com/SharePoint/PnP-PowerShell/wiki/Install-SharePointPnPPowerShellOnline,-PowerShell-5.0-and-Nuget-behind-proxy)

## Solution

Solution|Author(s)
--------|---------
Provisioning.CreateSitesFromCsv	 | Velin Georgiev ([@VelinGeorgiev](https://twitter.com/velingeorgiev))

## Version history

Version|Date|Comments
-------|----|--------
1.0|September 19, 2017 | Initial commit


## **Disclaimer** 
THIS CODE IS PROVIDED AS IS WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR PURPOSE, MERCHANT ABILITY, OR NON-INFRINGEMENT.

---

## Minimal Path to Awesome
- Replace <'your_site'> tag with actual tenant name in the csv file.
- Replace email tag with actual tenant admin email in the csv file.

<img src="https://telemetry.sharepointpnp.com/PnP-PowerShell/Samples/Provisioning.CreateSitesFromCsv" />