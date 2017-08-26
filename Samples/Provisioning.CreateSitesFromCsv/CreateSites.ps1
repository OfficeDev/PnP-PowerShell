# This script creates site collections and/or sub webs from CSV file configuration.
# Please note: 
# - Only sub webs one level/leaf deeper from the root site can be created. The script and the csv data have to be adjusted, if sub webs should be created on two or more levels/leafs away from the root web.
# - The csv sites are created from top to bottom so sub webs to be created should be listed after site collections that do not exist.

# Define variables.

# The root directory for the sctipts and the csv file.
$rootdir = "./"

# The directory path for the csv file.
$createSitesCsvPath = -join($rootdir, "CreateSites.csv")

# User email and password stored in a txt files.
$credentialsFilePath = -join($rootdir, "credentials.txt")

# Change the policy settings if needed.
#$policy = Get-ExecutionPolicy
#if ($policy -ne 'RemoteSigned') {
#    Set-ExecutionPolicy RemoteSigned
#}

# Install and import the PnP SharePoint modules.
if (-not (Get-Module -ListAvailable -Name SharePointPnPPowerShellOnline)) {
    Install-Module SharePointPnPPowerShellOnline
}

Import-Module SharePointPnPPowerShellOnline

# Add/Save user credentials to txt file.
if(-not (Test-Path $credentialsFilePath)){
    $user = Read-Host -Prompt "Please enter your email"
    $pass = Read-host -AsSecureString | ConvertFrom-SecureString
    "$user`n$pass" | Out-File $credentialsFilePath
}

# Get user credentials from txt file.
$email = (Get-Content $credentialsFilePath)[0]
$password = (Get-Content $credentialsFilePath)[1] | ConvertTo-SecureString
$credentials = New-Object –TypeName "System.Management.Automation.PSCredential" –ArgumentList $email, $password

# Create site collections and sub sites based on the csv file config data.
# Test comment
$createSitesConfig = Import-Csv -Path $createSitesCsvPath

# Iterate over the csv rows and create site collections or sub sites depending on the 'Type' column from the csv file.
foreach ($item in $createSitesConfig)
{
    $tenantUrl = $item.RootUrl -replace ".sharepoint.com.+", ".sharepoint.com"
    $siteUrl = -join($item.RootUrl, "/", $item.SiteUrl)
    
    if ($item.Type -eq "SiteCollection")
    {
        Connect-PnPOnline $tenantUrl -Credentials $credentials

        Write-Host "Connecting to $tenantUrl" -ForegroundColor Green

        Write-Host "Provisioning SiteCollection $siteUrl" -ForegroundColor Yellow

        if(Get-PnPTenantSite | where{$_.Url -eq $siteUrl}){
            # Please note this will remove any existing site collection at the specified url.

            Write-Host "$siteUrl exists. Removing the site collection." -ForegroundColor Yellow

            Remove-PnPTenantSite -Url $siteUrl -Force
        }

        # Create new Tenant Site Collection.
        New-PnPTenantSite -Owner $item.Owner -TimeZone 0 -Title $item.Title -Url $siteUrl -Template $item.Template -Wait

        Write-Host "SiteCollection $siteUrl successfully created." -ForegroundColor Yellow
    }


    if ($item.Type -eq "SubWeb")
    {
        $siteCollectionUrl = $item.RootUrl

        Connect-PnPOnline $siteCollectionUrl -Credentials $credentials

        Write-Host "Connecting to $siteCollectionUrl" -ForegroundColor Green

        Write-Host "Provisioning sub web $siteUrl." -ForegroundColor Yellow
        
        if(Get-PnPSubWebs | where{$_.Url -eq $siteUrl}){
            # Please note this will remove any existing sub web at the specified url.

            Write-Host "$siteUrl exists. Removing the sub web." -ForegroundColor Yellow

            Remove-PnPWeb -Url $item.SiteUrl -Force
        }
        
        # Create new site collection sub web.
        New-PnPWeb -Template $item.Template -Title $item.Title -Url $item.SiteUrl -Locale 1033

        Write-Host "Sub web $siteUrl successfully created." -ForegroundColor Yellow
    }
}