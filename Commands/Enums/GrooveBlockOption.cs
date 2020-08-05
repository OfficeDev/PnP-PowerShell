namespace PnP.PowerShell.Commands.Enums
{
    /// <summary>
    /// Specifies the options for allowing using the old Groove sync client, used in i.e. <see cref="SetTenantSyncClientRestriction"/>.
    /// </summary>
    public enum GrooveBlockOption : short
    {
        /// <summary>
        /// Allows the usage of the old Groove sync client
        /// </summary>
        OptOut,

        /// <summary>
        /// Groove sync client will be blocked and users are asked to upgrade
        /// </summary>
        HardOptin,

        /// <summary>
        /// Groove sync client is still allows but users will be asked to upgrade
        /// </summary>
        SoftOptin
    }
}
