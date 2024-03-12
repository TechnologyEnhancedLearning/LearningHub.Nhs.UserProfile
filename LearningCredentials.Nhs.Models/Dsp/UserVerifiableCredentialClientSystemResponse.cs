// <copyright file="UserVerifiableCredentialClientSystemResponse.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.LearningCredentials.Models.Dsp
{
    using global::System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Definition of the user verifiable credential response.
    /// </summary>
    public class UserVerifiableCredentialClientSystemResponse
    {
        /// <summary>
        /// Gets or sets the client system id.
        /// </summary>
        public int ClientSystemId { get; set; }

        /// <summary>
        /// Gets or sets the client system credential id.
        /// </summary>
        public int ClientSystemCredentialId { get; set; }

        /// <summary>
        /// Gets or sets the activity date.
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTimeOffset? ActivityDate { get; set; }
    }
}
