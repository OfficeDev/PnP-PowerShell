# Create SharePoint Sites from Csv file configuration

This script creates site collections and/or sub webs from CSV file configuration.
Please note: 
- Only sub webs one level/leaf deeper from the root site can be created. The script and the csv data have to be adjusted, if sub webs should be created on two or more levels/leafs away from the root web.
- The csv sites are created from top to bottom so sub webs to be created should be listed after site collections that do not exist.

Applies to

- Office 365 Multi-Tenant (MT)
- Office 365 Dedicated (D)
- SharePoint 2013 on-premises

## Prerequisites ##
	SharePoint Online Management Shell
	Office Dev PnP PowerShell Commands

## Solution ##
Script	
CreateSites.ps1	

Author(s)</br>
Velin Georgiev ([@VelinGeorgiev](https://twitter.com/velingeorgiev))

## Version history ##
Version:	1.0	</br>
Version	Date:  07/04/2017<br>
Comments:		Initial release</br>


## **Disclaimer** 
THIS CODE IS PROVIDED AS IS WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR PURPOSE, MERCHANT ABILITY, OR NON-INFRINGEMENT.
________________________________________
## General ##
- Replace <'your_site'> tag with actual tenant name in the csv file.
- Replace email tag with actual tenant admin email in the csv file.