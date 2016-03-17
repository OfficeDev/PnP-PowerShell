using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.PowerShell.CmdletHelpAttributes;
using OfficeDevPnP.Core.Utilities;

namespace OfficeDevPnP.PowerShell.Commands
{
    [Cmdlet("Ensure", "SPOFolder")]
    [CmdletHelp("Returns a folder given a site relative path, and will create it if it not exists.",
        Category = CmdletHelpCategory.Webs)]
    [CmdletExample(
        Code = @"PS:> Ensure-SPOFolder -SiteRelativePath ""demofolder/subfolder""",
        Remarks = "Creates a folder called subfolder in a folder called demofolder with located in the root folder of the site. If the folder hierarchy does not exist, it will be created.",
        SortOrder = 1)]
    public class EnsureFolder : SPOWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Site Relative Folder Path")]
        public string SiteRelativePath = string.Empty;

        protected override void ExecuteCmdlet()
        {
            WriteObject(SelectedWeb.EnsureFolderPath(SiteRelativePath));
        }
    }
}
