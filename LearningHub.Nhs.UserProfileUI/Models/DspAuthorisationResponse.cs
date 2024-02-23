// <copyright file="DspAuthorisationResponse.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserProfileUI.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// Defines the <see cref="DspAuthorisationResponse"/> class.
    /// </summary>
    public class DspAuthorisationResponse
    {
        /// <summary>
        /// Gets or sets the request uri.
        /// </summary>
        [JsonProperty("request_uri")]
        public string RequestUri { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the expires in period.
        /// </summary>
        [JsonProperty("expires_in")]
        public string ExpiresIn { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the nonce value.
        /// </summary>
        public string Nonce { get; set; } = string.Empty;
    }
}
