#if !ONPREMISES
using OfficeDevPnP.Core.Pages;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.ClientSidePages
{
    [Cmdlet(VerbsCommon.Add, "PnPClientSideWebPart")]
    [CmdletHelp("Adds a Client-Side Component to a page",
      Category = CmdletHelpCategory.ClientSidePages, SupportedPlatform = CmdletSupportedPlatform.Online)]
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
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The name of the page.", ParameterSetName = "DefaultBuiltIn")]
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The name of the page.", ParameterSetName = "Default3rdParty")]
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The name of the page.", ParameterSetName = "PositionedBuiltIn")]
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The name of the page.", ParameterSetName = "Positioned3rdParty")]
        public ClientSidePagePipeBind Page;

        [Parameter(Mandatory = true, HelpMessage = "Defines a default WebPart type to insert.", ParameterSetName = "DefaultBuiltIn")]
        [Parameter(Mandatory = true, HelpMessage = "Defines a default WebPart type to insert.", ParameterSetName = "PositionedBuiltIn")]
        public DefaultClientSideWebParts DefaultWebPartType;

        [Parameter(Mandatory = true, HelpMessage = "Specifies the component instance or Id to add.", ParameterSetName = "Default3rdParty")]
        [Parameter(Mandatory = true, HelpMessage = "Specifies the component instance or Id to add.", ParameterSetName = "Positioned3rdParty")]
        public ClientSideComponentPipeBind Component;

        [Parameter(Mandatory = false, HelpMessage = @"The properties of the WebPart", ParameterSetName = "DefaultBuiltIn")]
        [Parameter(Mandatory = false, HelpMessage = @"The properties of the WebPart", ParameterSetName = "Default3rdParty")]
        [Parameter(Mandatory = false, HelpMessage = @"The properties of the WebPart", ParameterSetName = "PositionedBuiltIn")]
        [Parameter(Mandatory = false, HelpMessage = @"The properties of the WebPart", ParameterSetName = "Positioned3rdParty")]
        public PropertyBagPipeBind WebPartProperties;

        [Parameter(Mandatory = false, HelpMessage = "Sets the order of the WebPart control. (Default = 1)", ParameterSetName = "DefaultBuiltIn")]
        [Parameter(Mandatory = false, HelpMessage = "Sets the order of the WebPart control. (Default = 1)", ParameterSetName = "Default3rdParty")]
        [Parameter(Mandatory = false, HelpMessage = "Sets the order of the WebPart control. (Default = 1)", ParameterSetName = "PositionedBuiltIn")]
        [Parameter(Mandatory = false, HelpMessage = "Sets the order of the WebPart control. (Default = 1)", ParameterSetName = "Positioned3rdParty")]
        public int Order = 1;

        [Parameter(Mandatory = true, HelpMessage = "Sets the section where to insert the WebPart control.", ParameterSetName = "PositionedBuiltIn")]
        [Parameter(Mandatory = true, HelpMessage = "Sets the section where to insert the WebPart control.", ParameterSetName = "Positioned3rdParty")]
        public int Section;

        [Parameter(Mandatory = true, HelpMessage = "Sets the column where to insert the WebPart control.", ParameterSetName = "PositionedBuiltIn")]
        [Parameter(Mandatory = true, HelpMessage = "Sets the column where to insert the WebPart control.", ParameterSetName = "Positioned3rdParty")]
        public int Column;

        protected override void ExecuteCmdlet()
        {
            if (MyInvocation.BoundParameters.ContainsKey("Section") && Section == 0)
            {
                throw new Exception("Section value should be at least 1 or higher");
            }

            if (MyInvocation.BoundParameters.ContainsKey("Column") && Column == 0)
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
            if (MyInvocation.BoundParameters.ContainsKey("DefaultWebPartType"))
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

            if (MyInvocation.BoundParameters.ContainsKey("Section"))
            {
                if (MyInvocation.BoundParameters.ContainsKey("Column"))
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
        }
    }
}
#endif