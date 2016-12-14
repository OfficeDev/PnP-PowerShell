using System;

namespace SharePointPnP.PowerShell.CmdletHelpAttributes
{
    [AttributeUsage(AttributeTargets.Class,
                     AllowMultiple = true)]
    public class CmdletRelatedLinkAttribute : Attribute
    {
        public string Text { get; set; }
        public string Url { get; set; }
    }
}
