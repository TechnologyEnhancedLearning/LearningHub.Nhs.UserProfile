// <copyright file="DspSettings.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserProfileUI.Configuration
{
    /// <summary>
    /// The digital staff passport settings.
    /// </summary>
    public class DspSettings
    {
        /// <summary>
        /// Gets or sets the DspGatewayUrl.
        /// </summary>
        public string DspGatewayUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the authorisation requrest url.
        /// </summary>
        public string AuthorisationRequestUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the authorisation redirect url.
        /// </summary>
        public string AuthorisationRedirectUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the HEE Organisation Identifier.
        /// </summary>
        public string HEEOrgIdentifier { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the client id.
        /// </summary>
        public string ClientId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the client secret.
        /// </summary>
        public string ClientSecret { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the signing key.
        /// </summary>
        public string SigningKey { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the issuing url.
        /// </summary>
        public string IssuingUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the token request url.
        /// </summary>
        public string TokenRequestUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets verify credential url.
        /// </summary>
        public string VerifyCredentialUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the revoke credential url.
        /// </summary>
        public string RevokeCredentialUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the http expiry time in minutes.
        /// </summary>
        public int HttpExpiryMins { get; set; }
    }
}
