using System.Runtime.Serialization;

namespace SharePointPnP.PowerShell.CmdletHelpAttributes
{
    public enum CmdletHelpCategory
    {
        [EnumMember(Value = "Tenant Administration")]
        TenantAdmin = 0,
        Apps = 1,
        [EnumMember(Value = "Base Cmdlets")]
        Base = 2,
        Branding = 3,
        [EnumMember(Value = "Content Types")]
        ContentTypes = 4,
        [EnumMember(Value = "Document Sets")]
        DocumentSets = 5,
        [EnumMember(Value = "Event Receivers")]
        EventReceivers = 6,
        Features = 7,
        Fields = 8,
        [EnumMember(Value = "Information Management")]
        InformationManagement = 9,
        Lists = 10,
        [EnumMember(Value = "User and group management")]
        Principals = 11,
        Publishing = 12,
        Search = 13,
        Sites = 14,
        Taxonomy = 15,
        [EnumMember(Value = "User Profiles")]
        UserProfiles = 16,
        Utilities = 17,
        Webs = 18,
        [EnumMember(Value = "Web Parts")]
        WebParts = 19,
        Workflows = 20,
        Provisioning = 21,
        [EnumMember(Value = "Files and Folders")]
        Files = 22,
        [EnumMember(Value = "Microsoft Graph")]
        Graph = 23,
        [EnumMember(Value = "SharePoint Recycle Bin")]
        RecycleBin = 24,
        [EnumMember(Value = "SharePoint WebHooks")]
        Webhooks = 25,
        [EnumMember(Value = "Records Management")]
        RecordsManagement = 26
    }
}
