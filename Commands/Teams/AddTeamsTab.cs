#if !ONPREMISES
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;
using PnP.PowerShell.Commands.Model.Teams;
using PnP.PowerShell.Commands.Utilities;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsCommon.Add, "PnPTeamsTab")]
    [CmdletHelp("Adds a tab to an existing Channel",
        Category = CmdletHelpCategory.Teams,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = "PS:> Add-PnPTeamsTab -Team \"My Team\" -Channel \"My Channel\" -DisplayName \"My Channel\" -Type WebSite -ContentUrl \"https://aka.ms/sppnp",
       Remarks = "Adds a web site tab to the specified channel.",
       SortOrder = 1)]
    [CmdletAdditionalParameter(ParameterType = typeof(string), ParameterName = "TeamsAppId", Mandatory = true, HelpMessage = @"Specifies the title of the new site collection")]
    [CmdletAdditionalParameter(ParameterType = typeof(string), ParameterName = "EntityId", Mandatory = true, HelpMessage = @"Specifies the title of the new site collection")]
    [CmdletAdditionalParameter(ParameterType = typeof(string), ParameterName = "ContentUrl", Mandatory = true, HelpMessage = @"Specifies the title of the new site collection")]
    [CmdletAdditionalParameter(ParameterType = typeof(string), ParameterName = "RemoveUrl", Mandatory = true, HelpMessage = @"Specifies the title of the new site collection")]
    [CmdletAdditionalParameter(ParameterType = typeof(string), ParameterName = "WebSiteUrl", Mandatory = true, HelpMessage = @"Specifies the title of the new site collection")]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.Group_ReadWrite_All)]
    public class AddTeamsTab : PnPGraphCmdlet, IDynamicParameters
    {
        [Parameter(Mandatory = true, HelpMessage = "Specify the group id, mailNickname or display name of the team to use.", ValueFromPipeline = true)]
        public TeamsTeamPipeBind Team;

        [Parameter(Mandatory = true, HelpMessage = "Specify the channel id of the team to retrieve.", ValueFromPipeline = true)]
        public TeamsChannelPipeBind Channel;


        [Parameter(Mandatory = true, HelpMessage = "Specify the tab type")]
        public string DisplayName;

        [Parameter(Mandatory = true, HelpMessage = "Specify the tab type")]
        public TeamTabType Type;


        private OfficeFileParameters officeFileParameters;
        private DocumentLibraryParameters documentLibraryParameters;
        private CustomParameters customParameters;
        public object GetDynamicParameters()
        {
            switch (Type)
            {
                case TeamTabType.Word:
                case TeamTabType.Excel:
                case TeamTabType.PowerPoint:
                case TeamTabType.PDF:
                    {
                        officeFileParameters = new OfficeFileParameters();
                        return officeFileParameters;
                    }
                case TeamTabType.DocumentLibrary:
                case TeamTabType.WebSite:
                    {
                        documentLibraryParameters = new DocumentLibraryParameters();
                        return documentLibraryParameters;
                    }
                case TeamTabType.Custom:
                    {
                        customParameters = new CustomParameters();
                        return customParameters;
                    }
            }
            return null;
        }


        protected override void ExecuteCmdlet()
        {
            var groupId = Team.GetGroupId(HttpClient, AccessToken);
            if (groupId != null)
            {
                var channelId = Channel.GetId(HttpClient, AccessToken, groupId);
                if (channelId != null)
                {
                    try
                    {
                        string entityId = null;
                        string contentUrl = null;
                        string removeUrl = null;
                        string webSiteUrl = null;
                        string teamsAppId = null;
                        switch (Type)
                        {
                            case TeamTabType.Word:
                            case TeamTabType.Excel:
                            case TeamTabType.PowerPoint:
                            case TeamTabType.PDF:
                                {
                                    entityId = officeFileParameters.EntityId;
                                    contentUrl = officeFileParameters.ContentUrl;
                                    break;
                                }
                            case TeamTabType.DocumentLibrary:
                            case TeamTabType.WebSite:
                                {
                                    contentUrl = documentLibraryParameters.ContentUrl;
                                    break;
                                }
                            case TeamTabType.Custom:
                                {
                                    entityId = customParameters.EntityId;
                                    contentUrl = customParameters.ContentUrl;
                                    removeUrl = customParameters.RemoveUrl;
                                    webSiteUrl = customParameters.WebSiteUrl;
                                    teamsAppId = customParameters.TeamsAppId;
                                    break;
                                }
                        }
                        TeamsUtility.AddTabAsync(HttpClient, AccessToken, groupId, channelId, DisplayName, Type, teamsAppId, entityId, contentUrl, removeUrl, webSiteUrl).GetAwaiter().GetResult();
                    }
                    catch (GraphException ex)
                    {
                        if (ex.Error != null)
                        {
                            throw new PSInvalidOperationException(ex.Error.Message);
                        }
                        else
                        {
                            throw ex;
                        }
                    }
                }
                else
                {
                    throw new PSArgumentException("Channel not found");
                }
            }
            else
            {
                throw new PSArgumentException("Group not found");
            }

        }

        public class OfficeFileParameters
        {
            [Parameter(Mandatory = true)]
            public string EntityId;

            [Parameter(Mandatory = true)]
            public string ContentUrl;
        }

        public class DocumentLibraryParameters
        {
            [Parameter(Mandatory = true)]
            public string ContentUrl;
        }

        public class CustomParameters
        {
            [Parameter(Mandatory = true)]
            public string TeamsAppId;

            [Parameter(Mandatory = false)]
            public string EntityId;

            [Parameter(Mandatory = false)]
            public string ContentUrl;

            [Parameter(Mandatory = false)]
            public string RemoveUrl;

            [Parameter(Mandatory = false)]
            public string WebSiteUrl;
        }
    }
}
#endif
