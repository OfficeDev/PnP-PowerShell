# PnP PowerShell  #

### Summary ###
This solution contains a library of PowerShell commands that allows you to perform complex provisioning and artifact management actions towards SharePoint. The commands use a combination of CSOM and REST behind the scenes, and can work against both SharePoint Online as SharePoint On-Premises.

![SharePoint Patterns and Practices](https://devofficecdn.azureedge.net/media/Default/PnP/sppnp.png)
  
### Applies to ###
-  Sharepoint Online (Multi Tenant & Dedicated)
-  SharePoint 2019 on-premises
-  SharePoint 2016 on-premises
-  SharePoint 2013 on-premises

### Prerequisites ###
In order to generate the Cmdlet help you need to have the Windows Management Framework v4.0 installed, which you can download from http://www.microsoft.com/en-us/download/details.aspx?id=40855

If it is not [pre-installed on your operating system](https://docs.microsoft.com/powershell/scripting/wmf/overview#wmf-availability-across-windows-operating-systems), you can find installation instructions in the [WMF release notes.](https://docs.microsoft.com/powershell/scripting/wmf/overview#wmf-release-notes)
Check out the "Getting Started" section to make sure you have all requirements in place. 

### Latest Release Quick Download

The latest release can be found on [this link](https://github.com/pnp/PnP-PowerShell/releases)

----------

# Commands included #
[Navigate here for an overview of all cmdlets and their parameters](https://docs.microsoft.com/powershell/sharepoint/sharepoint-pnp/sharepoint-pnp-cmdlets?view=sharepoint-ps)

# Installation #
There are two ways:

## 1. Using the [PowerShell Gallery](https://www.powershellgallery.com) **(Recommended)**

If you main OS is Windows 10, or if you have [PowerShellGet](https://github.com/powershell/powershellget) installed, you can run the following commands to install the PowerShell cmdlets:

|**SharePoint Version**|**Command to install**|
|------------------|------------------|
|SharePoint Online|```Install-Module SharePointPnPPowerShellOnline ```|
|SharePoint 2019|```Install-Module SharePointPnPPowerShell2019```|
|SharePoint 2016|```Install-Module SharePointPnPPowerShell2016```|
|SharePoint 2013|```Install-Module SharePointPnPPowerShell2013```|

*Notice*: if you install the latest PowerShellGet from Github, you might receive an error message stating 
>PackageManagement\Install-Package : The version 'x.x.x.x' of the module 'SharePointPnPPowerShellOnline' being installed is not catalog signed.

In order to install the cmdlets when you get this error specify the -SkipPublisherCheck switch with the Install-Module cmdlet, e.g. ```Install-Module SharePointPnPPowerShellOnline -SkipPublisherCheck -AllowClobber```

## 2. Downloading the Files directly

You can download the setup files from the [releases](https://github.com/pnp/PnP-PowerShell/releases) section of the PnP PowerShell repository. These files will up be updated on a monthly basis. Run the install and restart any open instances of PowerShell to use the cmdlets.

### How to Update the Cmdlets 
Every month a new release will be made available of the PnP PowerShell Cmdlets. If you earlier installed the cmdlets using the setup file, simply download the [latest version](https://github.com/pnp/PnP-PowerShell/releases/latest) and run the setup. This will update your existing installation.

If you have installed the cmdlets using PowerShellGet with ```Install-Module``` from the PowerShell Gallery then you will be able to use the following command to install the latest updated version:

```powershell
Update-Module SharePointPnPPowerShell*
``` 

This will automatically load the module after starting PowerShell 3.0.

You can check the installed PnP-PowerShell versions with the following command:

```powershell
Get-Module SharePointPnPPowerShell* -ListAvailable | Select-Object Name,Version | Sort-Object Version -Descending
```

# Getting started #

To use the library you first need to connect to your tenant:

```powershell
Connect-PnPOnline –Url https://yoursite.sharepoint.com –Credentials (Get-Credential)
```

Or if you have Multi Factor Authentication enabled or if you are using a federated identity provider like AD FS, instead use:

```powershell
Connect-PnPOnline –Url https://yoursite.sharepoint.com –UseWebLogin
```

To view all cmdlets, enter:

```powershell
Get-Command -Module SharePointPnPPowerShell*
```

At the following links you will find a few videos on how to get started with the cmdlets:

* https://channel9.msdn.com/blogs/OfficeDevPnP/PnP-Web-Cast-Introduction-to-Office-365-PnP-PowerShell
* https://channel9.msdn.com/blogs/OfficeDevPnP/Introduction-to-PnP-PowerShell-Cmdlets
* https://channel9.msdn.com/blogs/OfficeDevPnP/PnP-Webcast-PnP-PowerShell-Getting-started-with-latest-updates

## Setting up credentials ##
See this [wiki page](https://github.com/OfficeDev/PnP-PowerShell/wiki/How-to-use-the-Windows-Credential-Manager-to-ease-authentication-with-PnP-PowerShell) for more information on how to use the Windows Credential Manager to setup credentials that you can use in unattended scripts.

# Contributing #

If you want to contribute to this SharePoint Patterns and Practices PowerShell library, please [proceed here](CONTRIBUTING.md)

### Solution/Authors ###
Solution | Author(s)
---------|----------
SharePointPnP.PowerShell | Erwin van Hunen and countless community contributors

### Disclaimer ###
**THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.**


## Building the source code ##

If you have set up the projects and you are ready to build the source code, make sure to build the SharePointPnP.PowerShellModuleFilesGenerator project first. This project will be executed after every build and it will generate the required PSD1 and XML files with cmdlet documentation in them.

When you build the solution a postbuild script will copy the required files to a folder in your users folder called 
*C:\Users\\\<YourUserName\>\Documents\WindowsPowerShell\Modules\SharePointPnPPowerShell\<Platform\>*. During build also the help and document files will be generated. If you have a session of PowerShell open in which you have used the PnP Cmdlets, make sure to close this PowerShell session first before you build. You will receive a build error otherwise because it tries to overwrite files that are in use.

To debug the cmdlets: launch PowerShell and attach Visual Studio to the powershell.exe process. In case you want to debug methods in PnP Sites Core, make sure that you open the PnP Sites Core project instead, and then attach Visual Studio to the powershell.exe. In case you see strange debug behavior, like it wants to debug PSReadLine.ps1, uninstall the PowerShell extension from Visual Studio.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

<img src="https://telemetry.sharepointpnp.com/pnp-powershell/readme" /> 
