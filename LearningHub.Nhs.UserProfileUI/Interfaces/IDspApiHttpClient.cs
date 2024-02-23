// <copyright file="IDspApiHttpClient.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserProfileUI.Interfaces
{
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// The User Api HttpClient interface.
    /// </summary>
    public interface IDspApiHttpClient
    {
        /// <summary>
        /// The get client.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<HttpClient> GetClientAsync();
    }
}
