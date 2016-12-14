namespace SharePointPnP.PowerShell.Commands.Enums
{
    /// <summary>
    /// Scopes to which a CustomAction can be targeted
    /// </summary>
    public enum CustomActionScope
    {
        /// <summary>
        /// Sites
        /// </summary>
        Web,

        /// <summary>
        /// Site collections
        /// </summary>
        Site,

        /// <summary>
        /// Sites collections and sites
        /// </summary>
        All
    }
}
