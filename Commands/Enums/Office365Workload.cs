namespace PnP.PowerShell.Commands.Enums
{
    /// <summary>
    /// The types of workloads available within Office 365
    /// </summary>
    /// <remarks>Documented at https://docs.microsoft.com/en-us/office/office-365-management-api/office-365-service-communications-api-reference </remarks>
    public enum Office365Workload : int
    {
        Bookings,
        Exchange,
        Forms,
        kaizalamessagingservices,
        Lync,
        MicrosoftFlow,
        MicrosoftFlowM365,
        microsoftteams,
        MobileDeviceManagement,
        O365Client,
        officeonline,
        OneDriveForBusiness,
        OrgLiveID,
        OSDPPlatform,
        OSub,
        Planner,
        PowerAppsM365,
        PowerBIcom,
        SharePoint,
        SwayEnterprise
    }
}
