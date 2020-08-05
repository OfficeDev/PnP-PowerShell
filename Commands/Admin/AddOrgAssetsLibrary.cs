#if !ONPREMISES
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using System.Management.Automation;
using Microsoft.Online.SharePoint.TenantAdministration;

namespace PnP.PowerShell.Commands.Admin
{
    [Cmdlet(VerbsCommon.Add, "PnPOrgAssetsLibrary")]
    [CmdletHelp("Adds a given document library as a organizational asset source",
     DetailedDescription = @"Adds a given document library as an organizational asset source in your Sharepoint Online Tenant. All organizational asset sources you add must reside in the same site collection. Document libraries specified as organizational asset must be enabled as an Office 365 CDN source, either as private or public. It may take some time before this change will be reflected in the webinterface.",
     SupportedPlatform = CmdletSupportedPlatform.Online,
     Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(
     Code = @"PS:> Add-PnPOrgAssetsLibrary -LibraryUrl https://yourtenant.sharepoint.com/sites/branding/logos",
     Remarks = @"Adds the document library with the url ""logos"" located in the sitecollection at ""https://yourtenant.sharepoint.com/sites/branding"" as an organizational asset not specifying a thumbnail image for it and enabling the document library as a public Office 365 CDN source", SortOrder = 1)]
    [CmdletExample(
     Code = @"PS:> Add-PnPOrgAssetsLibrary -LibraryUrl https://yourtenant.sharepoint.com/sites/branding/logos -ThumbnailUrl https://yourtenant.sharepoint.com/sites/branding/logos/thumbnail.jpg",
     Remarks = @"Adds the document library with the url ""logos"" located in the sitecollection at ""https://yourtenant.sharepoint.com/sites/branding"" as an organizational asset specifying the thumbnail image ""thumbnail.jpg"" residing in the same document library for it and enabling the document library as a public Office 365 CDN source", SortOrder = 2)]
    [CmdletExample(
     Code = @"PS:> Add-PnPOrgAssetsLibrary -LibraryUrl https://yourtenant.sharepoint.com/sites/branding/logos -CdnType Private",
     Remarks = @"Adds the document library with the url ""logos"" located in the sitecollection at ""https://yourtenant.sharepoint.com/sites/branding"" as an organizational asset not specifying a thumbnail image for it and enabling the document library as a private Office 365 CDN source", SortOrder = 3)]
    public class AddOrgAssetsLibrary : PnPAdminCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The full url of the document library to be marked as one of organization's assets sources")]
        public string LibraryUrl;

        [Parameter(Mandatory = false, HelpMessage = "The full url to an image that should be used as a thumbnail for showing this source. The image must reside in the same site as the document library you specify.")]
        public string ThumbnailUrl;

        [Parameter(Mandatory = false, HelpMessage = @"Indicates what type of Office 365 CDN source the document library will be added to")]
        public SPOTenantCdnType CdnType = SPOTenantCdnType.Public;

        protected override void ExecuteCmdlet()
        {
            Tenant.AddToOrgAssetsLibAndCdn(CdnType, LibraryUrl, ThumbnailUrl);
            ClientContext.ExecuteQueryRetry();
        }
    }
}
#endif