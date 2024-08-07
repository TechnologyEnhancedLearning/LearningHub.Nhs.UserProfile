// <copyright file="UserVerifiableCredentialandIdentityResponse.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserProfileUI.Models
{
    using LearningHub.Nhs.LearningCredentials.Models.Dsp;

    /// <summary>
    /// Definition of the user verifiable credential response and identity status.
    /// </summary>
    public class UserVerifiableCredentialandIdentityResponse
    {
        /// <summary>
        /// Gets or sets UserVerifiableCredentialResponse.
        /// </summary>
        public List<UserVerifiableCredentialResponse>? UserVerifiableCredentialResponse { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user identity has been verified.
        /// </summary>
        public bool IdentityVerified { get; set; } = false;
    }
}
