using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.WorkflowServices;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands.Workflows
{
    [Cmdlet(VerbsCommon.Get, "PnPWorkflowDefinition")]
    [CmdletAlias("Get-SPOWorkflowDefinition")]
    [CmdletHelp("Returns a workflow definition",
        Category = CmdletHelpCategory.Workflows,
        OutputType = typeof(WorkflowDefinition),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.workflowservices.workflowdefinition.aspx")]

    public class GetWorkflowDefinition : PnPWebCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "The name of the workflow", Position = 0)]
        public string Name;

        [Parameter(Mandatory = false)]
        public SwitchParameter PublishedOnly = true;

        protected override void ExecuteCmdlet()
        {
            if (string.IsNullOrEmpty(Name))
            {
                var servicesManager = new WorkflowServicesManager(ClientContext, SelectedWeb);
                var deploymentService = servicesManager.GetWorkflowDeploymentService();
                var definitions = deploymentService.EnumerateDefinitions(PublishedOnly);

                ClientContext.Load(definitions);

                ClientContext.ExecuteQueryRetry();
                WriteObject(definitions, true);
            }
            else
            {
                WriteObject(SelectedWeb.GetWorkflowDefinition(Name, PublishedOnly));
            }
        }
    }

}
