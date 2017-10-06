# This script creates site collections and/or sub webs from csv configuration template.

# Change the policy settings if needed.
#$policy = Get-ExecutionPolicy
#if ($policy -ne 'RemoteSigned') {
#    Set-ExecutionPolicy RemoteSigned
#}

if (-not (Get-Module -ListAvailable -Name SharePointPnPPowerShellOnline)) 
{
    Install-Module SharePointPnPPowerShellOnline
}

Import-Module SharePointPnPPowerShellOnline

# Gets or Sets the csv data.
$csvConfig = $null

# Gets or Sets the tenant admin credentials.
$credentials = $null

# Windows Credentials Manager credential label.
$winCredentialsManagerLabel = "SPFX"

# Imports the csv configuration template.
try 
{ 
    $csvConfig = Import-Csv -Path "./CreateSites.csv"
} 
catch 
{
    # Prompts for csv configuration template path.
    $csvConfigPath = Read-Host -Prompt "Please enter the csv configuration template full path"
    $csvConfig = Import-Csv -Path $csvConfigPath
}

# Gets stored credentials from the Windows Credential Manager or show prompt.
# How to use windows credential manager:
# https://github.com/SharePoint/PnP-PowerShell/wiki/How-to-use-the-Windows-Credential-Manager-to-ease-authentication-with-PnP-PowerShell
if((Get-PnPStoredCredential -Name $winCredentialsManagerLabel) -ne $null)
{
    $credentials = $winCredentialsManagerLabel
}
else
{
    # Prompts for credentials, if not found in the Windows Credential Manager.
    $email = Read-Host -Prompt "Please enter tenant admin email"
    $pass = Read-host -AsSecureString "Please enter tenant admin password"
    $credentials = New-Object –TypeName "System.Management.Automation.PSCredential" –ArgumentList $email, $pass
}

if($credentials -eq $null -or $csvConfig -eq $null) 
{
    Write-Host "Error: Not enough details." -ForegroundColor DarkRed
    exit 1
}

# Iterates over the csv confuguration template rows and creates site collections or sub sites.
foreach ($item in $csvConfig) 
{
    $tenantUrl = $item.RootUrl -replace ".sharepoint.com.+", ".sharepoint.com"
    $siteUrl = -join($item.RootUrl, "/", $item.SiteUrl)
    
    if ($item.Type -eq "SiteCollection") 
    {
        Connect-PnPOnline $tenantUrl -Credentials $credentials

        Write-Host "Provisioning site collection $siteUrl" -ForegroundColor Yellow
         
        if(Get-PnPTenantSite | where {$_.Url -eq $siteUrl}) 
        {
            Write-Host "Site collection $siteUrl exists. Moving to the next one." -ForegroundColor Yellow
            continue
        }

        # Creates new site collection.
        New-PnPTenantSite -Owner $item.Owner -TimeZone $item.TimeZone -Title $item.Title -Url $siteUrl -Template $item.Template -Lcid $item.Locale -Wait

        Write-Host "SiteCollection $siteUrl successfully created." -ForegroundColor Green
    }  
    elseif ($item.Type -eq "SubWeb") 
    {

        $siteCollectionUrl = $item.RootUrl

        Connect-PnPOnline $siteCollectionUrl -Credentials $credentials

        Write-Host "Provisioning sub web $siteUrl." -ForegroundColor Yellow
        
        if(Get-PnPSubWebs | where {$_.Url -eq $siteUrl}) 
        {
            Write-Host "Sub web $siteUrl exists. Moving to the next one." -ForegroundColor Yellow
            continue
        }
        
        # Creates new sub web.
        New-PnPWeb -Template $item.Template -Title $item.Title -Url $item.SiteUrl -Locale $item.Locale

        Write-Host "Sub web $siteUrl successfully created." -ForegroundColor Green
    }
}