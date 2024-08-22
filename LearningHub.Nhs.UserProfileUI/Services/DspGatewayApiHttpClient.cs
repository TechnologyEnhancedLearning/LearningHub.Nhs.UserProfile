// <copyright file="DspGatewayApiHttpClient.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserProfileUI.Services
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using LearningHub.Nhs.Caching;
    using LearningHub.Nhs.UserProfileUI.Configuration;
    using LearningHub.Nhs.UserProfileUI.Interfaces;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// The user api http client.
    /// </summary>
    public class DspGatewayApiHttpClient : IDspGatewayApiHttpClient
    {
        private readonly HttpClient httpClient;
        private readonly WebSettings webSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="DspGatewayApiHttpClient"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The http context accessor.</param>
        /// <param name="webSettings">The web settings.</param>
        /// <param name="httpClient">The http client.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="cacheService">The cache service.</param>
        public DspGatewayApiHttpClient(
            IHttpContextAccessor httpContextAccessor,
            IOptions<WebSettings> webSettings,
            HttpClient httpClient,
            ILogger<UserApiHttpClient> logger,
            ICacheService cacheService)
        {
            this.webSettings = webSettings.Value;
            this.httpClient = httpClient;
            this.Initialise();
        }

        /// <summary>
        /// Gets the http client.
        /// </summary>
        /// <returns>The http client.</returns>
        public HttpClient GetClientAsync()
        {
            return this.httpClient;
        }

        /// <summary>
        /// The initialise.
        /// </summary>
        private void Initialise()
        {
            this.httpClient.BaseAddress = new Uri(this.webSettings.DspSettings.DspGatewayUrl);
            this.httpClient.DefaultRequestHeaders.Accept.Clear();
            this.httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
        }
    }
}