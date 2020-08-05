using System;
using System.Runtime.Serialization;

namespace PnP.PowerShell.CmdletHelpAttributes
{
    /// <summary>
    /// Defines the supported API permissions for the Office Management Api configurable in Azure Active Directory
    /// </summary>
    [Flags]
    public enum OfficeManagementApiPermission : int
    {
        None = 0,

        [EnumMember(Value = "ActivityFeed.Read")]
        ActivityFeed_Read = 1,

        // Documentation: https://docs.microsoft.com/en-us/office/office-365-management-api/office-365-service-communications-api-reference#the-fundamentals
        [EnumMember(Value = "ServiceHealth.Read")]
        ServiceHealth_Read = 2
    }

    /// <summary>
    /// Add this attribute on a cmdlet class in order to provide the Api permission needed to execute the cmdlet
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class CmdletOfficeManagementApiPermissionAttribute : CmdletApiPermissionBase
    {
        /// <summary>
        /// Friendly name for this API used in the generated documentation
        /// </summary>
        public override string ApiName => "Microsoft Office 365 Management API";

        /// <summary>
        /// One or more permissions of which only one is needed to granted to the token
        /// </summary>
        public OfficeManagementApiPermission OrApiPermissions { get; set; }
        public OfficeManagementApiPermission AndApiPermissions { get; set; }

        /// <summary>
        /// Constructs a new ApiPermissionAttribute
        /// </summary>
        /// <param name="apiPermission">One or more possible permissions of which only one is needed to be granted in the token</param>
        public CmdletOfficeManagementApiPermissionAttribute(OfficeManagementApiPermission orApiPermissions, OfficeManagementApiPermission andApiPermissions = OfficeManagementApiPermission.None)
        {
            OrApiPermissions = orApiPermissions;
            AndApiPermissions = andApiPermissions;
        }
    }
}
