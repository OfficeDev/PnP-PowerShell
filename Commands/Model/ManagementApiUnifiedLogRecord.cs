using System;
using System.Collections;
using System.Collections.Generic;

namespace PnP.PowerShell.Commands.Model
{
    /// <summary>
    /// Contains the data of one recorded event in the Office 365 Management API
    /// </summary>
    public class ManagementApiUnifiedLogRecord
    {
        /// <summary>
        /// Date and time at which the event occurred
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Unique identifier of the event
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Operation that occurred, i.e. FileAccessed, UserLoggedIn
        /// </summary>
        public string Operation { get; set; }

        /// <summary>
        /// The ID of the tenant in which the event occurred
        /// </summary>
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// In case it concerns Exchange, the name of the tenant
        /// </summary>
        public string OrganizationName { get; set; }

        /// <summary>
        /// In case it concerns Exchange, the name and IP address of the Exchange server on which the event occurred
        /// </summary>
        public string OriginatingServer { get; set; }

        /// <summary>
        /// Parameters in key/value pairs
        /// </summary>
        public IEnumerable<ManagementApiUnifiedLogRecordKeyValuePair> Parameters { get; set; }

        /// <summary>
        /// Properties in key/value pairs
        /// </summary>
        public IEnumerable<ManagementApiUnifiedLogRecordKeyValuePair> ExtendedProperties { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int RecordType { get; set; }
        
        /// <summary>
        /// The user causing the event in claims format
        /// </summary>
        public string UserKey { get; set; }

        public int UserType { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int Version { get; set; }
        
        /// <summary>
        /// The specific (sub)product where the event occurred, i.e. OneDrive
        /// </summary>
        public string Workload { get; set; }

        /// <summary>
        /// The IP address from which the event was triggered
        /// </summary>
        public string ClientIP { get; set; }

        /// <summary>
        /// The IP address from which the event was triggered
        /// </summary>
        public string ActorIpAddress { get; set; }

        /// <summary>
        /// In case it concerns Active Directory, the result of the operation
        /// </summary>
        public string ResultStatus { get; set; }

        /// <summary>
        /// The path to the item involved in the event, i.e. a HTTPS reference to a specific file
        /// </summary>
        public string ObjectId { get; set; }

        /// <summary>
        /// The e-mail address of the user triggering the event
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// The correlation Id of matching the operation that caused the event
        /// </summary>
        public Guid CorrelationId { get; set; }

        /// <summary>
        /// The product where the event occurred, i.e. SharePoint
        /// </summary>
        public string EventSource { get; set; }

        /// <summary>
        /// The type of item on which the event occurred, i.e. File
        /// </summary>
        public string ItemType { get; set; }

        /// <summary>
        /// The user agent identification of which tool caused the request which caused the event, i.e. a browser identification, OneDrive Sync client
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// Additional data about the event that occurred
        /// </summary>
        public string EventData { get; set; }

        /// <summary>
        /// In case it concerns SharePoint, the unique Id of the site on which the event occurred
        /// </summary>
        public Guid? WebId { get; set; }

        /// <summary>
        /// In case it concerns SharePoint, the unique Id of the list in which the event occurred
        /// </summary>
        public Guid? ListId { get; set; }

        /// <summary>
        /// In case it concerns SharePoint, the unique Id of the list item on which the event occurred
        /// </summary>
        public Guid? ListItemId { get; set; }

        /// <summary>
        /// In case it concerns SharePoint, the URL to the site on which the event occurred
        /// </summary>
        public string SiteUrl { get; set; }

        /// <summary>
        /// In case it concerns a file, the extension of the file
        /// </summary>
        public string SourceFileExtension { get; set; }

        /// <summary>
        /// In case it concerns a file, the name of the file
        /// </summary>
        public string SourceFileName { get; set; }

        /// <summary>
        /// In case it concerns SharePoint, the path starting from the root of the site to the list or library in which the event occurred
        /// </summary>
        public string SourceRelativeUrl { get; set; }


    }
}
