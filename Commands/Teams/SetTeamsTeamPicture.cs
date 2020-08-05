#if !ONPREMISES
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;
using PnP.PowerShell.Commands.Utilities;
using System.IO;
using System.Management.Automation;
using System.Threading.Tasks;

namespace PnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsCommon.Set, "PnPTeamsTeamPicture")]
    [CmdletHelp("Sets the picture of an existing team.",
        DetailedDescription = "Notice that this cmdlet will immediately return but it can take a few hours before the changes are reflected in Microsoft Teams.",
        Category = CmdletHelpCategory.Teams,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = "PS:> Set-PnPTeamsTeamPicture -Team \"MyTeam\" -Path \"c:\\myimage.jpg\"",
       Remarks = "Updates the channel called 'MyChannel' to have the display name set to 'My Channel'",
       SortOrder = 1)]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.Group_ReadWrite_All)]
    public class SetTeamsTeamPicture : PnPGraphCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Specify the group id, mailNickname or display name of the team to use.", ValueFromPipeline = true)]
        public TeamsTeamPipeBind Team;

        [Parameter(Mandatory = true, HelpMessage = "The path to the image file.")]
        public string Path;

        protected override void ExecuteCmdlet()
        {
            var groupId = Team.GetGroupId(HttpClient, AccessToken);
            if (groupId != null)
            {
                if (!System.IO.Path.IsPathRooted(Path))
                {
                    Path = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Path);

                }
                if (System.IO.File.Exists(Path))
                {
                    var contentType = "";
                    var fileInfo = new FileInfo(Path);
                    switch (fileInfo.Extension)
                    {
                        case ".jpg":
                        case ".jpeg":
                            {
                                contentType = "image/jpeg";
                                break;
                            }
                        case ".png":
                            {
                                contentType = "image/png";
                                break;
                            }
                    }
                    if (string.IsNullOrEmpty(contentType))
                    {
                        throw new PSArgumentException("File is not of a supported content type (jpg/png)");
                    }
                    var byteArray = System.IO.File.ReadAllBytes(Path);
                    TeamsUtility.SetTeamPictureAsync(HttpClient, AccessToken, groupId, byteArray, contentType).GetAwaiter().GetResult();
                }
                else
                {
                    throw new PSArgumentException("File not found");
                }
            }
            else
            {
                throw new PSArgumentException("Team not found");
            }

        }
    }
}
#endif