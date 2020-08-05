using System;

namespace PnP.PowerShell.CmdletHelpAttributes
{
    /// <summary>
    /// Base attribute on a cmdlet class in order to provide the Api permission needed to execute the cmdlet
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public abstract class CmdletApiPermissionBase : Attribute
    {
        /// <summary>
        /// Friendly name for this API used in the generated documentation
        /// </summary>
        public abstract string ApiName { get; }
    }
}
