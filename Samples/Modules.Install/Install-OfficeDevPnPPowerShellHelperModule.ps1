Function Install-Office365DevPNPModules {
<#
.Synopsis
   This Function will Check and see if the Office365DevPNP PowerShellModules are installed on your system and if not will use PowerShell Package Management to install them
.DESCRIPTION
   This uses System.Net.WebRequest & System.Net.WebClient to download the specific version of PowerShellPackageManager for your OS version (x64/x86) and then uses
   msiexec to install it.
.EXAMPLE
   Install-Office365DevPNPPowerShellModule -ModuleToInstall v15
   #>
#Requires -Version 3.0
#Requires -Modules PowerShellGet
param (
        [Parameter(Mandatory=$true,HelpMessage='v15 is for SharePoint On-Premises, v16 is for SharePoint Online')]
        [ValidateSet('v15','v16')]
        [string] $ModuleToInstall   
       )
       
       if (!(Get-command -Module OfficeDevPnP.PowerShell.$ModuleToInstall.Commands).count -gt 0)
           {
           Install-Module OfficeDevPnP.PowerShell.$ModuleToInstall.Commands -Force
           }
           switch ($ModuleToInstall)
           {
            'v15' { $moduleVersion = 'SharePoint On-Premises'}
            'v16' { $moduleVersion = 'SharePoint Online'}
           }
        Write-Output "The modules for $moduleVersion have been installed and can now be used"
        Write-Output 'On the next release you can just run Update-Module -force to update this and other installed modules'
}

function Request-SPOOrOnPremises
{
    [string]$title="Confirm"
    [string]$message="Which version of the Modules do you want to install?"
    
	$SPO = New-Object System.Management.Automation.Host.ChoiceDescription "SPO", "SharePoint Online"
	$OnPrem = New-Object System.Management.Automation.Host.ChoiceDescription "OnPrem", "SharePoint On-Premises"
	$options = [System.Management.Automation.Host.ChoiceDescription[]]($SPO, $OnPrem)

	$result = $host.ui.PromptForChoice($title, $message, $options, 0)

	switch ($result)
	{
		1 { Return 'v15' } 
		0 { Return 'v16' }
	}
}


if ((Get-command -Module PowerShellGet).count -gt 0) 
    { 
    Write-Output 'PowerShellPackageManagement now installed we will now run the next command in 10 Seconds'
    Start-Sleep -Seconds 10 
    Install-Office365DevPNPModules -ModuleToInstall (Request-SPOOrOnPremises)
    }
    else
        {
        Write-Output "PowerShellPackageManagement is not installed on this Machine - Please run the below to install - you will need to Copy and Paste it as i'm not doing everything for you ;-)"
        Write-Output "Invoke-Expression (New-Object Net.WebClient).DownloadString('http://bit.ly/PSPackManInstall')"
        }