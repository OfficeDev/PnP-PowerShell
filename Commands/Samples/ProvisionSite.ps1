$basePath = "C:\DeploymentFiles"
$themePath = "/sites/psdemo/_catalogs/theme/15" # ServerRelativeUrl
$tenant = "<yourtenantname>"

$tenantAdmin  = Get-Credential -Message "Enter Tenant Administrator Credentials"
Connect-PnPOnline -Url https://<tenant>-admin.sharepoint.com -Credentials $tenantAdmin

New-PnPTenantSite -Title "PS Site" -Url "https://$tenant.sharepoint.com/sites/psdemo" -Owner $tenantAdmin -Lcid 1033 -TimeZone 24 -Template STS#0 -RemoveDeletedSite -Wait

# Connect with the tenant admin credentials to the newly created site collection
Connect-PnPOnline -Url https://$tenant.sharepoint.com/sites/psdemo -Credentials $tenantAdmin

# Set Property Bag key to designate the type of site you're creating
Set-PnPPropertyBagValue -Key "PNP_SiteType" -Value "PROJECT"

# Upload a theme
Add-PnPFile -Path "$basePath\contoso.spcolor" -Url "$themePath/contoso.spcolor"
Add-PnPFile -Path "$basePath\contoso.spfont" -Url "$themePath/contoso.spfont"
Add-PnPFile -Path "$basePath\contosobg.jpg" -Url "$themePath/contosobg.jpg"
Set-PnPTheme -ColorPaletteUrl "$themePath/contoso.spcolor" -FontSchemeUrl "$themePath/contoso.spfont" -BackgroundImageUrl "$themePath/contosobg.jpg"

# Add a list and add a field to the list.
New-PnPList -Title "Projects" -Template GenericList -Url "lists/projects" -QuickLaunchOptions on
Add-PnPField -List "Projects" -InternalName "ProjectManager" -DisplayName "Project Manager" -StaticName "ProjectManager" -Type User -AddToDefaultView