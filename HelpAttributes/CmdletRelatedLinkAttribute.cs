using System;

namespace PnP.PowerShell.CmdletHelpAttributes
{
    /// <summary>
    /// Specify this attribute on a cmdlet class in order to provider additional 'read more' links on the cmdlet help.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class,
                     AllowMultiple = true)]
    public class CmdletRelatedLinkAttribute : Attribute
    {
        public string Text { get; set; }
        public string Url { get; set; }
    }
}
