using System;

namespace PnP.PowerShell.Commands.Model
{
    class ManagementApiSubscriptionContent
    {
        public string ContentUri { get; set; }

        public string ContentId { get; set; }

        public string ContentType { get; set; }

        public DateTime ContentCreated { get; set; }

        public DateTime ContentExpiration { get; set; }
    }
}
