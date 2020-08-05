using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System.Collections.Generic;

namespace PnP.PowerShell.Commands.Events
{
    [Cmdlet(VerbsCommon.Remove, "PnPEventReceiver", SupportsShouldProcess = true)]
    [CmdletHelp("Remove an eventreceiver",
        "Removes/unregisters a specific eventreceiver",
                Category = CmdletHelpCategory.EventReceivers)]
    [CmdletExample(Code = @"PS:> Remove-PnPEventReceiver -Identity fb689d0e-eb99-4f13-beb3-86692fd39f22",
                   Remarks = @"This will remove the event receiver with ReceiverId ""fb689d0e-eb99-4f13-beb3-86692fd39f22"" from the current web", 
                   SortOrder = 1)]
    [CmdletExample(Code = @"PS:> Remove-PnPEventReceiver -List ProjectList -Identity fb689d0e-eb99-4f13-beb3-86692fd39f22",
                   Remarks = @"This will remove the event receiver with ReceiverId ""fb689d0e-eb99-4f13-beb3-86692fd39f22"" from the ""ProjectList"" list", 
                   SortOrder = 2)]
    [CmdletExample(Code = @"PS:> Remove-PnPEventReceiver -List ProjectList -Identity MyReceiver",
                   Remarks = @"This will remove the event receiver with ReceiverName ""MyReceiver"" from the ""ProjectList"" list",
                   SortOrder = 3)]
    [CmdletExample(Code = @"PS:> Remove-PnPEventReceiver -List ProjectList",
                   Remarks = @"This will remove all event receivers from the ""ProjectList"" list",
                   SortOrder = 4)]
    [CmdletExample(Code = @"PS:> Remove-PnPEventReceiver",
                   Remarks = @"This will remove all event receivers from the current site",
                   SortOrder = 5)]
    [CmdletExample(Code = @"PS:> Get-PnPEventReceiver | ? ReceiverUrl -Like ""*azurewebsites.net*"" | Remove-PnPEventReceiver",
                   Remarks = @"This will remove all event receivers from the current site which are pointing to a service hosted on Azure Websites",
                   SortOrder = 6)]
    public class RemoveEventReceiver : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "The Guid of the event receiver on the list")]
        public EventReceiverPipeBind Identity;

        [Parameter(Mandatory = false, ParameterSetName="List", HelpMessage = "The list object from where to remove the event receiver object")]
        public ListPipeBind List;

        [Parameter(Mandatory = false, HelpMessage = "Specifying the Force parameter will skip the confirmation question")]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            // Keep a list with all event receivers to remove for better performance and to avoid the collection changing when removing an item in the collection
            var eventReceiversToDelete = new List<EventReceiverDefinition>();

            if (ParameterSetName == "List")
            {
                var list = List.GetList(SelectedWeb);
                if (list == null)
                    throw new PSArgumentException($"No list found with id, title or url '{List}'", "List");

                if (ParameterSpecified(nameof(Identity)))
                {
                    var eventReceiver = Identity.GetEventReceiverOnList(list);
                    if (eventReceiver != null)
                    {
                        if (Force || (ParameterSpecified("Confirm") && !bool.Parse(MyInvocation.BoundParameters["Confirm"].ToString())) || ShouldContinue(string.Format(Properties.Resources.RemoveEventReceiver, eventReceiver.ReceiverName, eventReceiver.ReceiverId), Properties.Resources.Confirm))
                        {
                            eventReceiversToDelete.Add(eventReceiver);
                        }
                    }
                }
                else
                {
                    var eventReceivers = list.EventReceivers;
                    SelectedWeb.Context.Load(eventReceivers);
                    SelectedWeb.Context.ExecuteQueryRetry();

                    foreach (var eventReceiver in eventReceivers)
                    {
                        if (Force || (ParameterSpecified("Confirm") && !bool.Parse(MyInvocation.BoundParameters["Confirm"].ToString())) || ShouldContinue(string.Format(Properties.Resources.RemoveEventReceiver, eventReceiver.ReceiverName, eventReceiver.ReceiverId), Properties.Resources.Confirm))
                        {
                            eventReceiversToDelete.Add(eventReceiver);
                        }
                    }
                    ClientContext.ExecuteQueryRetry();
                }
            }
            else
            {
                if (ParameterSpecified(nameof(Identity)))
                {
                    var eventReceiver = Identity.GetEventReceiverOnWeb(SelectedWeb);
                    if (eventReceiver != null)
                    {
                        if (Force || (ParameterSpecified("Confirm") && !bool.Parse(MyInvocation.BoundParameters["Confirm"].ToString())) || ShouldContinue(string.Format(Properties.Resources.RemoveEventReceiver, eventReceiver.ReceiverName, eventReceiver.ReceiverId), Properties.Resources.Confirm))
                        {
                            eventReceiversToDelete.Add(eventReceiver);
                        }
                    }
                }
                else
                {
                    var eventReceivers = SelectedWeb.EventReceivers;
                    SelectedWeb.Context.Load(eventReceivers);
                    SelectedWeb.Context.ExecuteQueryRetry();

                    foreach (var eventReceiver in eventReceivers)
                    {
                        if (Force || (ParameterSpecified("Confirm") && !bool.Parse(MyInvocation.BoundParameters["Confirm"].ToString())) || ShouldContinue(string.Format(Properties.Resources.RemoveEventReceiver, eventReceiver.ReceiverName, eventReceiver.ReceiverId), Properties.Resources.Confirm))
                        {
                            eventReceiversToDelete.Add(eventReceiver);
                        }
                    }
                }
            }

            if (eventReceiversToDelete.Count == 0)
            {
                WriteVerbose("No Event Receivers to remove");
                return;
            }

            for(var x = 0; x < eventReceiversToDelete.Count; x++)
            {
                WriteVerbose($"Removing Event Receiver with Id {eventReceiversToDelete[x].ReceiverId} named {eventReceiversToDelete[x].ReceiverName}");
                eventReceiversToDelete[x].DeleteObject();
            }
            SelectedWeb.Context.ExecuteQueryRetry();
        }
    }
}


