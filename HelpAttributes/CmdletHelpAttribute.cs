using System;

namespace SharePointPnP.PowerShell.CmdletHelpAttributes
{
    [AttributeUsage(AttributeTargets.Class,
                       AllowMultiple = false)]
    public sealed class CmdletHelpAttribute : Attribute
    {
        private readonly string _description;

        [Obsolete("Is not used. Use DetailedDescription instead.")]
        public string Details { get; set; }

        public string DetailedDescription { get; set; }
        public string Copyright { get; set; }
        public string Version { get; set; }

        public Type OutputType { get; set; }
        public string OutputTypeDescription { get; set; }
        public string OutputTypeLink { get; set; }

        public CmdletHelpCategory Category { get; set; }
        public CmdletHelpAttribute(string description)
        {
            _description = description;
        }

        public string Description => _description;
    }

}