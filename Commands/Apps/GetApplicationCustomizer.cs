#if !SP2013 && !SP2016
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using PnP.PowerShell.Commands.Enums;
using System.Collections.Generic;
using System.Linq;

namespace PnP.PowerShell.Commands.Branding
{
    [Cmdlet(VerbsCommon.Get, "PnPApplicationCustomizer", ConfirmImpact = ConfirmImpact.High, SupportsShouldProcess = true)]
    [CmdletHelp("Returns all SharePoint Framework client side extension application customizers",
        "Returns all SharePoint Framework client side extension application customizers registered on the current web and/or site",
        Category = CmdletHelpCategory.Apps,
        SupportedPlatform = CmdletSupportedPlatform.Online | CmdletSupportedPlatform.SP2019)]
    [CmdletExample(Code = @"PS:> Get-PnPApplicationCustomizer",
                   Remarks = @"Returns the custom action representing the SharePoint Framework client side extension registrations registered on the current site collection and web.",
                   SortOrder = 1)]
    [CmdletExample(Code = @"PS:> Get-PnPApplicationCustomizer -Identity aa66f67e-46c0-4474-8a82-42bf467d07f2", 
                   Remarks = @"Returns the custom action representing the SharePoint Framework client side extension registration with the id 'aa66f67e-46c0-4474-8a82-42bf467d07f2'.", 
                   SortOrder = 2)]
    [CmdletExample(Code = @"PS:> Get-PnPApplicationCustomizer -ClientSideComponentId aa66f67e-46c0-4474-8a82-42bf467d07f2 -Scope Web", 
                   Remarks = @"Returns the custom action(s) being registered for a SharePoint Framework solution having the id 'aa66f67e-46c0-4474-8a82-42bf467d07f2' in its manifest from the current web.", 
                   SortOrder = 3)]
    public class GetApplicationCustomizer : PnPWebRetrievalsCmdlet<UserCustomAction>
    {
        private const string ParameterSet_CUSTOMACTIONID = "Custom Action Id";
        private const string ParameterSet_CLIENTSIDECOMPONENTID = "Client Side Component Id";

        [Parameter(Mandatory = false, HelpMessage = "Identity of the SharePoint Framework client side extension application customizer to return. Omit to return all SharePoint Frameworkclient side extension application customizer.", ParameterSetName = ParameterSet_CUSTOMACTIONID)]
        public GuidPipeBind Identity;

        [Parameter(Mandatory = true, HelpMessage = "The Client Side Component Id of the SharePoint Framework client side extension application customizer found in the manifest for which existing custom action(s) should be removed", ParameterSetName = ParameterSet_CLIENTSIDECOMPONENTID)]
        public GuidPipeBind ClientSideComponentId;

        [Parameter(Mandatory = false, HelpMessage = "Scope of the SharePoint Framework client side extension application customizer, either Web, Site or All to return both (all is the default)")]
        public CustomActionScope Scope = CustomActionScope.All;

        [Parameter(Mandatory = false, HelpMessage = "Switch parameter if an exception should be thrown if the requested SharePoint Frameworkclient side extension application customizer does not exist (true) or if omitted, nothing will be returned in case the SharePoint Framework client side extension application customizer does not exist")]
        public SwitchParameter ThrowExceptionIfCustomActionNotFound;

        protected override void ExecuteCmdlet()
        {
            List<UserCustomAction> actions = new List<UserCustomAction>();

            if (Scope == CustomActionScope.All || Scope == CustomActionScope.Web)
            {
                actions.AddRange(SelectedWeb.GetCustomActions(RetrievalExpressions));
            }
            if (Scope == CustomActionScope.All || Scope == CustomActionScope.Site)
            {
                actions.AddRange(ClientContext.Site.GetCustomActions(RetrievalExpressions));
            }

            if (Identity != null)
            {
                var foundAction = actions.FirstOrDefault(x => x.Id == Identity.Id && x.Location == "ClientSideExtension.ApplicationCustomizer");
                if (foundAction != null || !ThrowExceptionIfCustomActionNotFound)
                {
                    WriteObject(foundAction, true);
                }
                else
                {
                    throw new PSArgumentException($"No SharePoint Framework client side extension application customizer found with the Identity '{Identity.Id}' within the scope '{Scope}'", "Identity");
                }
            }
            else
            {
                switch (ParameterSetName)
                {
                    case ParameterSet_CLIENTSIDECOMPONENTID:
                        actions = actions.Where(x => x.Location == "ClientSideExtension.ApplicationCustomizer" & x.ClientSideComponentId == ClientSideComponentId.Id).ToList();
                        break;

                    case ParameterSet_CUSTOMACTIONID:
                        actions = actions.Where(x => x.Location == "ClientSideExtension.ApplicationCustomizer").ToList();
                        break;
                }

                WriteObject(actions, true);
            }
        }
    }
}
#endif
