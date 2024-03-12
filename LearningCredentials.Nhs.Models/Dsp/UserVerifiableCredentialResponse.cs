// <copyright file="UserVerifiableCredentialResponse.cs" company="HEE.nhs.uk">
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
    public class UserVerifiableCredentialResponse : UserVerifiableCredentialRequest
    {
        /// <summary>
        /// Gets or sets the verifiable credential name.
        /// </summary>
        public string CredentialName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the client system id.
        /// </summary>
        public int ClientSystemId { get; set; }

        /// <summary>
        /// Gets or sets the client system credential id.
        /// </summary>
        public int ClientSystemCredentialId { get; set; }

        /// <summary>
        /// Gets or sets the display status.
        /// </summary>
        [DisplayName("Status")]
        public UserVerifiableCredentialDisplayStatusEnum DisplayStatus { get; set; }

        /// <summary>
        /// Gets or sets the Digital staff passport activity date.
        /// </summary>
        public DateTimeOffset? DSPActivityDate { get; set; }

        /// <summary>
        /// Gets or sets the Digital staff passport expiry date.
        /// </summary>
        public DateTimeOffset? DSPExpiryDate { get; set; }

        /// <summary>
        /// Gets or sets the text decribing the renewal period.
        /// </summary>
        public string RenewalPeriodText { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the origin.
        /// </summary>
        public string Origin { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the provider.
        /// </summary>
        public string Provider { get; set; } = string.Empty;
    }
}
