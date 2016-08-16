# SharePointPnP.PowerShell Commands #

### Summary ###
This solution shows how you can build a library of PowerShell commands that act towards SharePoint Online. The commands use CSOM and can work against both SharePoint Online as SharePoint On-Premises.

### Applies to ###
-  Office 365 Multi Tenant (MT)
-  Office 365 Dedicated (D)
-  SharePoint 2013 on-premises

### Prerequisites ###
In order to build the setup project the WiX toolset needs to be installed. You can obtain this from http://wix.codeplex.com. If you use Visual Studio 2015 you will need at least WiX 3.10, but do not install WiX v4.x, which can be downloaded from here: http://wixtoolset.org/releases/

In order to generate the Cmdlet help you need Windows Management Framework v4.0 which you can download from http://www.microsoft.com/en-us/download/details.aspx?id=40855

### Solution ###
Solution | Author(s)
---------|----------
SharePointPnP.PowerShell | Erwin van Hunen

### Disclaimer ###
**THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.**


----------

# COMMANDS INCLUDED #
[Navigate here for an overview of all cmdlets and their parameters](Documentation/readme.md)

# INSTALLATION #

## Setup files ##
You can download setup files from https://github.com/officedev/pnp-powershell/releases. These files will up be updated on a monthly basis.

## Using the Windows Management Framework ##

If you main OS is Windows 10, you can run the following commands to install the PowerShell cmdlets:

_SharePoint Online_
```powershell
Install-Module SharePointPnPPowerShellOnline
```

_SharePoint 2016_
```powershell
Install-Module SharePointPnPPowerShell2016
```

_SharePoint 2013_
```powershell
Install-Module SharePointPnPPowerShell2013
```

Alternatively for installation on machines that have at least PowerShell v3 installed (you can find this out by opening PowerShell and running $host.version and Major should be above 3) you can run the below command which will install PowerShell Package Management and then install the PowerShell Modules from the PowerShell Gallery

```powershell
Invoke-Expression (New-Object Net.WebClient).DownloadString('https://raw.githubusercontent.com/OfficeDev/PnP-PowerShell/master/Samples/Modules.Install/Install-SharePointPnPPowerShell.ps1')
```

If you wish to see the commands that the above will run please see the files as stored in the below locations:
* https://raw.githubusercontent.com/OfficeDev/PnP-PowerShell/master/Samples/Modules.Install/Install-PowerShellPackageMangement.ps1
* https://raw.githubusercontent.com/OfficeDev/PnP-PowerShell/master/Samples/Modules.Install/Install-SharePointPnPPowerShellHelperModule.ps1

Once the above has been completed you can then start to use the PowerShell Modules

Once new releases of the module are made available on the PowerShell Gallery you will be able to use the the following command to install the latest updated version

```powershell
Update-Module
``` 

This will automatically load the module after starting PowerShell 3.0.

You can check the installed PnP-PowerShell versions with the following command:

```powershell
Get-Module OfficeDevPnP.Powershell.* -ListAvailable | Select-Object Name,Version | Sort-Object Version -Descending
```

## HOW TO USE DURING DEVELOPMENT ##

A build script will copy the required files to a folder in your users folder, called:
*C:\Users\<YourUserName>\Documents\WindowsPowerShell\Modules\OfficeDevPnP.PowerShell.V16.Commands*

# GETTING STARTED #

To use the library you first need to connect to your tenant:

```powershell
Connect-SPOnline –Url https://yoursite.sharepoint.com –Credentials (Get-Credential)
```

To view all cmdlets, enter

```powershell
Get-Command -Module *PnP*
```

At the following links you will find a few videos on how to get started with the cmdlets:

* https://channel9.msdn.com/blogs/OfficeDevPnP/PnP-Web-Cast-Introduction-to-Office-365-PnP-PowerShell
* https://channel9.msdn.com/blogs/OfficeDevPnP/Introduction-to-PnP-PowerShell-Cmdlets

# SETTINGS UP CREDENTIALS #
In case of an unattended script you might want to add a new entry in your credential manager of windows. 

![](http://i.imgur.com/6NiMaFL.png)
 
Select Windows Credentials and add a new *generic* credential:

![](http://i.imgur.com/rhtgL1U.png)
 
Now you can use this entry to connect to your tenant as follows:

```powershell
Connect-SPOnline –Url https://yoursite.sharepoint.com –Credentials yourlabel
```

Alternatively you can create a credential manager entry with an internet or network address starting with your tenant url, e.g. https://mytenant.sharepoint.com. If you then use Connect-SPOnline -Url https://mytenant.sharepoint.com/sites/yoursite
to create a new connection, the cmdlet will resolve the credentials to use based upon the URL.

# Contributing #

If you want to contribute to this OfficeDevPnP PowerShell library, please [proceed here](CONTRIBUTING.md)

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
