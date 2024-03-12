// <copyright file="UserVerifiableCredentialRequest.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.LearningCredentials.Models.Dsp
{
    using global::System.ComponentModel;
    using global::System.ComponentModel.DataAnnotations;
    using LearningHub.Nhs.LearningCredentials.Models.Enums.Dsp;

    /// <summary>
    /// Definition of the user verifiable credential response.
    /// </summary>
    public class UserVerifiableCredentialRequest
    {
        /// <summary>
        /// Gets or sets the UserVerifiableCredential id.
        /// </summary>
        public int UserVerifiableCredentialId { get; set; }

        /// <summary>
        /// Gets or sets the verifiable credential id.
        /// </summary>
        public int VerifiableCredentialId { get; set; }

        /// <summary>
        /// Gets or sets the client system id.
        /// </summary>
        public UserVerifiableCredentialStatusEnum Status { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the activity date.
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [DisplayName("Activity Date")]
        public DateTimeOffset ActivityDate { get; set; }

        /// <summary>
        /// Gets or sets the activity date.
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [DisplayName("Expiry Date")]
        public DateTimeOffset ExpiryDate { get; set; }

        /// <summary>
        /// Gets or sets the attainment status.
        /// </summary>
        [DisplayName("Attainment Status")]
        public string AttainmentStatus { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the added to digital passport date.
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy HH:mm}")]
        [DisplayName("Added To Digial Wallet")]
        public DateTimeOffset? AddedToDSPDate { get; set; }

        /// <summary>
        /// Gets or sets the retirned digital staff passport code.
        /// </summary>
        public string DSPCode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the retirned digital staff passport serial number.
        /// </summary>
        [DisplayName("Serial Number")]
        public string SerialNumber { get; set; } = string.Empty;
    }
}
