#if !SP2013 && !SP2016

using Microsoft.SharePoint.Client;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.Linq.Expressions;
using System.Management.Automation;
using PnP.PowerShell.CmdletHelpAttributes;

namespace PnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Get, "PnPAvailableLanguage")]
    [CmdletHelp("Returns the available languages on the current web",
        Category = CmdletHelpCategory.Webs,
        SupportedPlatform = CmdletSupportedPlatform.Online | CmdletSupportedPlatform.SP2019)]
    [CmdletExample(
        Code = @"PS:> Get-PnPAvailableLanguage",
        Remarks = "This will return the available languages in the current web",
        SortOrder = 1)]
    public class GetAvailableLanguage : PnPRetrievalsCmdlet<Web>
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true, Position = 0, HelpMessage = "The guid of the web or web object")]
        public WebPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            DefaultRetrievalExpressions = new Expression<Func<Web, object>>[] { w => w.RegionalSettings.InstalledLanguages };
            if (Identity == null)
            {
                ClientContext.Web.EnsureProperties(RetrievalExpressions);
                WriteObject(ClientContext.Web.RegionalSettings.InstalledLanguages, true);
            }
            else
            {
                if (Identity.Id != Guid.Empty)
                {
                    WriteObject(ClientContext.Web.RegionalSettings.InstalledLanguages, true);
                }
                else if (Identity.Web != null)
                {
                    WriteObject(ClientContext.Web.RegionalSettings.InstalledLanguages, true);
                }
                else if (Identity.Url != null)
                {
                    WriteObject(ClientContext.Web.RegionalSettings.InstalledLanguages, true);
                }
            }
        }
    }
}
#endif