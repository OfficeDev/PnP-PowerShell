using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.Events
{
    [Cmdlet(VerbsCommon.Add, "PnPEventReceiver")]
    [CmdletHelp("Adds a new remote event receiver",
                Category = CmdletHelpCategory.EventReceivers,
                OutputType=typeof(EventReceiverDefinition),
                OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.eventreceiverdefinition.aspx")]
    [CmdletExample(Code = @"PS:> Add-PnPEventReceiver -List ""ProjectList"" -Name ""TestEventReceiver"" -Url https://yourserver.azurewebsites.net/eventreceiver.svc -EventReceiverType ItemAdded -Synchronization Asynchronous",
                   Remarks = @"This will add a new remote event receiver that is executed after an item has been added to the ProjectList list", 
                   SortOrder = 1)]
    [CmdletExample(Code = @"PS:> Add-PnPEventReceiver -Name ""TestEventReceiver"" -Url https://yourserver.azurewebsites.net/eventreceiver.svc -EventReceiverType WebAdding -Synchronization Synchronous",
                   Remarks = @"This will add a new remote event receiver that is executed while a new subsite is being created",
                   SortOrder = 2)]
    public class AddEventReceiver : PnPWebCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "The list object or name where the remote event receiver needs to be added. If omitted, the remote event receiver will be added to the web.")]
        public ListPipeBind List;

        [Parameter(Mandatory = true, HelpMessage = "The name of the remote event receiver")]
        public string Name;

        [Parameter(Mandatory = true, HelpMessage = "The URL of the remote event receiver web service")]
        public string Url;

        [Parameter(Mandatory = true, HelpMessage = "The type of the event receiver like ItemAdded, ItemAdding. See https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.eventreceivertype.aspx for the full list of available types.")]
        [Alias("Type")]
        public EventReceiverType EventReceiverType;

        [Parameter(Mandatory = true, HelpMessage = "The synchronization type: Asynchronous or Synchronous")]
        [Alias("Sync")]
        public EventReceiverSynchronization Synchronization;

        [Parameter(Mandatory = false, HelpMessage = "The sequence number where this remote event receiver should be placed")]
        public int SequenceNumber = 1000;

        [Parameter(Mandatory = false, HelpMessage = "Overwrites the output file if it exists.")]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            if (MyInvocation.BoundParameters.ContainsKey("List"))
            {
                var list = List.GetList(SelectedWeb);
                WriteObject(list.AddRemoteEventReceiver(Name, Url, EventReceiverType, Synchronization, SequenceNumber, Force));
            }
            else
            {
                WriteObject(SelectedWeb.AddRemoteEventReceiver(Name, Url, EventReceiverType, Synchronization, SequenceNumber, Force));
            }
        }
    }
}