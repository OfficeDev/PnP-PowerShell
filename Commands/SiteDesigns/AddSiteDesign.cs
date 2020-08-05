#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;
using PnP.PowerShell.Commands.Enums;
using System;
using System.Linq;
using System.Management.Automation;

namespace PnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Add, "PnPSiteDesign", SupportsShouldProcess = true)]
    [CmdletHelp(@"Creates a new Site Design on the current tenant.",
        Category = CmdletHelpCategory.TenantAdmin,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = @"PS:> Add-PnPSiteDesign -Title ""My Company Design"" -SiteScriptIds ""e84dcb46-3ab9-4456-a136-66fc6ae3d3c5"",""6def687f-0e08-4f1e-999c-791f3af9a600"" -Description ""My description"" -WebTemplate TeamSite",
        Remarks = "Adds a new Site Design, with the specified title and description. When applied it will run the scripts as referenced by the IDs. Use Get-PnPSiteScript to receive Site Scripts. The WebTemplate parameter specifies that this design applies to Team Sites.",
        SortOrder = 1)]
    public class AddSiteDesign : PnPAdminCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The title of the site design")]
        public string Title;

        [Parameter(Mandatory = true, HelpMessage = "An array of guids of site scripts")]
        public GuidPipeBind[] SiteScriptIds;

        [Parameter(Mandatory = false, HelpMessage = "The description of the site design")]
        public string Description;     

        [Parameter(Mandatory = false, HelpMessage = "Specifies if the site design is a default site design")]
        public SwitchParameter IsDefault;

        [Parameter(Mandatory = false, HelpMessage = "Sets the text for the preview image")]
        public string PreviewImageAltText;

        [Parameter(Mandatory = false, HelpMessage = "Sets the url to the preview image")]
        public string PreviewImageUrl;

        [Parameter(Mandatory = true, HelpMessage = "Specifies the type of site to which this design applies")]
        public SiteWebTemplate WebTemplate;


        protected override void ExecuteCmdlet()
        {
            TenantSiteDesignCreationInfo siteDesignInfo = new TenantSiteDesignCreationInfo
            {
                Title = Title,
                SiteScriptIds = SiteScriptIds.Select(t => t.Id).ToArray(),
                Description = Description,
                IsDefault = IsDefault,
                PreviewImageAltText = PreviewImageAltText,
                PreviewImageUrl = PreviewImageUrl,
                WebTemplate = ((int)WebTemplate).ToString()
            };

            var design = Tenant.CreateSiteDesign(siteDesignInfo);
            ClientContext.Load(design);
            ClientContext.ExecuteQueryRetry();
            WriteObject(design);
        }
    }
}         
#endif