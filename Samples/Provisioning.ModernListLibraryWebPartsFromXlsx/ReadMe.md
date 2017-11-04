# This sample provisions modern pages and sections with modern list or library web parts using an Excel template

## Summary

This PowerShell sample demonstrates how to use PnP PowerShell & ImportExcel Cmdlets to provision sites with modern pages, sections and list and library web parts using an Excel configuration file (please see sample template 'ModernPagesConfig.xlsx' provided).

## Prerequisites

- [SharePointPnPPowerShellOnline module](https://github.com/SharePoint/PnP-PowerShell/wiki/Install-SharePointPnPPowerShellOnline,-PowerShell-5.0-and-Nuget-behind-proxy)
- [ImportExcel module](https://github.com/dfinke/ImportExcel/blob/master/README.md)

## Getting Started

1. Edit 'ModernPagesConfig.xlsx' Excel Configuration file
- Update the TargetSiteUrl entry on the 'Site' worksheet to reflect the target tenant and site name e.g. https://mytenant.sharepoint.com/sites/group1
- Edit configuration values in the 'ModernPages' and 'ModernListLibraryWebParts' excel worksheets OR for initial testing leave as-is.
2. Edit $configFilePath variable in the ProvisionModernPagesAndWebParts.ps1 script with the path to your local Excel configuration file

3. Run the ProvisionModernPagesAndWebParts.ps1 script

## Version of Modules Used

- SharePointPnPPowerShellOnline 2.18.1709.1
- ImportExcel 2.4.0

## Windows Credential Manager can be used for automation scenarios

[Windows Credential Manager](https://github.com/SharePoint/PnP-PowerShell/wiki/How-to-use-the-Windows-Credential-Manager-to-ease-authentication-with-PnP-PowerShell) can be used in scenarios when full automation is needed.

## Author(s)

Geraint James

## Version history

Version|Date|Comments
-------|----|--------
1.0|1st November 2017 | Initial release

## **Disclaimer** 
THIS CODE IS PROVIDED AS IS WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR PURPOSE, MERCHANT ABILITY, OR NON-INFRINGEMENT.

---