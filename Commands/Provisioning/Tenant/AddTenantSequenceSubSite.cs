#if !ONPREMISES
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using PnP.PowerShell.CmdletHelpAttributes;
using System;
using System.Linq;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Provisioning.Tenant
{
    [Cmdlet(VerbsCommon.Add, "PnPTenantSequenceSubSite", SupportsShouldProcess = true)]
    [CmdletHelp("Adds a tenant sequence sub site object to a tenant sequence site object",
        Category = CmdletHelpCategory.Provisioning, SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = @"PS:> Add-PnPTenantSequenceSubSite -Site $mysite -SubSite $mysubsite",
       Remarks = "Adds an existing subsite object to an existing sequence site object",
       SortOrder = 1)]
    public class AddTenantSequenceSubSite : PSCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The subsite to add")]
        public TeamNoGroupSubSite SubSite;

        [Parameter(Mandatory = true, HelpMessage = "The site to add the subsite to", ValueFromPipeline = true)]
        public SiteCollection Site;

        protected override void ProcessRecord()
        {
            if (Site.Sites.Cast<TeamNoGroupSubSite>().FirstOrDefault(s => s.Url == SubSite.Url) == null)
            {
                Site.Sites.Add(SubSite);
            }
            else
            {
                WriteError(new ErrorRecord(new Exception($"Site with URL {SubSite.Url} already exists in sequence"), "DUPLICATEURL", ErrorCategory.InvalidData, SubSite));
            }
        }
    }
}
#endif