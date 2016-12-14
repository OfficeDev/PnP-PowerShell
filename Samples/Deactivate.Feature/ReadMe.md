# Deactivate web feature in all sub-sites in Site Collection 
This sample helps to understand how to iterate through all the sub sites in the site collection using PnP PowerShell commands, and also to deactivate a feature in all the sub-sites in the site collection. For this, we will deactivate "Mobile Browser View" feature. It will really come handy when you are trying to make your existing site collection responsive.

## Applies to ##
- Office 365 Multi-Tenant (MT)
- Office 365 Dedicated (D)
- SharePoint 2013 on-premises

## Prerequisites ##
	PnP PowerShell Commands

## GETTING STARTED ##
To use the script
```powershell
.\Deactivate-MobileBrowerView.ps1 -Url https://yoursitecollectionURL
```
To force deactivation
```powershell
.\Deactivate-MobileBrowerView.ps1 -Url https://yoursitecollectionURL -Force
```
Pass credentials, 
```powershell
.\Deactivate-MobileBrowerView.ps1 -Url https://yoursitecollectionURL -Credentials (Get-Credential)
```

## Solution ##
Solution | Author(s)
---------|----------
Deactivate-MobileBrowerView.ps1 | Rajesh Sitaraman

## Version history ##
Version  | Date | Comments
---------| -----| --------
1.0  | Apr 18 2016 | Initial release

----------

## **Disclaimer** 
THIS CODE IS PROVIDED AS IS WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.


