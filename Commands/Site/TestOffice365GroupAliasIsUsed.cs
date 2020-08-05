#if !ONPREMISES
using OfficeDevPnP.Core.Sites;
using PnP.PowerShell.CmdletHelpAttributes;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Site
{
    [Cmdlet(VerbsDiagnostic.Test, "PnPOffice365GroupAliasIsUsed")]
    [CmdletHelp("Tests if a given alias is already used used",
        DetailedDescription = "This command allows you to test if a provided alias is used or free, helps decide if it can be used as part of connecting an Office 365 Unified group to an existing classic site collection.",
        SupportedPlatform = CmdletSupportedPlatform.Online,
        Category = CmdletHelpCategory.Sites)]
    [CmdletExample(
        Code = @"PS:> Test-PnPOffice365GroupAliasIsUsed -Alias ""MyGroup""",
        Remarks = @"This will test if the alias MyGroup is already used", SortOrder = 1)]
    public class AddOffice365GroupAliasIsUsed : PnPSharePointCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Specifies the alias of the group. Cannot contain spaces.")]
        public string Alias;

        protected override void ExecuteCmdlet()
        {
            var results = SiteCollection.AliasExistsAsync(ClientContext, Alias);
            var returnedBool = results.GetAwaiter().GetResult();
            WriteObject(returnedBool);
        }
    }
}
#endif