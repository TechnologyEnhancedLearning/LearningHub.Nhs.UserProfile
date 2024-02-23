// <copyright file="IDspGatewayApiHttpClient.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserProfileUI.Interfaces
{
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// The User Api HttpClient interface.
    /// </summary>
    public interface IDspGatewayApiHttpClient
    {
        /// <summary>
        /// The get client.
        /// </summary>
        /// <returns>
        /// The HttpClient.
        /// </returns>
        HttpClient GetClientAsync();
    }
}
