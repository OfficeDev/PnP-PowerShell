
$configFilePath = "D:\ModernPagesConfig.xlsx" #Please update with relevant Filename/Filepath

<# For troubleshooting, you can change PowerShell Verbose preference so that verbose output is displayed

$oldverbose = $VerbosePreference #stores current verbose preference, so that it can be reset later on (see end of script)
$VerbosePreference = "Continue" #enables Verbose output to be displayed
#>

function Add-PnPModernListWebPart() {
   param(
      [Parameter(Mandatory)]
      [ValidateSet("Library", "List")]
      [String] $WebPartType,

      [Parameter(Mandatory)]
      [ValidateScript( {
            if (-not (Get-PnPClientSidePage $_)) {
               throw "The [$_] page does not exist"
            }
            else {
               $true
            }           
         })]
      [String] $PageName,

      [Parameter(Mandatory)]
      [ValidateScript( {        
            if (-not (Get-PnPList -Identity $_)) {
               throw "The [$_] list does not exist"
            }
            else {
               $true
            }  
         })]
      [String] $ListName,

      [Parameter(Mandatory)]
      [ValidateScript( {
            if (-not (Get-PnPView -List $listName -Identity $_)) {
               throw "The [$_] view does not exist in the [$listName] $WebPartType"
            }
            else {
               $true
            }        
         })]
      [String] $ViewName,
        
      [String] $WebPartTitle, #Will default to "'listTitle' - 'viewName'" if left blank
        
      [ValidateSet(0, 1, 2, 3, 4)] #0 is a validation option as it can be left blank
      [int] $WebPartHeight, 

      [int] $Section, #####ideally need to add some validation if/when a Get-PnPSection command is added to the PNP-PowerShell project#####

      [ValidateSet(0, 1, 2, 3)] #0 is a validation option as it can be left blank
      [int] $Column

      #[int] $Order #####Order parameter for Add-PnPClientSideWebPart doesn't appear to work currently#####
   )

   #Create hashtable to store the web part properties
   [hashtable]$webPartProperties

   #Set isDocumentLibrary property and add it to the web part properties hashtable
   If ($WebPartType -eq "Library") {
      $webPartProperties = @{"isDocumentLibrary" = $true; }
   }

   If ($WebPartType -eq "List") {
      $webPartProperties = @{"isDocumentLibrary" = $false; }
   }

   $list = Get-PnPList -Identity $ListName

   #Set List/Library ID property and add it to the web part properties hashtable
   $listId = $list.Id.ToString()
   $webPartProperties.Add("selectedListId", $listId)


   #Set List/Library URL property and add it to the web part properties hashtable
   $listUrl = $list.RootFolder.ServerRelativeUrl
   $webPartProperties.Add("selectedListUrl", $listUrl)


   #Set View ID property and add it to the web part properties hashtable    
   $view = Get-PnPView -List $ListName -Identity $ViewName
   $viewId = $view.Id.ToString()
   $webPartProperties.Add("selectedViewId", $viewId)
    

   #If a WebPart Title was provided in function call then add it to the web part properties hashtable
   If ($WebPartTitle) {
      $webPartProperties.Add("listTitle", $WebPartTitle)
   }

   #If no WebPart Title was provided in function call, then combine list title and view name and add them to the web part properties hashtable
   Else {
      $WebPartTitle = $list.Title + ' - ' + $ViewName
      $webPartProperties.Add("listTitle", $WebPartTitle)
   }

   #If WebPart Height was provided in function call then add it to the web part properties hashtable
   If ($WebPartHeight -ne 0) {
      $webPartProperties.Add("webpartHeightKey", $WebPartHeight)
   }

   If (($Section -eq 0) -and ($Column -eq 0)) {
      Write-Warning "The Section and Column fields for the [$WebPartTitle] web part have been left blank or have zero values"
      try {
         Add-PnPClientSideWebPart -Page $PageName -DefaultWebPartType List -WebPartProperties $webPartProperties
      }
      catch {
         Write-Error "Unable to add [$WebPartTitle] web part to the [$PageName] page. Check that that there is a section [$Section] with [$Column] columns"
      }
   }
   Else {
      try {                     
         Add-PnPClientSideWebPart -Page $PageName -DefaultWebPartType List -WebPartProperties $webPartProperties -Section $Section -Column $Column -ErrorAction Stop #-Order $Order
      }
      catch {
         Write-Error "Unable to add [$WebPartTitle] web part to the [$PageName] page. Check that that there is a section [$Section] with [$Column] columns"
      }
   }
}
#Import 'Site' worksheet from the excel configuration spreadsheet

try {
   Write-Verbose "Importing site worksheet from the excel configuration file: [$configFilePath]"
   $xlSiteSheet = Import-Excel -Path $configFilePath -WorkSheetname Site #Opens the excel file and imports the Site worksheet
}
catch {
   Write-Error "Unable to open spreadsheet from [$configFilePath] or 'Site' worksheet does not exist"
   EXIT
}

#Save site url to a variable and connect to the site
try {
   Write-Verbose "Importing site url from the site worksheet."
   $site = $xlSiteSheet[0].'TargetSiteUrl'  #gets the first site url value from the TargetSiteUrl column
    
   Write-Verbose "Connecting to site: $site"
   Connect-PnPOnline -Url $site
}
catch {
   Write-Error "Unable to open site at [$site]"
   EXIT
}

#Import 'ModernPages' worksheet from the excel configuration spreadsheet
try {
   Write-Verbose "Importing ModernPages worksheet from the excel configuration file."
   $xlPagesSheet = Import-Excel -Path $configFilePath -WorkSheetname ModernPages #Imports the Libraries worksheet
}
catch { 
   Write-Error "Unable to open spreadsheet from [$configFilePath] or 'ModernPages' worksheet does not exist."
   EXIT
}

Write-Verbose "Begin adding ModernPages to the site."

#Import each worksheet row and add modern site pages and relevant sections to the site
ForEach ($row in $xlPagesSheet) {
   $page = $row.'PageName'; #saves value from the worksheet 'PageName' column to a variable
   $layout = $row.'LayoutType'; #saves value from the worksheet 'LayoutType' column to a variable
   $sections = $row.'Sections'; #saves value from the worksheet 'Sections' column to a variable

   #Add new modern site page
   try {
      Write-Verbose "Adding the $page page with $layout layout."
      Add-PnPClientSidePage -Name $page -LayoutType $layout
   }
   catch {
      Write-Warning "Unable to add [$page] page."
   }
   
   #Add sections to the new page (in the order specified in the worksheet)
   if ($sections) {

      $arraySections = $sections.split("`n"); #splits string into an array of strings - looking for newline character as a separator
      $sectionOrder = 1

      ForEach ($section in $arraySections) {
         Write-Verbose "Adding the $section section to the $page page. Section order is $sectionOrder."
         try {
            Add-PnPClientSidePageSection -Page $page  -SectionTemplate $section -Order $sectionOrder
         }
         catch {
            Write-Warning "Unable to add [$section] section to [$page] page. Ensure [$section] is a valid Section Template value (e.g. OneColumn, TwoColumn, ThreeColumn etc)"
         }
         $sectionOrder++
      }
   }
} 

try {
   Write-Verbose "Importing ModernListLibraryWebParts worksheet from the excel configuration file."
   $xlWebPartsSheet = Import-Excel -Path $configFilePath -WorkSheetname ModernListLibraryWebParts #Imports the Libraries worksheet
}
catch { 
   Write-Error "Unable to open spreadsheet from [$configFilePath] or 'ModernListLibraryWebParts' worksheet does not exist."
   EXIT
}

Write-Verbose "Begin adding Modern List / Library web parts to pages:"

#iterate through the worksheet rows and add web parts to the pages
ForEach ($row in $xlWebPartsSheet) {
   $page = $row.'PageName'; #saves value in page name column to a variable 
   $section = $row.'Section'; #saves value in Section column to a variable
   $column = $row.'Column'; #saves values in sections column to a variable
   #$order = $row.'Order';#saves value in Order column to a variable - have commented this out as order parameter for Add-PnPClientSideWebPart command doesn't seem to work currently
   $wpType = $row.'WebPartType'; #saves value in WebPartType column to a variable
   $listLibraryName = $row.'ListOrLibraryName'; #saves value in ListOrLibraryName column to a variable
   $viewName = $row.'ViewName'; #saves value in ViewName column to a variable
   $wpTitle = $row.'WebPartTitle'; #saves value in WebPartTitle column to a variable
   $wpHeight = $row.'WebPartHeight'; #saves value in WebPartHeight column to a variable

   Write-Verbose "Adding web part to the '$page' page with title [$wpTitle]"
   Write-Verbose "web part will be added with '$viewName' view for the '$listLibraryName' $wpType"
   Write-Verbose "web part will be added to column $column in section $section height is set to $wpHeight" #order is: $order
    
   Add-PnPModernListWebPart -PageName $page -WebPartType $wpType -ListName $listLibraryName -ViewName $viewName -WebPartHeight $wpHeight -WebPartTitle $wpTitle -Section $section -Column $column #-Order $order
}

<#This allows you to rollback the Verbose preference value to the original, assuming it was changed for troubleshooting
$VerbosePreference = $oldverbose

#>