// <copyright file="DspGetTokenResponse.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserProfileUI.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// Defines the <see cref="DspGetTokenResponse"/> class.
    /// </summary>
    public class DspGetTokenResponse
    {
        /// <summary>
        /// Gets or sets the request uri.
        /// </summary>
        [JsonProperty("id_token")]
        public string IdToken { get; set; } = string.Empty;
    }
}
