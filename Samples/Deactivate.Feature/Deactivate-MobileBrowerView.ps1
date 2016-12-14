# Author: Rajesh Sitaraman | http://rjesh.com
# THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
# FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
# LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#
<#
.SYNOPSIS
Deactivate "Mobile browser view feature" in all sub sites in the site collection.

.EXAMPLE
PS C:\> .\Deactivate-MobileBrowerView.ps1 -Url https://yoursitecollectionURL

.EXAMPLE
PS C:\> .\Deactivate-MobileBrowerView.ps1 -Url https://yoursitecollectionURL -Force

.EXAMPLE
PS C:\> $creds = Get-Credential
PS C:\> .\Deactivate-MobileBrowerView.ps1 -Url https://yoursitecollectionURL -Credentials $creds
#>

[CmdletBinding()]
param
(
    [Parameter(Mandatory = $false, HelpMessage="Optional administration credentials")]
    [PSCredential] $Credentials,
    [Parameter(Mandatory = $true, HelpMessage="Required Site Url")]
    [string] $Url,
    [Parameter(Mandatory = $false, HelpMessage="Optional Force switch")]
    [switch]$Force
)


if($Credentials -eq $null)
{
	$Credentials = Get-Credential -Message "Enter your credentials"
}

# Gets all webs in site collection
function GetWebs
{
    $separator = ","
    $sites = Get-SPOSite
	Get-SPOSubWebs -Web $sites.RootWeb -Recurse | foreach { $subIds += $_.ServerRelativeUrl + $separator }
	#$option = [System.StringSplitOptions]::RemoveEmptyEntries

	return $subIds.Split($separator)
}

try
{
    Connect-SPOnline $Url -Credentials $Credentials
	GetWebs | foreach {
		Write-Host "Really working hard in site $_" -ForegroundColor Yellow
        $cWeb = Get-SPOWeb -Identity $_
        if ($Force) {
            Disable-SPOFeature -Identity d95c97f3-e528-4da2-ae9f-32b3535fbb59 -Scope Web -Web $cWeb -Force
        }
        else {
            Disable-SPOFeature -Identity d95c97f3-e528-4da2-ae9f-32b3535fbb59 -Scope Web -Web $cWeb
        }

		Write-Host "Feature Deactivated." -ForegroundColor Green
	}
}
catch
{
    Write-Host -ForegroundColor Red "Exception occurred!"
    Write-Host -ForegroundColor Red "Exception Type: $($_.Exception.GetType().FullName)"
    Write-Host -ForegroundColor Red "Exception Message: $($_.Exception.Message)"
}

