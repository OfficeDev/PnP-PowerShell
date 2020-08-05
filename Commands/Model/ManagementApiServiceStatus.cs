using System;
using System.Collections.Generic;

namespace PnP.PowerShell.Commands.Model
{
    /// <summary>
    /// Information regarding the current status of an Office 365 service
    /// </summary>
    public class ManagementApiServiceStatus
    {
        public string Id { get; set; }

        public string Status { get; set; }

        public string StatusDisplayName { get; set; }

        public DateTime? StatusTime { get; set; }

        public string Workload { get; set; }

        public string WorkloadDisplayName { get; set; }

        public IEnumerable<string> IncidentIds { get; set; }

        public IEnumerable<ManagementApiFeatureStatus> FeatureStatus { get; set; }
    }
}
