using System;

namespace SharePointPnP.PowerShell.CmdletHelpAttributes
{
    [AttributeUsage(AttributeTargets.Class,
                      AllowMultiple = true)]
    public sealed class CmdletExampleAttribute : Attribute
    {
       
        public string Code { get; set; }
        public string Introduction { get; set; }
        public string Remarks { get; set; }
        public int SortOrder { get; set; }
    }
}
