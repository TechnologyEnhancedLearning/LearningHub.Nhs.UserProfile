// <copyright file="DspCacheProgressModel.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserProfileUI.Models
{
    using LearningHub.Nhs.LearningCredentials.Models.Dsp;

    /// <summary>
    /// Defines the <see cref="DspCacheProgressModel"/> class.
    /// </summary>
    public class DspCacheProgressModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspCacheProgressModel"/> class.
        /// </summary>
        public DspCacheProgressModel()
        {
            this.UserVerifiableCredentialRequest = new UserVerifiableCredentialRequest();
        }

        /// <summary>
        /// Gets or sets the nonce value.
        /// </summary>
        public string Nonce { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user verifiable credential request.
        /// </summary>
        public UserVerifiableCredentialRequest UserVerifiableCredentialRequest { get; set; }
    }
}
