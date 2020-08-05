#if !ONPREMISES
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;
using PnP.PowerShell.Commands.Model.Teams;
using PnP.PowerShell.Commands.Utilities;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsCommon.New, "PnPTeamsApp")]
    [CmdletHelp("Adds an app to the Teams App Catalog.",
        Category = CmdletHelpCategory.Teams,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = "PS:> New-PnPTeamsApp -Path c:\\myapp.zip",
       Remarks = "Adds the app as defined in the zip file to the Teams App Catalog",
       SortOrder = 1)]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.Directory_ReadWrite_All)]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.AppCatalog_ReadWrite_All)]
    public class NewTeamsApp : PnPGraphCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The path pointing to the packaged/zip file containing the app")]
        public string Path;

        protected override void ExecuteCmdlet()
        {
            if (!System.IO.Path.IsPathRooted(Path))
            {
                Path = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Path);
            }

            if (System.IO.File.Exists(Path))
            {
                try
                {
                    var bytes = System.IO.File.ReadAllBytes(Path);
                    TeamsUtility.AddAppAsync(HttpClient, AccessToken, bytes).GetAwaiter().GetResult();
                }
                catch (GraphException ex)
                {
                    if (ex.Error != null)
                    {
                        throw new PSInvalidOperationException(ex.Error.Message);
                    }
                    else
                    {
                        throw new PSInvalidOperationException(ex.Message);
                    }
                }
            }
            else
            {
                new PSArgumentException("File not found");
            }
        }
    }
}
#endif