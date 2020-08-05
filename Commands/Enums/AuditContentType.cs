namespace PnP.PowerShell.Commands.Enums
{
    /// <summary>
    /// The types of audit logs available through the Office 365 Management API
    /// </summary>
    /// <remarks>Documented at https://docs.microsoft.com/office/office-365-management-api/office-365-management-activity-api-reference#working-with-the-office-365-management-activity-api</remarks>
    public enum AuditContentType
    {
        AzureActiveDirectory,
        Exchange,
        SharePoint,
        General,
        DLP
    }
}
