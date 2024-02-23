// <copyright file="WebSettings.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserProfileUI.Configuration
{
    /// <summary>
    /// The web settings.
    /// </summary>
    public class WebSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebSettings"/> class.
        /// </summary>
        public WebSettings()
        {
            this.DspSettings = new DspSettings();
            this.SupportUrls = new SupportUrls();
        }

        /// <summary>
        /// Gets or sets the BuildNumber.
        /// </summary>
        public string BuildNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user profile url.
        /// </summary>
        public string UserProfileUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the learning credentials api url.
        /// </summary>
        public string LearningCredentialsApiUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user api url.
        /// </summary>
        public string UserApiUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the authentication service url.
        /// </summary>
        public string AuthenticationServiceUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the learning hub secret.
        /// </summary>
        public string AuthClientSecret { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the client id.
        /// </summary>
        public string AuthClientId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the auth timeout.
        /// </summary>
        public int AuthTimeout { get; set; }

        /// <summary>
        /// Gets or sets the digital staff passport settings.
        /// </summary>
        public DspSettings DspSettings { get; set; }

        /// <summary>
        /// Gets or sets the support url settings.
        /// </summary>
        public SupportUrls SupportUrls { get; set; }
    }
}