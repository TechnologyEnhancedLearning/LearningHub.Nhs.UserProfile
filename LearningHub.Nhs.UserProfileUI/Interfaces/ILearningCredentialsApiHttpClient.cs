// <copyright file="ILearningCredentialsApiHttpClient.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserProfileUI.Interfaces
{
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// The LearningCredentialsApiHttpClient interface.
    /// </summary>
    public interface ILearningCredentialsApiHttpClient
    {
        /// <summary>
        /// The get client async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<HttpClient> GetClientAsync();
    }
}