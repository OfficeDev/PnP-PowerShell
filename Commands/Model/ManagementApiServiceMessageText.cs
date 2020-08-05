using System;

namespace PnP.PowerShell.Commands.Model
{
    /// <summary>
    /// Information regarding the text inside a message concerning a service in Office 365
    /// </summary>
    public class ManagementApiServiceMessageText
    {
        public string MessageText { get; set; }

        public DateTime? PublishedTime { get; set; }
    }
}
