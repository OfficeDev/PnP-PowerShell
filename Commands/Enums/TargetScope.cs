namespace SharePointPnP.PowerShell.Commands.Enums
{
    /// <summary>
    /// Defines the available scopes for a Provisioning Template definition
    /// </summary>
    public enum TargetScope
    {
        /// <summary>
        /// Defines a Provisioning Template for a Site Collection
        /// </summary>
        Site,
        /// <summary>
        /// Defines a Provisioning Template for a Sub Site
        /// </summary>
        Web,
        /// <summary>
        /// Defines a Provisioning Template that can be applied as a partial add-on to a site
        /// </summary>
        Partial,
    }
}
