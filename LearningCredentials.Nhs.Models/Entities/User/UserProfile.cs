// <copyright file="UserProfile.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.LearningCredentials.Models.Entities.User
{
    using global::System.ComponentModel.DataAnnotations.Schema;
    using LearningHub.Nhs.LearningCredentials.Models.Entities.Dsp;

    /// <summary>
    /// Definition of the user profile.
    /// </summary>
    public class UserProfile : EntityBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfile"/> class.
        /// </summary>
        public UserProfile()
        {
            this.UserVerifiableCredentials = new HashSet<UserVerifiableCredential>();
        }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string UserName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the version start time.
        /// </summary>
        [NotMapped]
        public DateTime VersionStartTime { get; set; }

        /// <summary>
        /// Gets or sets the version end time.
        /// </summary>
        [NotMapped]
        public DateTime VersionEndTime { get; set; }

        /// <summary>
        /// Gets or sets the user verifiable credentials.
        /// </summary>
        public virtual ICollection<UserVerifiableCredential> UserVerifiableCredentials { get; set; }
    }
}
