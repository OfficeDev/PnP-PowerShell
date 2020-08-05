#if !SP2013 && !SP2016
using OfficeDevPnP.Core.Pages;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.WebParts
{
    [Cmdlet(VerbsCommon.Add, "PnPClientSideWebPart")]
    [CmdletHelp("Adds a Client-Side Web Part to a client-side page",
        "Adds a client-side web part to an existing client-side page.",
      Category = CmdletHelpCategory.ClientSidePages, SupportedPlatform = CmdletSupportedPlatform.Online | CmdletSupportedPlatform.SP2019)]
    [CmdletExample(
        Code = @"PS:> Add-PnPClientSideWebPart -Page ""MyPage"" -DefaultWebPartType BingMap",
        Remarks = "Adds a built-in Client-Side component 'BingMap' to the page called 'MyPage'",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Add-PnPClientSideWebPart -Page ""MyPage"" -Component ""HelloWorld""",
        Remarks = "Adds a Client-Side component 'HelloWorld' to the page called 'MyPage'",
        SortOrder = 3)]
    [CmdletExample(
        Code = @"PS:> Add-PnPClientSideWebPart  -Page ""MyPage"" -Component ""HelloWorld"" -Section 1 -Column 2",
        Remarks = "Adds a Client-Side component 'HelloWorld' to the page called 'MyPage' in section 1 and column 2",
        SortOrder = 4)]
    public class AddClientSideWebPart : PnPWebCmdlet
    {
        private const string ParameterSet_DEFAULTBUILTIN = "Default with built-in web part";
        private const string ParameterSet_DEFAULT3RDPARTY = "Default with 3rd party web part";
        private const string ParameterSet_POSITIONED3RDPARTY = "Positioned with 3rd party web part";
        private const string ParameterSet_POSITIONEDBUILTIN = "Positioned with built-in web part";
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The name of the page.", ParameterSetName = ParameterSet_DEFAULTBUILTIN)]
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The name of the page.", ParameterSetName = ParameterSet_DEFAULT3RDPARTY)]
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The name of the page.", ParameterSetName = ParameterSet_POSITIONEDBUILTIN)]
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The name of the page.", ParameterSetName = ParameterSet_POSITIONED3RDPARTY)]
        public ClientSidePagePipeBind Page;

        [Parameter(Mandatory = true, HelpMessage = "Defines a default web part type to insert.", ParameterSetName = ParameterSet_DEFAULTBUILTIN)]
        [Parameter(Mandatory = true, HelpMessage = "Defines a default web part type to insert.", ParameterSetName = ParameterSet_POSITIONEDBUILTIN)]
        public DefaultClientSideWebParts DefaultWebPartType;

        [Parameter(Mandatory = true, HelpMessage = "Specifies the component instance or Id to add.", ParameterSetName = ParameterSet_DEFAULT3RDPARTY)]
        [Parameter(Mandatory = true, HelpMessage = "Specifies the component instance or Id to add.", ParameterSetName = ParameterSet_POSITIONED3RDPARTY)]
        public ClientSideComponentPipeBind Component;

        [Parameter(Mandatory = false, HelpMessage = @"The properties of the web part", ParameterSetName = ParameterSet_DEFAULTBUILTIN)]
        [Parameter(Mandatory = false, HelpMessage = @"The properties of the web part", ParameterSetName = ParameterSet_DEFAULT3RDPARTY)]
        [Parameter(Mandatory = false, HelpMessage = @"The properties of the web part", ParameterSetName = ParameterSet_POSITIONEDBUILTIN)]
        [Parameter(Mandatory = false, HelpMessage = @"The properties of the web part", ParameterSetName = ParameterSet_POSITIONED3RDPARTY)]
        public PropertyBagPipeBind WebPartProperties;

        [Parameter(Mandatory = false, HelpMessage = "Sets the order of the web part control. (Default = 1)", ParameterSetName = ParameterSet_DEFAULTBUILTIN)]
        [Parameter(Mandatory = false, HelpMessage = "Sets the order of the web part control. (Default = 1)", ParameterSetName = ParameterSet_DEFAULT3RDPARTY)]
        [Parameter(Mandatory = false, HelpMessage = "Sets the order of the web part control. (Default = 1)", ParameterSetName = ParameterSet_POSITIONEDBUILTIN)]
        [Parameter(Mandatory = false, HelpMessage = "Sets the order of the web part control. (Default = 1)", ParameterSetName = ParameterSet_POSITIONED3RDPARTY)]
        public int Order = 1;

        [Parameter(Mandatory = true, HelpMessage = "Sets the section where to insert the web part control.", ParameterSetName = ParameterSet_POSITIONEDBUILTIN)]
        [Parameter(Mandatory = true, HelpMessage = "Sets the section where to insert the web part control.", ParameterSetName = ParameterSet_POSITIONED3RDPARTY)]
        public int Section;

        [Parameter(Mandatory = true, HelpMessage = "Sets the column where to insert the web part control.", ParameterSetName = ParameterSet_POSITIONEDBUILTIN)]
        [Parameter(Mandatory = true, HelpMessage = "Sets the column where to insert the web part control.", ParameterSetName = ParameterSet_POSITIONED3RDPARTY)]
        public int Column;

        protected override void ExecuteCmdlet()
        {
            if (ParameterSpecified(nameof(Section)) && Section == 0)
            {
                throw new Exception("Section value should be at least 1 or higher");
            }

            if (ParameterSpecified(nameof(Column)) && Column == 0)
            {
                throw new Exception("Column value should be at least 1 or higher");
            }

            var clientSidePage = Page.GetPage(ClientContext);
            // If the client side page object cannot be found
            if (clientSidePage == null)
            {
                throw new Exception($"Page {Page} cannot be found.");
            }

            ClientSideWebPart webpart = null;
            if (ParameterSpecified(nameof(DefaultWebPartType)))
            {
                webpart = clientSidePage.InstantiateDefaultWebPart(DefaultWebPartType);
            }
            else
            {
                webpart = new ClientSideWebPart(Component.GetComponent(clientSidePage));
            }

            if (WebPartProperties != null)
            {
                if (WebPartProperties.Properties != null)
                {
                    webpart.Properties.Merge(WebPartProperties.JsonObject);
                }
                else if (!string.IsNullOrEmpty(WebPartProperties.Json))
                {
                    webpart.PropertiesJson = WebPartProperties.Json;
                }
            }

            if (ParameterSpecified(nameof(Section)))
            {
                if (ParameterSpecified(nameof(Column)))
                {
                    clientSidePage.AddControl(webpart,
                                clientSidePage.Sections[Section - 1].Columns[Column - 1], Order);
                }
                else
                {
                    clientSidePage.AddControl(webpart, clientSidePage.Sections[Section - 1], Order);
                }
            }
            else
            {
                clientSidePage.AddControl(webpart, Order);
            }

            clientSidePage.Save();
            WriteObject(webpart);
        }
    }
}
#endif