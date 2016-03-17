param($ProjectDir, $ConfigurationName, $TargetDir, $TargetFileName, $SolutionDir)

if($ConfigurationName -like "Debug*")
{
	$documentsFolder = [environment]::getfolderpath("mydocuments");

	if($ConfigurationName -like "Debug15")
	{
		$PSModuleHome = "$documentsFolder\WindowsPowerShell\Modules\OfficeDevPnP.PowerShell.V15.Commands"
	} else {
		$PSModuleHome = "$documentsFolder\WindowsPowerShell\Modules\OfficeDevPnP.PowerShell.V16.Commands"
	}
	# Module folder there?
	if(Test-Path $PSModuleHome)
	{
		# Yes, empty it
		Remove-Item $PSModuleHome\*
	} else {
		# No, create it
		New-Item -Path $PSModuleHome -ItemType Directory -Force >$null # Suppress output
	}

	Write-Host "Copying files from $TargetDir to $PSModuleHome"
	Copy-Item "$TargetDir\*.dll" -Destination "$PSModuleHome"
	Copy-Item "$TargetDir\*help.xml" -Destination "$PSModuleHome"
	if($ConfigurationName -like "Debug15")
	{
		Copy-Item "$TargetDir\ModuleFiles\OfficeDevPnP.PowerShell.V15.Commands.psd1" -Destination  "$PSModuleHome"
		Copy-Item "$TargetDir\ModuleFiles\OfficeDevPnP.PowerShell.V15.Commands.Format.ps1xml" -Destination "$PSModuleHome"
	} else {
		Copy-Item "$TargetDir\ModuleFiles\OfficeDevPnP.PowerShell.V16.Commands.psd1" -Destination  "$PSModuleHome"
		Copy-Item "$TargetDir\ModuleFiles\OfficeDevPnP.PowerShell.V16.Commands.Format.ps1xml" -Destination "$PSModuleHome"
	}
} elseif ($ConfigurationName -like "Release*")
{
    $documentsFolder = [environment]::getfolderpath("mydocuments");

	if($ConfigurationName -like "Release15")
	{
		$PSModuleHome = "$documentsFolder\WindowsPowerShell\Modules\OfficeDevPnP.PowerShell.V15.Commands"
	} else {
		$PSModuleHome = "$documentsFolder\WindowsPowerShell\Modules\OfficeDevPnP.PowerShell.V16.Commands"
	}
	# Module folder there?
	if(Test-Path $PSModuleHome)
	{
		# Yes, empty it
		Remove-Item $PSModuleHome\*
	} else {
		# No, create it
		New-Item -Path $PSModuleHome -ItemType Directory -Force >$null # Suppress output
	}

	Write-Host "Copying files from $TargetDir to $PSModuleHome"
	Copy-Item "$TargetDir\*.dll" -Destination "$PSModuleHome"
	Copy-Item "$TargetDir\*help.xml" -Destination "$PSModuleHome"
	if($ConfigurationName -like "Release15")
	{
		Copy-Item "$TargetDir\ModuleFiles\OfficeDevPnP.PowerShell.V15.Commands.psd1" -Destination  "$PSModuleHome"
		Copy-Item "$TargetDir\ModuleFiles\OfficeDevPnP.PowerShell.V15.Commands.Format.ps1xml" -Destination "$PSModuleHome"
	} else {
		Copy-Item "$TargetDir\ModuleFiles\OfficeDevPnP.PowerShell.V16.Commands.psd1" -Destination  "$PSModuleHome"
		Copy-Item "$TargetDir\ModuleFiles\OfficeDevPnP.PowerShell.V16.Commands.Format.ps1xml" -Destination "$PSModuleHome"
	}
}

	