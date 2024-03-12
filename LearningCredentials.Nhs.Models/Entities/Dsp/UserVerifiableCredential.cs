// <copyright file="UserVerifiableCredential.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.LearningCredentials.Models.Entities.Dsp
{
    using LearningHub.Nhs.LearningCredentials.Models.Entities.User;

    /// <summary>
    /// Definition of user verifiable credential.
    /// </summary>
    public class UserVerifiableCredential : EntityBase
    {
        /// <summary>
        /// Gets or sets the verifiable credential id.
        /// </summary>
        public int VerifiableCredentialId { get; set; }

        /// <summary>
        /// Gets or sets the user verifiable credential status id.
        /// </summary>
        public int UserVerifiableCredentialStatusId { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the activity date.
        /// </summary>
        public DateTimeOffset? ActivityDate { get; set; }

        /// <summary>
        /// Gets or sets the expiry date.
        /// </summary>
        public DateTimeOffset? ExpiryDate { get; set; }

        /// <summary>
        /// Gets or sets the attainment status.
        /// </summary>
        public string AttainmentStatus { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the added to digital staff passport date.
        /// </summary>
        public DateTimeOffset? AddedToDSPDate { get; set; }

        /// <summary>
        /// Gets or sets the digital staff passport return code.
        /// </summary>
        public string DSPCode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the serial number of the user's credential.
        /// </summary>
        public string SerialNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        public virtual UserProfile User { get; set; } = null!;

        /// <summary>
        /// Gets or sets the verifiable credential.
        /// </summary>
        public virtual VerifiableCredential VerifiableCredential { get; set; } = null!;

        /// <summary>
        /// Gets or sets the user verifiable credential status.
        /// </summary>
        public virtual UserVerifiableCredentialStatus UserVerifiableCredentialStatus { get; set; } = null!;
    }
}
