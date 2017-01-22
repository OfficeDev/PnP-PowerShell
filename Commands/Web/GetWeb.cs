using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.Linq.Expressions;
using System.Management.Automation;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using SharePointPnP.PowerShell.Commands.Extensions;

namespace SharePointPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Get, "PnPWeb")]
    [CmdletAlias("Get-SPOWeb")]
    [CmdletHelp("Returns the current web object",
        Category = CmdletHelpCategory.Webs,
        OutputType = typeof(Web),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.web.aspx")]
    public class GetWeb : PnPRetrievalsCmdlet<Web>
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true, Position = 0)]
        public WebPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            DefaultRetrievalExpressions = new Expression<Func<Web, object>>[] { w => w.Id, w => w.Url, w => w.Title, w => w.ServerRelativeUrl };
            if (Identity == null)
            {
                ClientContext.Web.EnsureProperties(RetrievalExpressions);
                WriteObject(ClientContext.Web);
            }
            else
            {
                if (Identity.Id != Guid.Empty)
                {
                    WriteObject(ClientContext.Web.GetWebById(Identity.Id, RetrievalExpressions));
                }
                else if (Identity.Web != null)
                {
                    WriteObject(ClientContext.Web.GetWebById(Identity.Web.Id, RetrievalExpressions));
                }
                else if (Identity.Url != null)
                {
                    WriteObject(ClientContext.Web.GetWebByUrl(Identity.Url, RetrievalExpressions));
                }
            }
        }

    }
}
