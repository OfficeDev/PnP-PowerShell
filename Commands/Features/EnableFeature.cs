using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using PnP.PowerShell.Commands.Enums;

namespace PnP.PowerShell.Commands.Features
{
    [Cmdlet(VerbsLifecycle.Enable, "PnPFeature")]
    [CmdletHelp("Enables a feature", Category = CmdletHelpCategory.Features)]
    [CmdletExample(
        Code = "PS:> Enable-PnPFeature -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe", 
        Remarks = @"This will enable the feature with the id ""99a00f6e-fb81-4dc7-8eac-e09c6f9132fe""", 
        SortOrder = 1)]
    [CmdletExample(
        Code = "PS:> Enable-PnPFeature -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe -Force", 
        Remarks = @"This will enable the feature with the id ""99a00f6e-fb81-4dc7-8eac-e09c6f9132fe"" with force.", 
        SortOrder = 2)]
    [CmdletExample(
        Code = "PS:> Enable-PnPFeature -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe -Scope Web",
        Remarks = @"This will enable the feature with the id ""99a00f6e-fb81-4dc7-8eac-e09c6f9132fe"" with the web scope.",  
        SortOrder = 3)]
    public class EnableFeature : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, Position=0, ValueFromPipeline=true, HelpMessage = "The id of the feature to enable.")]
        public GuidPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "Specifies whether to overwrite an existing feature with the same feature identifier. This parameter is ignored if there are no errors.")]
        public SwitchParameter Force;

        [Parameter(Mandatory = false, HelpMessage = "Specify the scope of the feature to activate, either Web or Site. Defaults to Web.")]
        public FeatureScope Scope = FeatureScope.Web;

        [Parameter(Mandatory = false, HelpMessage = "Specify this parameter if the feature you're trying to activate is part of a sandboxed solution.")]
        public SwitchParameter Sandboxed;


        protected override void ExecuteCmdlet()
        {
            var featureId = Identity.Id;
            if(Scope == FeatureScope.Web)
            {
                SelectedWeb.ActivateFeature(featureId, Sandboxed);
            }
            else
            {
                ClientContext.Site.ActivateFeature(featureId, Sandboxed);
            }
        }

    }
}
