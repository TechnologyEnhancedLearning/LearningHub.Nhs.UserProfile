// <copyright file="UserVerifiableCredentialStatus.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.LearningCredentials.Models.Entities.Dsp
{
    /// <summary>
    /// Definition of user verifiable credential.
    /// </summary>
    public class UserVerifiableCredentialStatus : EntityBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserVerifiableCredentialStatus"/> class.
        /// </summary>
        public UserVerifiableCredentialStatus()
        {
            this.UserVerifiableCredentials = new HashSet<UserVerifiableCredential>();
        }

        /// <summary>
        /// Gets or sets the status name.
        /// </summary>
        public string StatusName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user verifiable credentials.
        /// </summary>
        public virtual ICollection<UserVerifiableCredential> UserVerifiableCredentials { get; set; }
    }
}
