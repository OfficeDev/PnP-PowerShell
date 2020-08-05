using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.Events
{
    [Cmdlet(VerbsCommon.Get, "PnPEventReceiver")]
    [CmdletHelp("Return registered eventreceivers",
        "Returns all registered or a specific eventreceiver",
        Category = CmdletHelpCategory.EventReceivers,
        OutputType = typeof(EventReceiverDefinition),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.eventreceiverdefinition.aspx")]
    [CmdletExample(
      Code = @"PS:> Get-PnPEventReceiver",
      Remarks = @"This will return all registered event receivers on the current web", SortOrder = 1)]
    [CmdletExample(
      Code = @"PS:> Get-PnPEventReceiver -Identity fb689d0e-eb99-4f13-beb3-86692fd39f22",
      Remarks = @"This will return the event receiver with the provided ReceiverId ""fb689d0e-eb99-4f13-beb3-86692fd39f22"" from the current web", SortOrder = 2)]
    [CmdletExample(
      Code = @"PS:> Get-PnPEventReceiver -Identity MyReceiver",
      Remarks = @"This will return the event receiver with the provided ReceiverName ""MyReceiver"" from the current web", SortOrder = 3)]
    [CmdletExample(
      Code = @"PS:> Get-PnPEventReceiver -List ""ProjectList""",
      Remarks = @"This will return all registered event receivers in the provided ""ProjectList"" list", SortOrder = 4)]
    [CmdletExample(
      Code = @"PS:> Get-PnPEventReceiver -List ""ProjectList"" -Identity fb689d0e-eb99-4f13-beb3-86692fd39f22",
      Remarks = @"This will return the event receiver in the provided ""ProjectList"" list with with the provided ReceiverId ""fb689d0e-eb99-4f13-beb3-86692fd39f22""", SortOrder = 5)]
    [CmdletExample(
      Code = @"PS:> Get-PnPEventReceiver -List ""ProjectList"" -Identity MyReceiver",
      Remarks = @"This will return the event receiver in the ""ProjectList"" list with the provided ReceiverName ""MyReceiver""", SortOrder = 5)]
    public class GetEventReceiver : PnPWebRetrievalsCmdlet<EventReceiverDefinition>
    {
        [Parameter(Mandatory = false, ParameterSetName = "List", HelpMessage = "The list object from which to get the event receiver object")]
        public ListPipeBind List;

        [Parameter(Mandatory = false, ValueFromPipeline = true, HelpMessage = "The Guid of the event receiver")]
        public EventReceiverPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            if (ParameterSetName == "List")
            {
                var list = List.GetList(SelectedWeb);

                if (list != null)
                {
                    if (!ParameterSpecified(nameof(Identity)))
                    {
                        var query = ClientContext.LoadQuery(list.EventReceivers);
                        ClientContext.ExecuteQueryRetry();
                        WriteObject(query, true);
                    }
                    else
                    {
                        WriteObject(Identity.GetEventReceiverOnList(list));
                    }
                }
            }
            else
            {
                if (!ParameterSpecified(nameof(Identity)))
                {
                    var query = ClientContext.LoadQuery(SelectedWeb.EventReceivers);
                    ClientContext.ExecuteQueryRetry();
                    WriteObject(query, true);
                }
                else
                {
                    WriteObject(Identity.GetEventReceiverOnWeb(SelectedWeb));
                }
            }
        }
    }
}