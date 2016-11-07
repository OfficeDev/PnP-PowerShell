using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.Events
{
    [Cmdlet(VerbsCommon.Get, "SPOEventReceiver")]
    [CmdletHelp("Returns all or a specific event receiver",
        Category = CmdletHelpCategory.EventReceivers,
        OutputType = typeof(EventReceiverDefinition),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.eventreceiverdefinition.aspx")]
    [CmdletExample(
      Code = @"PS:> Get-SPOEventReceiver",
      Remarks = @"This will return all registered event receivers on the current web", SortOrder = 1)]
    [CmdletExample(
      Code = @"PS:> Get-SPOEventReceiver -Identity fb689d0e-eb99-4f13-beb3-86692fd39f22",
      Remarks = @"This will return a specific registered event receiver from the current web", SortOrder = 2)]
    [CmdletExample(
      Code = @"PS:> Get-SPOEventReceiver -List ""ProjectList""",
      Remarks = @"This will return all registered event receivers in the list with the name ProjectList", SortOrder = 3)]
    [CmdletExample(
      Code = @"PS:> Get-SPOEventReceiver -List ""ProjectList"" -Identity fb689d0e-eb99-4f13-beb3-86692fd39f22",
      Remarks = @"This will return a specific registered event receiver in the list with the name ProjectList", SortOrder = 4)]
    public class GetEventReceiver : SPOWebCmdlet
    {
        [Parameter(Mandatory = false, ParameterSetName = "List", HelpMessage = "The list object from which to get the event receiver object")]
        public ListPipeBind List;

        [Parameter(Mandatory = false, HelpMessage = "The Guid of the event receiver on the list")]
        public GuidPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            if (ParameterSetName == "List")
            {
                var list = List.GetList(SelectedWeb);

                if (list != null)
                {
                    if (Identity == null)
                    {
                        var query = ClientContext.LoadQuery(list.EventReceivers);
                        ClientContext.ExecuteQueryRetry();
                        WriteObject(query, true);
                    }
                    else
                    {
                        WriteObject(list.GetEventReceiverById(Identity.Id));
                    }
                }
            }
            else
            {
                if (Identity == null)
                {
                    var query = ClientContext.LoadQuery(SelectedWeb.EventReceivers);
                    ClientContext.ExecuteQueryRetry();
                    WriteObject(query, true);
                }
                else
                {
                    WriteObject(SelectedWeb.GetEventReceiverById(Identity.Id));
                }
            }

        }
    }
}


