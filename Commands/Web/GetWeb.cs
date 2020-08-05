using Microsoft.SharePoint.Client;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.Linq.Expressions;
using System.Management.Automation;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Extensions;

namespace PnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Get, "PnPWeb")]
    [CmdletHelp("Returns the current web object",
        Category = CmdletHelpCategory.Webs,
        OutputType = typeof(Web),
        OutputTypeLink = "https://docs.microsoft.com/previous-versions/office/sharepoint-server/ee537040(v=office.15)",
        SupportedPlatform = CmdletSupportedPlatform.All)]
    [CmdletExample(
        Code = @"PS:> Get-PnPWeb",
        Remarks = "This will return the current web",
        SortOrder = 1)]
    public class GetWeb : PnPRetrievalsCmdlet<Web>
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true, Position = 0, HelpMessage = "The guid of the web or web object")]
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
