Add-Type -assembly "System.IO.Compression.FileSystem"

Write-Host "------------------------------------------------------------"
Write-Host "| Creating WebJob zip files"
Write-Host "------------------------------------------------------------"
$webJobs = @("governance-daily","provisioning")
$dir = $(Get-Location).Path 
foreach( $webJob in $webJobs){   
    $zipFile = "$($dir)\$($webJob).zip"
    $tempDir = "$($dir)\dist\$($webJob)"
    
    if(Test-path $zipFile) {Remove-item $zipFile}
    if(Test-path $tempDir) {Remove-item -Recurse -Force $tempDir}
    New-Item -ItemType Directory $tempDir | Out-Null
    
    Write-Host "Copying files to $($tempDir)"
    copy-item "$($dir)\Engine\bundle"         -destination "$($tempDir)\bundle"    -recurse
    copy-item "$($dir)\Engine\resources"      -destination "$($tempDir)\resources" -recurse
    copy-item "$($dir)\Engine\*.ps1"          -destination $tempDir 
    copy-item "$($dir)\Engine\$($webJob).cmd" -destination $tempDir
    
    Write-Host "Creating $($zipFile)" 
    [IO.Compression.ZipFile]::CreateFromDirectory($tempDir, $zipFile)
    Remove-item -Recurse -Force $tempDir
}
Write-Host "Zip files for '$($webJobs)' created in current folder"