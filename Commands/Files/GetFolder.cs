using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Utilities;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using File = Microsoft.SharePoint.Client.File;

namespace SharePointPnP.PowerShell.Commands.Files
{
    [Cmdlet(VerbsCommon.Get, "PnPFolder")]
    [CmdletAlias("Get-SPOFolder")]
    [CmdletHelp("Return a folder object", Category = CmdletHelpCategory.Files,
        DetailedDescription = "Retrieves a folder if it exists. Use Ensure-PnPFolder to create the folder if it does not exist.",
        OutputType = typeof(Folder),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.file.aspx")]
    [CmdletExample(
        Code = @"PS:> Get-PnPFolder -RelativeUrl ""Shared Documents""",
        Remarks = "Returns the folder called 'Shared Documents' which is located in the root of the current web",
        SortOrder = 1
        )]
    [CmdletExample(
        Code = @"PS:> Get-PnPFolder -RelativeUrl ""/sites/demo/Shared Documents""",
        Remarks = "Returns the folder called 'Shared Documents' which is located in the root of the current web",
        SortOrder = 1
        )]
    [CmdletRelatedLink(
        Text = "Ensure-PnPFolder", 
        Url = "https://github.com/OfficeDev/PnP-PowerShell/blob/master/Documentation/EnsureSPOFolder.md")]
    public class GetFolder : SPOWebCmdlet
    {

        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, HelpMessage = "Site or server relative URL of the folder to retrieve. In the case of a server relative url, make sure that the url starts with the managed path as the current web.")]
        public string RelativeUrl;

        protected override void ExecuteCmdlet()
        {

            var folder = SelectedWeb.GetFolderByServerRelativeUrl(RelativeUrl);
            
            folder.EnsureProperties(f => f.Name, f => f.ServerRelativeUrl);

            WriteObject(folder);
        }
    }
}
