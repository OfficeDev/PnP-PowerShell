param(
    [switch]$syncPermissions
)

. $PSScriptRoot/shared.ps1

Write-Output @"
  ___  ___       _____                                                
  |  \/  |      |  __ \                                               
  | .  . |_ __  | |  \/ _____   _____ _ __ _ __   __ _ _ __   ___ ___ 
  | |\/| | '__| | | __ / _ \ \ / / _ \ '__| '_ \ / _`` | '_ \ / __/ _ \
  | |  | | |_   | |_\ \ (_) \ V /  __/ |  | | | | (_| | | | | (_|  __/
  \_|  |_/_(_)   \____/\___/ \_/ \___|_|  |_| |_|\__,_|_| |_|\___\___|
                                                         by Puzzlepart
  Code by: @mikaelsvenson

"@

$variablesSet = CheckEnvironmentalVariables
if ($variablesSet -eq $false) {    
    Write-Output "Missing one of the following environmental variables: TenantURL, PrimarySiteCollectionOwnerEmail, SiteDirectoryUrl, AppId, AppSecret"
    exit
}
Connect -Url "$tenantURL$siteDirectorySiteUrl"

$query = 'spcontenttype="team site" path:' + "$tenantURL$siteDirectorySiteUrl$siteDirectoryList"
$res = Submit-PnPSearchQuery -Query $query -All -SelectProperties "PZLSiteURLOWSURLH", "ListItemID"

foreach ($site in $res.ResultRows) {
    $url = $site["PZLSiteURLOWSURLH"].split(',')[0]
    $itemId = $site["ListItemID"]
    Connect -Url "$tenantURL$siteDirectorySiteUrl"
    $siteItem = Get-PnPListItem -List $siteDirectoryList -Id $itemId

    Write-Output "Processing $url - $itemId"
    if ($syncPermissions) {        
        SyncPermissions -siteUrl $url -item $siteItem
    }
}

Disconnect-PnPOnline