#if !SP2013 && !SP2016
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Entities;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Enums;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.Branding
{
    [Cmdlet(VerbsCommon.Add, "PnPApplicationCustomizer")]
    [CmdletHelp("Adds a SharePoint Framework client side extension application customizer",
        "Adds a SharePoint Framework client side extension application customizer by registering a user custom action to a web or sitecollection",
        Category = CmdletHelpCategory.Apps,
        SupportedPlatform = CmdletSupportedPlatform.Online | CmdletSupportedPlatform.SP2019)]
    [CmdletExample(Code = @"Add-PnPApplicationCustomizer -Title ""CollabFooter"" -ClientSideComponentId c0ab3b94-8609-40cf-861e-2a1759170b43 -ClientSideComponentProperties ""{`""sourceTermSet`"":`""PnP-CollabFooter-SharedLinks`"",`""personalItemsStorageProperty`"":`""PnP-CollabFooter-MyLinks`""}",
    Remarks = @"Adds a new application customizer to the current web. This requires that a SharePoint Framework solution has been deployed containing the application customizer specified in its manifest. Be sure to run Install-PnPApp before trying this cmdlet on a site.",
    SortOrder = 1)]
    public class AddApplicationCustomizer : PnPWebCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "The title of the application customizer")]
        public string Title = string.Empty;

        [Parameter(Mandatory = false, HelpMessage = "The description of the application customizer")]
        public string Description = string.Empty;

        [Parameter(Mandatory = false, HelpMessage = "Sequence of this application customizer being injected. Use when you have a specific sequence with which to have multiple application customizers being added to the page.")]
        public int Sequence = 0;

        [Parameter(Mandatory = false, HelpMessage = "The scope of the CustomAction to add to. Either Web or Site; defaults to Web. 'All' is not valid for this command.")]
        public CustomActionScope Scope = CustomActionScope.Web;

        [Parameter(Mandatory = true, HelpMessage = "The Client Side Component Id of the SharePoint Framework client side extension application customizer found in the manifest")]
        public GuidPipeBind ClientSideComponentId;

        [Parameter(Mandatory = false, HelpMessage = "The Client Side Component Properties of the application customizer. Specify values as a json string : \"{Property1 : 'Value1', Property2: 'Value2'}\"")]
        public string ClientSideComponentProperties;

#if !ONPREMISES
        [Parameter(Mandatory = false, HelpMessage = "The Client Side Host Properties of the application customizer. Specify values as a json string : \"{'preAllocatedApplicationCustomizerTopHeight': '50', 'preAllocatedApplicationCustomizerBottomHeight': '50'}\"")]
        public string ClientSideHostProperties;
#endif

        protected override void ExecuteCmdlet()
        {
            CustomActionEntity ca = new CustomActionEntity
            {
                Title = Title,
                Location = "ClientSideExtension.ApplicationCustomizer",
                ClientSideComponentId = ClientSideComponentId.Id,
                ClientSideComponentProperties = ClientSideComponentProperties,
#if !ONPREMISES
                ClientSideHostProperties = ClientSideHostProperties
#endif
            };

            switch (Scope)
            {
                case CustomActionScope.Web:
                    SelectedWeb.AddCustomAction(ca);
                    break;

                case CustomActionScope.Site:
                    ClientContext.Site.AddCustomAction(ca);
                    break;

                case CustomActionScope.All:
                    WriteWarning("CustomActionScope 'All' is not supported for adding CustomActions");
                    break;
            }
        }
    }
}
#endif