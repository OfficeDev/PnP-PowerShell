param($ProjectDir, $ConfigurationName, $TargetDir, $TargetFileName, $SolutionDir)

if($ConfigurationName -like "Debug*")
{
	$documentsFolder = [environment]::getfolderpath("mydocuments");
	if($ConfigurationName -like "Debug15")
	{
		$DestinationFolder = "$documentsFolder\WindowsPowerShell\Modules\SharePointPnPPowerShell2013"
	} elseif($ConfigurationName -like "Debug16")
	{
		$DestinationFolder = "$documentsFolder\WindowsPowerShell\Modules\SharePointPnPPowerShell2016"
	} else {
		$DestinationFolder = "$documentsFolder\WindowsPowerShell\Modules\SharePointPnPPowerShellOnline"
	}
	
	# Module folder there?
	if(Test-Path $DestinationFolder)
	{
		# Yes, empty it
		Remove-Item $DestinationFolder\*
	} else {
		# No, create it
		Write-Host "Creating target folder: $DestinationFolder"
		New-Item -Path $DestinationFolder -ItemType Directory -Force >$null # Suppress output
	}

	Write-Host "Copying files from $TargetDir to $DestinationFolder"
	Copy-Item "$TargetDir\*.dll" -Destination "$DestinationFolder"
	Copy-Item "$TargetDir\*help.xml" -Destination "$DestinationFolder"
	switch($ConfigurationName)
	{
		"Debug15" {
			Copy-Item "$TargetDir\ModuleFiles\SharePointPnPPowerShell2013.psd1" -Destination  "$DestinationFolder"
			Copy-Item "$TargetDir\ModuleFiles\SharePointPnP.PowerShell.2013.Commands.Format.ps1xml" -Destination "$DestinationFolder"
			Copy-Item "$TargetDir\ModuleFiles\SharePointPnPPowerShell2013.psm1" -Destination "$DestinationFolder"
		} 
		"Debug16" {
			Copy-Item "$TargetDir\ModuleFiles\SharePointPnPPowerShell2016.psd1" -Destination  "$DestinationFolder"
			Copy-Item "$TargetDir\ModuleFiles\SharePointPnP.PowerShell.2016.Commands.Format.ps1xml" -Destination "$DestinationFolder"
			Copy-Item "$TargetDir\ModuleFiles\SharePointPnPPowerShell2016.psm1" -Destination "$DestinationFolder"
		} 
		"Debug" {
			Copy-Item "$TargetDir\ModuleFiles\SharePointPnPPowerShellOnline.psd1" -Destination  "$DestinationFolder"
			Copy-Item "$TargetDir\ModuleFiles\SharePointPnP.PowerShell.Online.Commands.Format.ps1xml" -Destination "$DestinationFolder"
			Copy-Item "$TargetDir\ModuleFiles\SharePointPnPPowerShellOnline.psm1" -Destination "$DestinationFolder"
		}
	}
} elseif ($ConfigurationName -like "Release*")
{
    $documentsFolder = [environment]::getfolderpath("mydocuments");
	switch($ConfigurationName)
	{
		"Release15" 
		{
			$DestinationFolder = "$documentsFolder\WindowsPowerShell\Modules\SharePointPnPPowerShell2013"
		}
		"Release16"
		{
			$DestinationFolder = "$documentsFolder\WindowsPowerShell\Modules\SharePointPnPPowerShell2016"
		}
		"Release"
		{
			$DestinationFolder = "$documentsFolder\WindowsPowerShell\Modules\SharePointPnPPowerShellOnline"
		}
	}

	# Module folder there?
	if(Test-Path $DestinationFolder)
	{
		# Yes, empty it
		Remove-Item $DestinationFolder\*
	} else {
		# No, create it
		Write-Host "Creating target folder: $DestinationFolder"
		New-Item -Path $DestinationFolder -ItemType Directory -Force >$null # Suppress output
	}

	Write-Host "Copying files from $TargetDir to $DestinationFolder"
	Copy-Item "$TargetDir\*.dll" -Destination "$DestinationFolder"
	Copy-Item "$TargetDir\*help.xml" -Destination "$DestinationFolder"
	switch($ConfigurationName)
	{
		"Release15" {
			Copy-Item "$TargetDir\ModuleFiles\SharePointPnPPowerShell2013.psd1" -Destination  "$DestinationFolder"
			Copy-Item "$TargetDir\ModuleFiles\SharePointPnP.PowerShell.2013.Commands.Format.ps1xml" -Destination "$DestinationFolder"
		} 
		"Release16" {
			Copy-Item "$TargetDir\ModuleFiles\SharePointPnPPowerShell2016.psd1" -Destination  "$DestinationFolder"
			Copy-Item "$TargetDir\ModuleFiles\SharePointPnP.PowerShell.2016.Commands.Format.ps1xml" -Destination "$DestinationFolder"
		} 
		"Release" {
			Copy-Item "$TargetDir\ModuleFiles\SharePointPnPPowerShellOnline.psd1" -Destination  "$DestinationFolder"
			Copy-Item "$TargetDir\ModuleFiles\SharePointPnP.PowerShell.Online.Commands.Format.ps1xml" -Destination "$DestinationFolder"
		}
	}
}

	