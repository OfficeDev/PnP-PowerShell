#if !SP2013 && !SP2016
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.ClientSidePages
{
    [Cmdlet(VerbsCommon.Set, "PnPClientSideWebPart")]
    [CmdletHelp("Set Client-Side Web Part properties",
        SupportedPlatform = CmdletSupportedPlatform.Online | CmdletSupportedPlatform.SP2019,
        DetailedDescription = "Sets specific client side web part properties. Notice that the title parameter will only set the -internal- title of web part. The title which is shown in the UI will, if possible, have to be set using the PropertiesJson parameter. Use Get-PnPClientSideComponent to retrieve the instance id and properties of a web part.",
        Category = CmdletHelpCategory.WebParts)]
    [CmdletExample(
        Code = @"PS:> Set-PnPClientSideWebPart -Page Home -Identity a2875399-d6ff-43a0-96da-be6ae5875f82 -PropertiesJson $myproperties",
        Remarks = @"Sets the properties of the client side web part given in the $myproperties variable.", SortOrder = 1)]
    public class SetClientSideWebPart : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The name of the page")]
        public ClientSidePagePipeBind Page;

        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "The identity of the web part. This can be the web part instance id or the title of a web part")]
        public ClientSideWebPartPipeBind Identity;

        [Parameter(Mandatory = false, ValueFromPipeline = true, HelpMessage = "Sets the internal title of the web part. Notice that this will NOT set a visible title.")]
        public string Title;

        [Parameter(Mandatory = false, ValueFromPipeline = true, HelpMessage = "Sets the properties as a JSON string.")]
        public string PropertiesJson;

        protected override void ExecuteCmdlet()
        {
            var clientSidePage = Page.GetPage(ClientContext);

            if (clientSidePage == null)
                throw new Exception($"Page '{Page?.Name}' does not exist");

            var controls = Identity.GetWebPart(clientSidePage);
            if (controls.Any())
            {
                if (controls.Count > 1)
                {
                    throw new Exception("Found multiple webparts with the same name. Please use the InstanceId to retrieve the cmdlet.");
                }
                var webpart = controls.First();
                bool updated = false;

                if (ParameterSpecified(nameof(PropertiesJson)))
                {
                    webpart.PropertiesJson = PropertiesJson;
                    updated = true;
                }
                if (ParameterSpecified(nameof(Title)))
                {
                    webpart.Title = Title;
                    updated = true;
                }

                if (updated)
                {
                    clientSidePage.Save();
                }

            }
            else
            {
                throw new Exception($"Web part does not exist");
            }
        }
    }
}
#endif
