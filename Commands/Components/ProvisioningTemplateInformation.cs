using System;
using SharePointPnP.PowerShell.Commands.Enums;

namespace SharePointPnP.PowerShell.Commands.Components
{
    /// <summary>
    /// Defines a Provisioning Template stored in a Provisioning Repository
    /// </summary>
    public class ProvisioningTemplateInformation
    {
        /// <summary>
        /// Defines the URI of the template file
        /// </summary>
        public string TemplateFileUri { get; set; }

        /// <summary>
        /// Defines the target Scope of the Provisioning Template
        /// </summary>
        public TargetScope Scope { get; set; }

        /// <summary>
        /// Defines the target Platforms of the Provisioning Template
        /// </summary>
        public TargetPlatform Platforms { get; set; }

        /// <summary>
        /// Defines the URL of the source Site from which the Provisioning Template has been generated, if any
        /// </summary>
        public string TemplateSourceUrl { get; set; }

        /// <summary>
        /// Defines the URL of the source Site from which the Provisioning Template has been generated, if any
        /// </summary>
        public string TemplateImageUrl { get; set; }

        /// <summary>
        /// Defines the Display Name of the Provisioning Template
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Defines the Description of the Provisioning Template
        /// </summary>
        public string Description { get; set; }

        public Guid Id { get; set; }
    }
}
