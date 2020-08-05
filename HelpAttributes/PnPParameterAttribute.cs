using System;

namespace PnP.PowerShell.CmdletHelpAttributes
{
    /// <summary>
    /// Specify this attribute on a cmdlet parameter to define its ordering in the documentation
    /// </summary>
    [AttributeUsage(AttributeTargets.Field,
                    AllowMultiple = false)]
    public class PnPParameterAttribute : Attribute
    {
        public int Order { get; set; }
    }
}
