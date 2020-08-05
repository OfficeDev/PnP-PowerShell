using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.Taxonomy
{
    [Cmdlet(VerbsCommon.New, "PnPTermLabel", SupportsShouldProcess = false)]
    [CmdletHelp(@"Creates a localized label for a taxonomy term",
        Category = CmdletHelpCategory.Taxonomy,
        OutputType = typeof(Label),
        OutputTypeLink = "https://docs.microsoft.com/en-us/dotnet/api/microsoft.sharepoint.taxonomy.label",
        DetailedDescription = "Creates a localized label for a taxonomy term. Use Get-PnPTerm -Include Labels to request the current labels on a taxonomy term.",
        SupportedPlatform = CmdletSupportedPlatform.All)]
    [CmdletExample
        (Code = @"PS:> New-PnPTermLabel -Name ""Finanzwesen"" -Lcid 1031 -Term (Get-PnPTerm -Identity ""Finance"" -TermSet ""Departments"" -TermGroup ""Corporate"")",
        Remarks = @"Creates a new localized taxonomy label in German (LCID 1031) named ""Finanzwesen"" for the term ""Finance"" in the termset Departments which is located in the ""Corporate"" termgroup",
        SortOrder = 1)]
    [CmdletExample
        (Code = @"PS:> Get-PnPTerm -Identity ""Finance"" -TermSet ""Departments"" -TermGroup ""Corporate"" | New-PnPTermLabel -Name ""Finanzwesen"" -Lcid 1031",
        Remarks = @"Creates a new localized taxonomy label in German (LCID 1031) named ""Finanzwesen"" for the term ""Finance"" in the termset Departments which is located in the ""Corporate"" termgroup",
        SortOrder = 2)]
    public class NewTermLabel : PnPSharePointCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The term to add the localized label to")]
        public TaxonomyItemPipeBind<Term> Term;

        [Parameter(Mandatory = true, HelpMessage = "The localized name of the term")]
        public string Name;

        [Parameter(Mandatory = true, HelpMessage = "The locale id to use for the localized term")]
        public int Lcid;

        [Parameter(Mandatory = false, HelpMessage = "Makes this new label the default label", ParameterSetName = ParameterAttribute.AllParameterSets)]
        public SwitchParameter IsDefault = true;

        protected override void ExecuteCmdlet()
        {
            if (Term.Item == null)
            {
                throw new ArgumentException("You must pass in a Term instance to this command", nameof(Term));
            }

            var label = Term.Item.CreateLabel(Name, Lcid, IsDefault.IsPresent ? IsDefault.ToBool() : true);
            ClientContext.ExecuteQueryRetry();
            WriteObject(label);
        }
    }
}
