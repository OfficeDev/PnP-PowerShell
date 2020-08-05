#if !SP2013 && !SP2016
using System;
using System.Collections.Generic;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using PnP.PowerShell.Commands.Enums;

namespace PnP.PowerShell.Commands.Principals
{
    [Cmdlet(VerbsCommon.Add, "PnPAlert")]
    [CmdletHelp("Adds an alert for a user to a list",
        Category = CmdletHelpCategory.Principals,
        OutputType = typeof(AlertCreationInformation),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.alertcreationinformation.aspx", SupportedPlatform = CmdletSupportedPlatform.SP2019 | CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = @"PS:> Add-PnPAlert -List ""Demo List""",
        Remarks = @"Adds a new alert to the ""Demo List"" for the current user.",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Add-PnPAlert -Title ""Daily summary"" -List ""Demo List"" -Frequency Daily -ChangeType All -Time (Get-Date -Hour 11 -Minute 00 -Second 00)",
        Remarks = @"Adds a daily alert for the current user at the given time to the ""Demo List"". Note: a timezone offset might be applied so please verify on your tenant that the alert indeed got the right time.",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Add-PnPAlert -Title ""Alert for user"" -List ""Demo List"" -User ""i:0#.f|membership|Alice@contoso.onmicrosoft.com""",
        Remarks = @"Adds a new alert for user ""Alice"" to the ""Demo List"". Note: Only site owners and admins are permitted to set alerts for other users.",
        SortOrder = 3)]
    public class AddAlert : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The ID, Title or Url of the list.")]
        public ListPipeBind List;

        [Parameter(Mandatory = false, HelpMessage = "Alert title")]
        public string Title = "Alert";

        [Parameter(Mandatory = false, HelpMessage = "User to create the alert for (User ID, login name or actual User object). Skip this parameter to create an alert for the current user. Note: Only site owners can create alerts for other users.")]
        public UserPipeBind User;

        [Parameter(Mandatory = false, HelpMessage = "Alert delivery method")]
        public AlertDeliveryChannel DeliveryMethod = AlertDeliveryChannel.Email;

        [Parameter(Mandatory = false, HelpMessage = "Alert change type")]
        public AlertEventType ChangeType = AlertEventType.All;

        [Parameter(Mandatory = false, HelpMessage = "Alert frequency")]
        public AlertFrequency Frequency = AlertFrequency.Immediate;

        [Parameter(Mandatory = false, HelpMessage = @"Alert filter")]
        public AlertFilter Filter = AlertFilter.AnythingChanges;

        [Parameter(Mandatory = false, HelpMessage = "Alert time (if frequency is not immediate)")]
        public DateTime Time = DateTime.MinValue;

        protected override void ExecuteCmdlet()
        {
            List list = null;
            if (List != null)
            {
                list = List.GetList(SelectedWeb);
            }
            if (list != null)
            {
                var alert = new AlertCreationInformation();

                User user;
                if (null != User)
                {
                    user = User.GetUser(ClientContext);
                    if (user == null)
                    {
                        throw new ArgumentException("Unable to find user", "Identity");
                    }
                }
                else
                {
                    user = SelectedWeb.CurrentUser;
                }

                alert.AlertFrequency = Frequency;
                alert.AlertType = AlertType.List;
                alert.AlwaysNotify = false;
                alert.DeliveryChannels = DeliveryMethod;
                var filterValue = Convert.ChangeType(Filter, Filter.GetTypeCode()).ToString();
                alert.Filter = filterValue;

                // setting the value of Filter sometimes does not work (CSOM < Jan 2017, ...?), so we use a known workaround
                // reference: http://toddbaginski.com/blog/how-to-create-office-365-sharepoint-alerts-with-the-client-side-object-model-csom/
                var properties = new Dictionary<string, string>()
                {
                    { "FilterIndex", filterValue }
                    //Send Me an alert when:
                    // 0 = Anything Changes
                    // 1 = Someone else changes a document
                    // 2 = Someone else changes a document created by me
                    // 3 = Someone else changes a document modified by me
                };
                alert.Properties = properties;

                alert.List = list;
                alert.Status = AlertStatus.On;
                alert.Title = Title;
                alert.User = user;
                alert.EventType = ChangeType;
                if (Time != DateTime.MinValue)
                {
                    alert.AlertTime = Time;
                }

                user.Alerts.Add(alert);
                ClientContext.ExecuteQueryRetry();
                WriteObject(alert);
            }
            else
            {
                throw new ArgumentException("Unable to find list", "List");
            }
        }
    }
}
#endif