// <copyright file="LearningCredentialsApiHttpClient.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserProfileUI.Services
{
    using System.Net.Http;
    using LearningHub.Nhs.Caching;
    using LearningHub.Nhs.UserProfileUI.Configuration;
    using LearningHub.Nhs.UserProfileUI.Interfaces;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// The learning credentials http client.
    /// </summary>
    public class LearningCredentialsApiHttpClient : BaseHttpClient, ILearningCredentialsApiHttpClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LearningCredentialsApiHttpClient"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The http context accessor.</param>
        /// <param name="webSettings">The web settings.</param>
        /// <param name="client">The http client.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="cacheService">The cache service.</param>
        public LearningCredentialsApiHttpClient(
            IHttpContextAccessor httpContextAccessor,
            IOptions<WebSettings> webSettings,
            HttpClient client,
            ILogger<LearningCredentialsApiHttpClient> logger,
            ICacheService cacheService)
            : base(httpContextAccessor, webSettings.Value, client, logger, cacheService)
        {
        }

        /// <summary>
        /// Gets the learning credentials api url.
        /// </summary>
        public override string ApiUrl => this.WebSettings.LearningCredentialsApiUrl;
    }
}