namespace PnP.PowerShell.Commands.Enums
{
    /// <summary>
    /// Defines the commonly used audiences in oAuth tokens to connect to APIs
    /// </summary>
    public enum TokenAudience : short
    {
        /// <summary>
        /// Any other API not explicitly defined in this enumerator
        /// </summary>
        Other = 0,

        /// <summary>
        /// SharePoint Online
        /// </summary>
        SharePointOnline = 1,

        /// <summary>
        /// Microsoft Graph
        /// </summary>
        MicrosoftGraph = 2,

        /// <summary>
        /// Office 365 Management API
        /// </summary>
        OfficeManagementApi = 3
    }
}
