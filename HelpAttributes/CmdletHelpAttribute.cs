using System;

namespace SharePointPnP.PowerShell.CmdletHelpAttributes
{
    /// <summary>
    /// Specify the attribute on a cmdlet class in order specify out in the help files and documentation
    /// </summary>
    [AttributeUsage(AttributeTargets.Class,
                       AllowMultiple = false)]
    public sealed class CmdletHelpAttribute : Attribute
    {
        [Obsolete("Is not used. Use DetailedDescription instead.")]
        public string Details { get; set; }

        /// <summary>
        /// Generic description of the cmdlet
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// If this cmdlet is only functional for a specific version of SharePoint, specify the version here.
        /// </summary>
        public CmdletSupportedPlatform SupportedPlatform { get; set; }

        /// <summary>
        /// The detailed description of the cmdlet
        /// </summary>
        public string DetailedDescription { get; set; }

        public string Copyright { get; set; }
        public string Version { get; set; }

        /// <summary>
        /// The type of the object it returns. If specified this information will be listed in the help
        /// </summary>
        public Type OutputType { get; set; }

        /// <summary>
        /// A description describing what will be returned
        /// </summary>
        public string OutputTypeDescription { get; set; }

        /// <summary>
        /// A link to docs.microsoft.com, MSDN etc. to provide more information about the object which is returned
        /// </summary>
        public string OutputTypeLink { get; set; }

        /// <summary>
        /// The cmdlet category. This is required parameter in order to add the parameter to the correct subcategory of cmdlets.
        /// </summary>
        public CmdletHelpCategory Category { get; set; }
        public CmdletHelpAttribute(string description, CmdletSupportedPlatform supportedPlatform = CmdletSupportedPlatform.All)
        {
            Description = description;
            SupportedPlatform = supportedPlatform;
        }

        public CmdletHelpAttribute(string description, string detailedDescription, CmdletSupportedPlatform supportedPlatform = CmdletSupportedPlatform.All)
        {
            Description = description;
            DetailedDescription = detailedDescription;
            SupportedPlatform = supportedPlatform;
        }

    }

}