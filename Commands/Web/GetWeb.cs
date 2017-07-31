using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.Management.Automation;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Get, "PnPWeb")]
    [CmdletHelp("Returns a web object",
        Category = CmdletHelpCategory.Webs,
        DetailedDescription = "This allows returning a web object representing either the current context its web or one of the webs located underneath it",
        OutputType = typeof(Web),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.web.aspx")]
    [CmdletExample(Code = @"PS:> Get-PnPWeb",
                Remarks = @"Returns the web of the current context",
                SortOrder = 1)]
    [CmdletExample(Code = @"PS:> Get-PnPWeb -Identity aa66f67e-46c0-4474-8a82-42bf467d07f2",
                Remarks = @"Returns the current web or a subweb with the Id 'aa66f67e-46c0-4474-8a82-42bf467d07f2'",
                SortOrder = 2)]
    [CmdletExample(Code = @"PS:> Get-PnPWeb -Identity ""HR""",
                Remarks = @"Returns the subsite located at /HR",
                SortOrder = 3)]
    public class GetWeb : PnPRetrievalsCmdlet<Web>
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true, Position = 0)]
        public WebPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            Web web = null;
            if (Identity == null)
            {
                WriteVerbose("Listing current web");
                web = ClientContext.Web;
            }
            else
            {
                if (Identity.Id != Guid.Empty)
                {
                    WriteVerbose($"Retrieving web through Id {Identity.Id}");
                    web = ClientContext.Site.OpenWebById(Identity.Id);
                }
                else if (Identity.Web != null)
                {
                    WriteVerbose("Received web instance");
                    web = Identity.Web;
                }
                else if (Identity.Url != null)
                {
                    WriteVerbose($"Retrieving web through Url {Identity.Url}");
                    web = ClientContext.Site.OpenWeb(Identity.Url);
                }
            }

            if(web == null)
            {
                throw new ArgumentException("Unable to define web to retrieve", "Identity");
            }
            ClientContext.Load(web);

            try
            {
                ClientContext.ExecuteQueryRetry();
            }
            catch(ServerException e)
            {
                if (e.ServerErrorTypeName == "System.IO.FileNotFoundException")
                {
                    throw new ArgumentException("A web with the provided Identity does not exist", "Identity", e);
                }
                throw e;
            }

            var webProperties = Utilities.PSObjectConverter.ConvertGenericObject(web, this);
            WriteObject(webProperties);
        }
    }
}
