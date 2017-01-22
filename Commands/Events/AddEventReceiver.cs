using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.Events
{
    [Cmdlet(VerbsCommon.Add, "PnPEventReceiver")]
    [CmdletAlias("Add-SPOEventReceiver")]
    [CmdletHelp("Adds a new event receiver",
        Category = CmdletHelpCategory.EventReceivers,
        OutputType=typeof(EventReceiverDefinition),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.eventreceiverdefinition.aspx")]
    [CmdletExample(
      Code = @"PS:> Add-PnPEventReceiver -List ""ProjectList"" -Name ""TestEventReceiver"" -Url https://yourserver.azurewebsites.net/eventreceiver.svc -EventReceiverType ItemAdded -Synchronization Asynchronous",
      Remarks = @"This will add a new event receiver that is executed after an item has been added to the ProjectList list", SortOrder = 1)]
    public class AddEventReceiver : PnPWebCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "The list object or name where the event receiver needs to be added")]
        public ListPipeBind List;

        [Parameter(Mandatory = true, HelpMessage = "The name of the event receiver")]
        public string Name;

        [Parameter(Mandatory = true, HelpMessage = "The URL of the event receiver web service")]
        public string Url;

        [Parameter(Mandatory = true, HelpMessage = "The type of the event receiver like ItemAdded, ItemAdding")]
        [Alias("Type")]
        public EventReceiverType EventReceiverType;

        [Parameter(Mandatory = true, HelpMessage = "The Synchronization type, Asynchronous or Synchronous")]
        [Alias("Sync")]
        public EventReceiverSynchronization Synchronization;

        [Parameter(Mandatory = false, HelpMessage = "The sequence number where this event receiver should be placed")]
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


