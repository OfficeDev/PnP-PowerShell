using System;

namespace SharePointPnP.PowerShell.Commands.Enums
{
    /// <summary>
    /// Defines the target platforms for a Provisioning Template
    /// </summary>
    [Flags]
    public enum TargetPlatform
    {
        /// <summary>
        /// No target platform
        /// </summary>
        None = 0,
        /// <summary>
        /// SharePoint 2013
        /// </summary>
        SharePoint2013 = 1,
        /// <summary>
        /// SharePoint 2016
        /// </summary>
        SharePoint2016 = 2,
        /// <summary>
        /// SharePoint Online
        /// </summary>
        SharePointOnline = 4,
        /// <summary>
        /// SharePoint 2013, SharePoint 2016, SharePoint Online
        /// </summary>
        All = SharePoint2013 | SharePoint2016 | SharePointOnline,
    }
}