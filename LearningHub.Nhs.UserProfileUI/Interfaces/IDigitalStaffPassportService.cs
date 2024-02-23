// <copyright file="IDigitalStaffPassportService.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserProfileUI.Interfaces
{
    using System.Security.Claims;
    using LearningHub.Nhs.LearningCredentials.Models.Dsp;
    using LearningHub.Nhs.LearningCredentials.Models.Entities.Dsp;
    using LearningHub.Nhs.Models.Common;

    /// <summary>
    /// Defines the <see cref="IDigitalStaffPassportService"/> interface.
    /// </summary>
    public interface IDigitalStaffPassportService
    {
        /// <summary>
        /// The get the authorisation url.
        /// </summary>
        /// <param name="verifiableCredentialId">Verifiable credential id.</param>
        /// <param name="currentUserId">The current user id.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<string> GetAuthUrl(int verifiableCredentialId, int currentUserId);

        /// <summary>
        /// Gets the verifiable credentials by id.
        /// </summary>
        /// <param name="verifiableCredentialId">The verifiable credential id.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<VerifiableCredentialResponse> GetVerifiableCredentialById(int verifiableCredentialId);

        /// <summary>
        /// Gets the verifiable credentials.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<List<VerifiableCredentialResponse>> GetVerifiableCredentials();

        /// <summary>
        /// Gets the verifiable credentials for the user.
        /// </summary>
        /// <param name="userVerifiableCredential">The user verifiable credential id.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<UserVerifiableCredentialResponse> GetUserVerifiableCredentialById(int userVerifiableCredential);

        /// <summary>
        /// Gets the user verifiable credentials bu verifiable credential id.
        /// </summary>
        /// <param name="id">The verifiable credential id.</param>
        /// <returns>A list of user verifiable credentials.</returns>
        Task<List<UserVerifiableCredentialResponse>> GetCurrentUserUserVerifiableCredentials(int id);

        /// <summary>
        /// Gets the verifiable credentials summaryfor the user.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<List<UserVerifiableCredentialResponse>> GetCurrentUserVerifiableCredentialsSummary();

        /////// <summary>
        /////// Gets the verifiable credentials for the user.
        /////// </summary>
        /////// <param name="verifiableCredentialId">The verifiable credential id.</param>
        /////// <returns>The <see cref="Task"/>.</returns>
        ////Task<UserVerifiableCredentialResponse> GetCurrentUserVerifiableCredentialById(int verifiableCredentialId);

        /// <summary>
        /// Gets the verifiable credentials for the user by verifiable credential id.
        /// </summary>
        /// <param name="verifiableCredentialId">The verifiable credential id.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<List<UserVerifiableCredentialResponse>> GetCurrentUserVerifiableCredentialsById(int verifiableCredentialId);

        /// <summary>
        /// Gets the client verifiable credentials for the user by verifiable credential id.
        /// </summary>
        /// <param name="verifiableCredentialId">The verifiable credential id.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<ClientSystemCredentialResult> GetClientSystemCredentialForCurrentUser(int verifiableCredentialId);

        /// <summary>
        /// Creates or updates verifiable credentials for the user.
        /// </summary>
        /// <param name="userVerifiableCredentialRequest">The new/existing user verifiable credential.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<ApiResponse> UpdateUserVerifiableCredential(UserVerifiableCredentialRequest userVerifiableCredentialRequest);

        /// <summary>
        /// Requests a token after creation of the credential.
        /// </summary>
        /// <param name="code">The code returned when after creation of a credential.</param>
        /// <param name="currentUserId">The current user id.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<UserVerifiableCredentialResponse> ProcessTokenResponse(string code, int currentUserId);

        /// <summary>
        /// Requests a token and extracts the claims.
        /// </summary>
        /// <param name="code">The code returned when after creation of a credential.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<IEnumerable<Claim>> GetCredentialDetails(string code);

        /////// <summary>
        /////// Returns a list of claims for the certificate.
        /////// </summary>
        /////// <param name="verifiableCredential">The variable credential.</param>
        /////// <returns>The list of claims.</returns>
        ////Dictionary<string, string> PopulateCertificateClaims(VerifiableCredentialResponse verifiableCredential);

        /// <summary>
        /// The get the authorisation url.
        /// </summary>
        /// <param name="credentialType">The type of credential.</param>
        /// <returns>The redirect url.</returns>
        string GetVerifyCredentialUrl(string credentialType);

        /////// <summary>
        /////// Revoke a credential.
        /////// </summary>
        /////// <param name="userVerifiableCredential">The user verifiable credential.</param>
        /////// <returns>The redirect url.</returns>
        ////Task<bool> RevokeVerifiableCredential(UserVerifiableCredentialResponse userVerifiableCredential);

        /// <summary>
        /// Gets the verifiable credentials for the current user.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<List<UserVerifiableCredentialResponse>> GetCurrentUserVerifiableCredentials();

        /// <summary>
        /// Revoke credentials for current user.
        /// </summary>
        /// <param name="verifiableCredentialId">The verifiable credential id.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task RevokeUserVerifiableCredentials(int verifiableCredentialId);
    }
}
