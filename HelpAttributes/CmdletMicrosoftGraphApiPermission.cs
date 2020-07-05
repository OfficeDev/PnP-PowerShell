using System;
using System.Runtime.Serialization;

namespace SharePointPnP.PowerShell.CmdletHelpAttributes
{
    /// <summary>
    /// Defines the supported API permissions for the Microsoft Graph Api configurable in Azure Active Directory
    /// </summary>
    [Flags]
    public enum MicrosoftGraphApiPermission : int
    {
        None = 0,

        #region Groups - https://docs.microsoft.com/graph/permissions-reference#group-permissions

        /// <summary>
        /// Read all Groups
        /// </summary>
        [EnumMember(Value = "Group.Read.All")]
        Group_Read_All = 1,

        /// <summary>
        /// Read and write all groups
        /// </summary>
        [EnumMember(Value = "Group.ReadWrite.All")]
        Group_ReadWrite_All = 2,

        /// <summary>
        /// Read group memberships
        /// </summary>
        [EnumMember(Value = "GroupMember.Read.All")]
        GroupMember_Read_All = 4,

        /// <summary>
        /// Read and write group memberships
        /// </summary>
        [EnumMember(Value = "GroupMember.ReadWrite.All")]
        GroupMember_ReadWrite_All = 8,

        /// <summary>
        /// Create groups
        /// </summary>
        [EnumMember(Value = "Group.Create")]
        Group_Create = 16,

        #endregion

        #region Directory - https://docs.microsoft.com/graph/permissions-reference#directory-permissions

        /// <summary>
        /// Read directory data
        /// </summary>
        [EnumMember(Value = "Directory.Read.All")]
        Directory_Read_All = 32,

        /// <summary>
        /// Read and write directory data
        /// </summary>
        [EnumMember(Value = "Directory.ReadWrite.All")]
        Directory_ReadWrite_All = 64,

        #endregion

        #region User - https://docs.microsoft.com/en-us/graph/permissions-reference#user-permissions

        /// <summary>
        /// Read all users' full profiles
        /// </summary>
        [EnumMember(Value = "User.Read.All")]
        User_Read_All = 128,

        /// <summary>
        /// Read and write all users' full profiles
        /// </summary>
        [EnumMember(Value = "User.ReadWrite.All")]
        User_ReadWrite_All = 256,

        /// <summary>
        /// Invite guest users to the organization
        /// </summary>
        [EnumMember(Value = "User.Invite.All")]
        User_Invite_All = 512,

        /// <summary>
        /// Export users' data
        /// </summary>
        [EnumMember(Value = "User.Export.All")]
        User_Export_All = 1024,

        /// <summary>
        /// Manage all user identities
        /// </summary>
        [EnumMember(Value = "User.ManageIdentities.All")]
        User_ManageIdentities_All = 2048,


        #endregion

        #region AppCatalog - https://docs.microsoft.com/en-gb/graph/permissions-reference#appcatalog-resource-permissions

        [EnumMember(Value = "AppCatalog.Read.All")]
        AppCatalog_Read_All = 4096,

        [EnumMember(Value = "AppCatalog.ReadWrite.All")]
        AppCatalog_ReadWrite_All = 8192,

        #endregion
    }

    /// <summary>
    /// Add this attribute on a cmdlet class in order to provide the Api permission needed to execute the cmdlet
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class CmdletMicrosoftGraphApiPermission : CmdletApiPermissionBase
    {
        /// <summary>
        /// Friendly name for this API used in the generated documentation
        /// </summary>
        public override string ApiName => "Microsoft Graph API";

        /// <summary>
        /// One or more permissions of which only one is needed to granted to the token
        /// </summary>
        public MicrosoftGraphApiPermission OrApiPermissions { get; set; }

        public MicrosoftGraphApiPermission AndApiPermissions { get; set; }

        /// <summary>
        /// Constructs a new ApiPermissionAttribute
        /// </summary>
        /// <param name="apiPermission">One or more possible permissions of which only one is needed to be granted in the token</param>
        public CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission orPermissions, MicrosoftGraphApiPermission andPermissions = MicrosoftGraphApiPermission.None)
        {
            OrApiPermissions = orPermissions;
            AndApiPermissions = andPermissions;
        }
    }
}
