using System;
using System.Collections.Generic;

namespace PnP.PowerShell.Commands.Model
{
    /// <summary>
    /// Information regarding a message concerning a service in Office 365
    /// </summary>
    public class ManagementApiServiceMessage
    {
        public string Id { get; set; }

        public string ActionType { get; set; }

        public string ActionDetails { get; set; }

        public int? AffectedTenantCount { get; set; }

        public int? AffectedUserCount { get; set; }

        public IEnumerable<string> AffectedWorkloadDisplayNames { get; set; }

        public IEnumerable<string> AffectedWorkloadNames { get; set; }

        public int AnnouncementId { get; set; }

        public string AppliesTo { get; set; }

        public string BlogLink { get; set; }

        public string Category { get; set; }

        public string Classification { get; set; }

        public DateTime? EndTime { get; set; }

        public string ExternalLink { get; set; }

        public string FeatureDisplayName { get; set; }

        public string Feature { get; set; }

        public string FlightName { get; set; }

        public string HelpLink { get; set; }

        public string ImpactDescription { get; set; }

        public bool? IsDismissed { get; set; }

        public bool? IsMajorChange { get; set; }

        public bool? IsRead { get; set; }

        public DateTime? LastUpdatedTime { get; set; }
        
        public IEnumerable<ManagementApiServiceMessageText> Messages { get; set; }

        public IEnumerable<string> MessageTagNames { get; set; }

        public string MessageType { get; set; }

        public string Milestone { get; set; }

        public DateTime? MilestoneDate { get; set; }

        public string PostIncidentDocumentUrl { get; set; }

        public string PreviewDuration { get; set; }

        public string Severity { get; set; }

        public DateTime? StartTime { get; set; }

        public string Status { get; set; }

        public string Title { get; set; }

        public string UserFunctionalImpact { get; set; }

        public string Workload { get; set; }

        public string WorkloadDisplayName { get; set; }
    }
}
