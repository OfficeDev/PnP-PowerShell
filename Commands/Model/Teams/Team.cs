using System.Collections.Generic;

namespace PnP.PowerShell.Commands.Model.Teams
{
    /// <summary>
    /// Defines Team for automated provisiong/update of Microsoft Teams
    /// </summary>
    public class Team
    {
        #region Public Members
        public string DisplayName { get; set; }

        public string Classification { get; set; }
        /// <summary>
        /// The Fun Settings for the Team
        /// </summary>
        public TeamFunSettings FunSettings { get; set; }

        /// <summary>
        /// The Guest Settings for the Team
        /// </summary>
        public TeamGuestSettings GuestSettings { get; set; }

        /// <summary>
        /// The Members Settings for the Team
        /// </summary>
        public TeamMemberSettings MemberSettings { get; set; }

        /// <summary>
        /// The Messaging Settings for the Team
        /// </summary>
        public TeamMessagingSettings MessagingSettings { get; set; }

        /// <summary>
        /// The Discovery Settings for the Team
        /// </summary>
        public TeamDiscoverySettings DiscoverySettings { get; set; }

        /// <summary>
        /// Defines the Security settings for the Team
        /// </summary>
        public TeamSecurity Security { get; set; }

        ///// <summary>
        ///// Defines the Channels for the Team
        ///// </summary>
        //public List<TeamChannel> Channels { get; private set; } = new List<TeamChannel>();

        /// <summary>
        /// Defines the Apps to install or update on the Team
        /// </summary>
        public List<TeamAppInstance> Apps { get; private set; } = new List<TeamAppInstance>();

        public TeamSpecialization? Specialization { get; set; }

        /// <summary>
        /// Declares the ID of the targt Group/Team to update, optional attribute. Cannot be used together with CloneFrom.
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// Declares the ID of another Team to Clone the current Team from
        /// </summary>
        public string CloneFrom { get; set; }

        /// <summary>
        /// Declares whether the Team is archived or not
        /// </summary>
        public bool? IsArchived { get; set; }

        /// <summary>
        /// Declares the nickname for the Team, optional attribute
        /// </summary>
        public string MailNickname { get; set; }

        /// <summary>
        /// Declares the description for the team
        /// </summary>
        public string Description { get; set; }

        public GroupVisibility? Visibility { get; set; }

        #endregion

    }

    public enum GroupVisibility
    {
        NotSpecified,
        Private,
        Public
    }

    /// <summary>
    /// The Specialization for the Team
    /// </summary>
    public enum TeamSpecialization
    {
        /// <summary>
        /// Default type for a team which gives the standard team experience
        /// </summary>
        None,
        /// <summary>
        /// Team created by an education user. All teams created by education user are of type Edu.
        /// </summary>
        EducationStandard,
        /// <summary>
        /// Team experience optimized for a class. This enables segmentation of features across O365.
        /// </summary>
        EducationClass,
        /// <summary>
        /// Team experience optimized for a PLC. Learn more about PLC here.
        /// </summary>
        EducationProfessionalLearningCommunity,
        /// <summary>
        /// Team type for an optimized experience for staff in an organization, where a staff leader, like a principal, is the admin and teachers are members in a team that comes with a specialized notebook.
        /// </summary>
        EducationStaff
    }
}
