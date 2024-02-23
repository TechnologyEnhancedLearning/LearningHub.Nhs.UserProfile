// <copyright file="UserSessionHelper.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserProfileUI.Helper
{
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Common;
    using LearningHub.Nhs.UserProfileUI.Interfaces;

    /// <summary>
    /// Defines the <see cref="UserSessionHelper" />.
    /// </summary>
    public class UserSessionHelper : IUserSessionHelper
    {
        private readonly IUserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserSessionHelper"/> class.
        /// </summary>
        /// <param name="userService">The user service.</param>
        public UserSessionHelper(IUserService userService)
        {
            this.userService = userService;
        }

        /// <inheritdoc/>
        public async Task StartSession(int userId)
        {
            UserHistoryViewModel userHistoryViewModel = new UserHistoryViewModel()
            {
                Detail = "User Account Client user session started",
                UserId = userId,
            };
            await this.userService.StoreUserHistory(userHistoryViewModel);
            await this.userService.SyncUserFromMasterAsync(userId);
        }
    }
}
