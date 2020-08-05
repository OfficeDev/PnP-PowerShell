#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System.Management.Automation;

namespace PnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Set, "PnPSiteScript", SupportsShouldProcess = true)]
    [CmdletHelp(@"Updates an existing Site Script on the current tenant.",
        Category = CmdletHelpCategory.TenantAdmin,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = @"PS:> Set-PnPSiteScript -Identity f1d55d9b-b116-4f54-bc00-164a51e7e47f -Title ""My Site Script""",
        Remarks = "Updates an existing Site Script and changes the title.",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> $script = Get-PnPSiteScript -Identity f1d55d9b-b116-4f54-bc00-164a51e7e47f 
PS:> Set-PnPSiteScript -Identity $script -Title ""My Site Script""",
        Remarks = "Updates an existing Site Script and changes the title.",
        SortOrder = 2)]
    public class SetSiteScript : PnPAdminCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The guid or an object representing the site script")]
        public TenantSiteScriptPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "The title of the site script")]
        public string Title;

        [Parameter(Mandatory = false, HelpMessage = "The description of the site script")]
        public string Description;

        [Parameter(Mandatory = false, HelpMessage = "A JSON string containing the site script")]
        public string Content;

        [Parameter(Mandatory = false, HelpMessage = "Specifies the version of the site script")]
        public int Version;

        protected override void ExecuteCmdlet()
        {
            var script = Tenant.GetSiteScript(ClientContext, Identity.Id);
            if (script != null)
            {
                var isDirty = false;

                if (ParameterSpecified(nameof(Title)))
                {
                    script.Title = Title;
                    isDirty = true;
                }
                if (ParameterSpecified(nameof(Description)))
                {
                    script.Description = Description;
                    isDirty = true;
                }
                if (ParameterSpecified(nameof(Content)))
                {
                    script.Content = Content;
                    isDirty = true;
                }
                if (ParameterSpecified(nameof(Version)))
                {
                    script.Version = Version;
                    isDirty = true;
                }
                if (isDirty)
                {
                    Tenant.UpdateSiteScript(script);
                    ClientContext.ExecuteQueryRetry();
                    WriteObject(script);
                }
            }
        }
    }
}
#endif