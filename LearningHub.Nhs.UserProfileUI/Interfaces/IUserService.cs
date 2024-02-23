// <copyright file="IUserService.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserProfileUI.Interfaces
{
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Common;
    using LearningHub.Nhs.LearningCredentials.Models.User;
    using LearningHub.Nhs.Models.Validation;
    using LearningHub.Nhs.UserProfileUI.Models;

    /// <summary>
    /// Defines the <see cref="IUserService" />.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// The GetElfhUserByUserIdAsync.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<UserViewModel> GetElfhUserByUserIdAsync(int id);

        /// <summary>
        /// The GetUserByUserIdAsync.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<UserProfileResponse> GetUserByUserIdAsync(int id);

        /// <summary>
        /// The StoreUserHistory.
        /// </summary>
        /// <param name="userHistory">The userHistory<see cref="UserHistoryViewModel"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task StoreUserHistory(UserHistoryViewModel userHistory);

        /// <summary>
        /// The SyncLHUserAsync method.
        /// </summary>
        /// <param name="userId">User Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task SyncUserFromMasterAsync(int userId);

        /// <summary>
        /// Create a new user.
        /// </summary>
        /// <param name="newLcUser">
        /// The newLhUser.
        /// </param>
        /// <returns>
        /// The <see cref="T:Task{LearningHubValidationResult}"/>.
        /// </returns>
        Task<LearningHubValidationResult> CreateUserAsync(UserProfileRequest newLcUser);

        /// <summary>
        /// Update a user.
        /// </summary>
        /// <param name="userUpdateViewModel">The lhUser update model.</param>
        /// <returns>
        /// The <see cref="T:Task{LearningHubValidationResult}"/>.
        /// </returns>
        Task<LearningHubValidationResult> UpdateUserAsync(UserProfileRequest userUpdateViewModel);
    }
}
