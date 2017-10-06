#if !ONPREMISES
using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core;
using OfficeDevPnP.Core.Entities;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using Resources = SharePointPnP.PowerShell.Commands.Properties.Resources;
using System.Threading.Tasks;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using SharePointPnP.PowerShell.Commands.Enums;

namespace SharePointPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.New, "PnPSite")]
    [CmdletHelp("BETA: This cmdlet is using early release APIs. Notice that functionality and parameters can change. Creates a new site collection",
        OutputType = typeof(string),
        OutputTypeDescription = "Returns the url of the newly created site collection",
        DetailedDescription = @"The New-PnPSite cmdlet creates a new site collection for the current tenant. Currently only 'modern' sites like Communication Site and the Modern Team Site are supported. If you want to create a classic site, use New-PnPTenantSite.",
        Category = CmdletHelpCategory.TenantAdmin, SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = @"PS:> New-PnPSite -Type CommunicationSite -Title Contoso -Url https://tenant.sharepoint.com/sites/contoso",
        Remarks = @"This will create a new Communications Site collection with the title 'Contoso' and the url 'https://tenant.sharepoint.com/sites/contoso'",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> New-PnPSite -Type CommunicationSite -Title Contoso -Url https://tenant.sharepoint.com/sites/contoso -SiteDesign Showcase",
        Remarks = @"This will create a new Communications Site collection with the title 'Contoso' and the url 'https://tenant.sharepoint.com/sites/contoso'. It will use the 'Showcase' design for the site.",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> New-PnPSite -Type CommunicationSite -Title Contoso -Url https://tenant.sharepoint.com/sites/contoso -SiteDesignId ae2349d5-97d6-4440-94d1-6516b72449ac",
        Remarks = @"This will create a new Communications Site collection with the title 'Contoso' and the url 'https://tenant.sharepoint.com/sites/contoso'. It will use the specified custom site design for the site.",
        SortOrder = 3)]
    [CmdletExample(
        Code = @"PS:> New-PnPSite -Type CommunicationSite -Title Contoso -Url https://tenant.sharepoint.com/sites/contoso -Classification ""HBI""",
        Remarks = @"This will create a new Communications Site collection with the title 'Contoso' and the url 'https://tenant.sharepoint.com/sites/contoso'. The classification for the site will be set to ""HBI""",
        SortOrder = 4)]
    [CmdletExample(
        Code = @"PS:> New-PnPSite -Type CommunicationSite -Title Contoso -Url https://tenant.sharepoint.com/sites/contoso -AllowFileSharingForGuestUsers",
        Remarks = @"This will create a new Communications Site collection with the title 'Contoso' and the url 'https://tenant.sharepoint.com/sites/contoso'. File sharing for guest users will be enabled.",
        SortOrder = 5)]
    [CmdletExample(
        Code = @"PS:> New-PnPSite -Type TeamSite -Title Contoso -Alias contoso",
        Remarks = @"This will create a new Modern Team Site collection with the title 'Contoso' and the url 'https://tenant.sharepoint.com/sites/contoso'.",
        SortOrder = 6)]
    [CmdletExample(
        Code = @"PS:> New-PnPSite -Type TeamSite -Title Contoso -Alias contoso -IsPublic",
        Remarks = @"This will create a new Modern Team Site collection with the title 'Contoso' and the url 'https://tenant.sharepoint.com/sites/contoso' and sets the site to public.",
        SortOrder = 7)]
    [CmdletAdditionalParameter(ParameterType = typeof(string), ParameterName = "Title", Mandatory = true, HelpMessage = @"Specifies the title of the new site collection", ParameterSetName = "CommunicationBuiltInDesign")]
    [CmdletAdditionalParameter(ParameterType = typeof(string), ParameterName = "Title", Mandatory = true, HelpMessage = @"Specifies the title of the new site collection", ParameterSetName = "CommunicationCustomInDesign")]
    [CmdletAdditionalParameter(ParameterType = typeof(string), ParameterName = "Url", Mandatory = true, HelpMessage = @"Specifies the full url of the new site collection", ParameterSetName = "CommunicationBuiltInDesign")]
    [CmdletAdditionalParameter(ParameterType = typeof(string), ParameterName = "Url", Mandatory = true, HelpMessage = @"Specifies the full url of the new site collection", ParameterSetName = "CommunicationCustomInDesign")]
    [CmdletAdditionalParameter(ParameterType = typeof(string), ParameterName = "Description", Mandatory = false, HelpMessage = @"Specifies the description of the new site collection", ParameterSetName = "CommunicationBuiltInDesign")]
    [CmdletAdditionalParameter(ParameterType = typeof(string), ParameterName = "Description", Mandatory = false, HelpMessage = @"Specifies the description of the new site collection", ParameterSetName = "CommunicationCustomInDesign")]
    [CmdletAdditionalParameter(ParameterType = typeof(string), ParameterName = "Classification", Mandatory = false, HelpMessage = @"Specifies the classification of the new site collection", ParameterSetName = "CommunicationBuiltInDesign")]
    [CmdletAdditionalParameter(ParameterType = typeof(string), ParameterName = "Classification", Mandatory = false, HelpMessage = @"Specifies the classification of the new site collection", ParameterSetName = "CommunicationCustomInDesign")]
    [CmdletAdditionalParameter(ParameterType = typeof(SwitchParameter), ParameterName = "AllowFileSharingForGuestUsers", Mandatory = false, HelpMessage = @"Specifies if guest users can share files in the new site collection", ParameterSetName = "CommunicationBuiltInDesign")]
    [CmdletAdditionalParameter(ParameterType = typeof(SwitchParameter), ParameterName = "AllowFileSharingForGuestUsers", Mandatory = false, HelpMessage = @"Specifies if guest users can share files in the new site collection", ParameterSetName = "CommunicationCustomInDesign")]
    [CmdletAdditionalParameter(ParameterType = typeof(OfficeDevPnP.Core.Sites.CommunicationSiteDesign), ParameterName = "SiteDesign", Mandatory = false, HelpMessage = @"Specifies the site design of the new site collection. Defaults to 'Topic'", ParameterSetName = "CommunicationBuiltInDesign")]
    [CmdletAdditionalParameter(ParameterType = typeof(GuidPipeBind), ParameterName = "SiteDesignId", Mandatory = true, HelpMessage = @"Specifies the site design id to use for the new site collection. If specified will override SiteDesign", ParameterSetName = "CommunicationCustomInDesign")]
    [CmdletAdditionalParameter(ParameterType = typeof(uint), ParameterName = "Lcid", Mandatory = false, HelpMessage = @"Specifies the language of the new site collection. Defaults to the current language of the web connected to.", ParameterSetName = "CommunicationBuiltInDesign")]
    [CmdletAdditionalParameter(ParameterType = typeof(uint), ParameterName = "Lcid", Mandatory = false, HelpMessage = @"Specifies the language of the new site collection. Defaults to the current language of the web connected to.", ParameterSetName = "CommunicationCustomInDesign")]
    [CmdletAdditionalParameter(ParameterType = typeof(string), ParameterName = "Title", Mandatory = true, HelpMessage = @"Specifies the title of the new site collection", ParameterSetName = "TeamSite")]
    [CmdletAdditionalParameter(ParameterType = typeof(string), ParameterName = "Alias", Mandatory = true, HelpMessage = @"Specifies the alias of the new site collection", ParameterSetName = "TeamSite")]
    [CmdletAdditionalParameter(ParameterType = typeof(string), ParameterName = "Description", Mandatory = false, HelpMessage = @"Specifies the description of the new site collection", ParameterSetName = "TeamSite")]
    [CmdletAdditionalParameter(ParameterType = typeof(string), ParameterName = "Classification", Mandatory = false, HelpMessage = @"Specifies the classification of the new site collection", ParameterSetName = "TeamSite")]
    [CmdletAdditionalParameter(ParameterType = typeof(string), ParameterName = "IsPublic", Mandatory = false, HelpMessage = @"Specifies if new site collection is public. Defaults to false.", ParameterSetName = "TeamSite")]
    public class NewSite : PnPCmdlet, IDynamicParameters
    {
        [Parameter(Mandatory = true, HelpMessage = "@Specifies with type of site to create.")]
        public SiteType Type;

        private CommunicationSiteParameters _communicationSiteParameters;
        private TeamSiteParameters _teamSiteParameters;

        public object GetDynamicParameters()
        {
            switch (Type)
            {

                case SiteType.CommunicationSite:
                    {
                        _communicationSiteParameters = new CommunicationSiteParameters();
                        return _communicationSiteParameters;
                    }
                case SiteType.TeamSite:
                    {
                        _teamSiteParameters = new TeamSiteParameters();
                        return _teamSiteParameters;
                    }
            }
            return null;
        }

        protected override void ExecuteCmdlet()
        {
            if (Type == SiteType.CommunicationSite)
            {
                if (!MyInvocation.BoundParameters.ContainsKey("Lcid"))
                {
                    ClientContext.Web.EnsureProperty(w => w.Language);
                    _communicationSiteParameters.Lcid = ClientContext.Web.Language;
                }
                var creationInformation = new OfficeDevPnP.Core.Sites.CommunicationSiteCollectionCreationInformation();
                creationInformation.Title = _communicationSiteParameters.Title;
                creationInformation.Url = _communicationSiteParameters.Url;
                creationInformation.Description = _communicationSiteParameters.Description;
                creationInformation.Classification = _communicationSiteParameters.Classification;
                creationInformation.AllowFileSharingForGuestUsers = _communicationSiteParameters.AllowFileSharingForGuestUsers;
                creationInformation.Lcid = _communicationSiteParameters.Lcid;
                if (ParameterSetName == "CommunicationCustomInDesign")
                {
                    creationInformation.SiteDesignId = _communicationSiteParameters.SiteDesignId.Id;
                }
                else
                {
                    creationInformation.SiteDesign = _communicationSiteParameters.SiteDesign;
                }
                var results = ClientContext.CreateSiteAsync(creationInformation);
                var returnedContext = results.GetAwaiter().GetResult();
                WriteObject(returnedContext.Url);
            }
            else
            {
                var creationInformation = new OfficeDevPnP.Core.Sites.TeamSiteCollectionCreationInformation();
                creationInformation.DisplayName = _teamSiteParameters.Title;
                creationInformation.Alias = _teamSiteParameters.Alias;
                creationInformation.Classification = _teamSiteParameters.Classification;
                creationInformation.Description = _teamSiteParameters.Description;
                creationInformation.IsPublic = _teamSiteParameters.IsPublic;

                var results = ClientContext.CreateSiteAsync(creationInformation);
                var returnedContext = results.GetAwaiter().GetResult();
                WriteObject(returnedContext.Url);
            }
        }

        public class CommunicationSiteParameters
        {
            [Parameter(Mandatory = true, ParameterSetName = "CommunicationBuiltInDesign")]
            [Parameter(Mandatory = true, ParameterSetName = "CommunicationCustomInDesign")]
            public string Title;

            [Parameter(Mandatory = true, ParameterSetName = "CommunicationBuiltInDesign")]
            [Parameter(Mandatory = true, ParameterSetName = "CommunicationCustomInDesign")]
            public string Url;

            [Parameter(Mandatory = false, ParameterSetName = "CommunicationBuiltInDesign")]
            [Parameter(Mandatory = false, ParameterSetName = "CommunicationCustomInDesign")]
            public string Description;

            [Parameter(Mandatory = false, ParameterSetName = "CommunicationBuiltInDesign")]
            [Parameter(Mandatory = false, ParameterSetName = "CommunicationCustomInDesign")]
            public string Classification;

            [Parameter(Mandatory = false, ParameterSetName = "CommunicationBuiltInDesign")]
            [Parameter(Mandatory = false, ParameterSetName = "CommunicationCustomInDesign")]
            public SwitchParameter AllowFileSharingForGuestUsers;

            [Parameter(Mandatory = false, ParameterSetName = "CommunicationBuiltInDesign")]
            public OfficeDevPnP.Core.Sites.CommunicationSiteDesign SiteDesign = OfficeDevPnP.Core.Sites.CommunicationSiteDesign.Topic;

            [Parameter(Mandatory = true, ParameterSetName = "CommunicationCustomInDesign")]
            public GuidPipeBind SiteDesignId;

            [Parameter(Mandatory = false, ParameterSetName = "CommunicationBuiltInDesign")]
            [Parameter(Mandatory = false, ParameterSetName = "CommunicationCustomInDesign")]
            public uint Lcid;
        }

        public class TeamSiteParameters
        {
            [Parameter(Mandatory = true, ParameterSetName = "TeamSite")]
            public string Title;

            [Parameter(Mandatory = true, ParameterSetName = "TeamSite")]
            public string Alias;

            [Parameter(Mandatory = false, ParameterSetName = "TeamSite")]
            public string Description;

            [Parameter(Mandatory = false, ParameterSetName = "TeamSite")]
            public string Classification;

            [Parameter(Mandatory = false, ParameterSetName = "TeamSite")]
            public SwitchParameter IsPublic;
        }
    }
}
#endif