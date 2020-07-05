param($ProjectDir, $ConfigurationName, $TargetDir, $TargetFileName, $SolutionDir)

if($ConfigurationName -like "Debug*")
{
	$documentsFolder = [environment]::getfolderpath("mydocuments");
	if($TargetDir -like "*Core*")
	{
		$DestinationFolder = "$documentsFolder\PowerShell\Modules\PnPPowerShellCore"
	} else {
		if($ConfigurationName -like "Debug15")
		{
			$DestinationFolder = "$documentsFolder\WindowsPowerShell\Modules\SharePointPnPPowerShell2013"
		} elseif($ConfigurationName -like "Debug16")
		{
			$DestinationFolder = "$documentsFolder\WindowsPowerShell\Modules\SharePointPnPPowerShell2016"
		} elseif($ConfigurationName -like "Debug19")
		{
			$DestinationFolder = "$documentsFolder\WindowsPowerShell\Modules\SharePointPnPPowerShell2019"
		} else {
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
	Try {
		Copy-Item "$TargetDir\*.dll" -Destination "$DestinationFolder"
		Copy-Item "$TargetDir\*help.xml" -Destination "$DestinationFolder"
		if($TargetDir -like "*Core*")
		{
			Copy-Item "$TargetDir\ModuleFiles\*" -Destination "$DestinationFolder"
		} else {
			switch($ConfigurationName)
			{
				"Debug15" {
					Copy-Item "$TargetDir\ModuleFiles\SharePointPnPPowerShell2013.psd1" -Destination  "$DestinationFolder"
					Copy-Item "$TargetDir\ModuleFiles\SharePointPnP.PowerShell.2013.Commands.Format.ps1xml" -Destination "$DestinationFolder"
					if(Test-Path -Path "$TargetDir\ModuleFiles\SharePointPnPPowerShell2013Aliases.psm1")
					{
						Copy-Item "$TargetDir\ModuleFiles\SharePointPnPPowerShell2013Aliases.psm1" -Destination "$DestinationFolder"
					}
				} 
				"Debug16" {
					Copy-Item "$TargetDir\ModuleFiles\SharePointPnPPowerShell2016.psd1" -Destination  "$DestinationFolder"
					Copy-Item "$TargetDir\ModuleFiles\SharePointPnP.PowerShell.2016.Commands.Format.ps1xml" -Destination "$DestinationFolder"
					if(Test-Path -Path "$TargetDir\ModuleFiles\SharePointPnPPowerShell2016Aliases.psm1")
					{
						Copy-Item "$TargetDir\ModuleFiles\SharePointPnPPowerShell2016Aliases.psm1" -Destination "$DestinationFolder"
					}
				} 
				"Debug19" {
					Copy-Item "$TargetDir\ModuleFiles\SharePointPnPPowerShell2019.psd1" -Destination  "$DestinationFolder"
					Copy-Item "$TargetDir\ModuleFiles\SharePointPnP.PowerShell.2019.Commands.Format.ps1xml" -Destination "$DestinationFolder"
					if(Test-Path -Path "$TargetDir\ModuleFiles\SharePointPnPPowerShell2019Aliases.psm1")
					{
						Copy-Item "$TargetDir\ModuleFiles\SharePointPnPPowerShell2019Aliases.psm1" -Destination "$DestinationFolder"
					}
				} 
				"Debug" {
					Copy-Item "$TargetDir\ModuleFiles\SharePointPnPPowerShellOnline.psd1" -Destination  "$DestinationFolder"
					Copy-Item "$TargetDir\ModuleFiles\SharePointPnP.PowerShell.Online.Commands.Format.ps1xml" -Destination "$DestinationFolder"
					if(Test-Path -Path "$TargetDir\ModuleFiles\SharePointPnPPowerShellOnlineAliases.psm1")
					{
						Copy-Item "$TargetDir\ModuleFiles\SharePointPnPPowerShellOnlineAliases.psm1" -Destination "$DestinationFolder"
					}
				}
			}
		}
	}
	Catch
	{
		exit 1
	}
} elseif ($ConfigurationName -like "Release*")
{
    $documentsFolder = [environment]::getfolderpath("mydocuments");
	if($TargetDir -like "*Core*")
	{
		$DestinationFolder = "$documentsFolder\PowerShell\Modules\PnPPowerShellCore"
	} else {
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
			"Release19"
			{
				$DestinationFolder = "$documentsFolder\WindowsPowerShell\Modules\SharePointPnPPowerShell2019"
			}
			"Release"
			{
				$DestinationFolder = "$documentsFolder\WindowsPowerShell\Modules\SharePointPnPPowerShellOnline"
			}
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
	Try
	{
		Copy-Item "$TargetDir\*.dll" -Destination "$DestinationFolder"
		Copy-Item "$TargetDir\*help.xml" -Destination "$DestinationFolder"
		if($TargetDir -like "*Core*")
		{
			Copy-Item "$TargetDir\ModuleFiles\PnPPowerShellCore.psd1" -Destination "$DestinationFolder"
			Copy-Item "$TargetDir\ModuleFiles\PnP.PowerShell.Core.Format.ps1xml" -Destination "$DestinationFolder"
		} else {
			switch($ConfigurationName)
			{
				"Release15" {
					Copy-Item "$TargetDir\ModuleFiles\SharePointPnPPowerShell2013.psd1" -Destination  "$DestinationFolder"
					Copy-Item "$TargetDir\ModuleFiles\SharePointPnP.PowerShell.2013.Commands.Format.ps1xml" -Destination "$DestinationFolder"
					if(Test-Path -Path "$TargetDir\ModuleFiles\SharePointPnPPowerShell2013Aliases.psm1")
					{
						Copy-Item "$TargetDir\ModuleFiles\SharePointPnPPowerShell2013Aliases.psm1" -Destination "$DestinationFolder"
					}
				} 
				"Release16" {
					Copy-Item "$TargetDir\ModuleFiles\SharePointPnPPowerShell2016.psd1" -Destination  "$DestinationFolder"
					Copy-Item "$TargetDir\ModuleFiles\SharePointPnP.PowerShell.2016.Commands.Format.ps1xml" -Destination "$DestinationFolder"
					if(Test-Path -Path "$TargetDir\ModuleFiles\SharePointPnPPowerShell2016Aliases.psm1")
					{
						Copy-Item "$TargetDir\ModuleFiles\SharePointPnPPowerShell2016Aliases.psm1" -Destination "$DestinationFolder"
					}
				} 
				"Release19" {
					Copy-Item "$TargetDir\ModuleFiles\SharePointPnPPowerShell2019.psd1" -Destination  "$DestinationFolder"
					Copy-Item "$TargetDir\ModuleFiles\SharePointPnP.PowerShell.2019.Commands.Format.ps1xml" -Destination "$DestinationFolder"
					if(Test-Path -Path "$TargetDir\ModuleFiles\SharePointPnPPowerShell2019Aliases.psm1")
					{
						Copy-Item "$TargetDir\ModuleFiles\SharePointPnPPowerShell2019Aliases.psm1" -Destination "$DestinationFolder"
					}
				} 
				"Release" {
					Copy-Item "$TargetDir\ModuleFiles\SharePointPnPPowerShellOnline.psd1" -Destination  "$DestinationFolder"
					Copy-Item "$TargetDir\ModuleFiles\SharePointPnP.PowerShell.Online.Commands.Format.ps1xml" -Destination "$DestinationFolder"		
					if(Test-Path -Path "$TargetDir\ModuleFiles\SharePointPnPPowerShellOnlineAliases.psm1")
					{
						Copy-Item "$TargetDir\ModuleFiles\SharePointPnPPowerShellOnlineAliases.psm1" -Destination "$DestinationFolder"
					}
				}
			}
		}
	} 
	Catch
	{
		exit 1
	}
}

	