// <copyright file="UserProfileService.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserProfileUI.Services
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Common;
    using LearningHub.Nhs.Caching;
    using LearningHub.Nhs.LearningCredentials.Models.User;
    using LearningHub.Nhs.Models.Common;
    using LearningHub.Nhs.Models.Validation;
    using LearningHub.Nhs.UserProfileUI.Interfaces;
    using LearningHub.Nhs.UserProfileUI.Models;
    using Newtonsoft.Json;

    /// <summary>
    /// Defines the <see cref="UserProfileService" />.
    /// </summary>
    public class UserProfileService : IUserService
    {
        private readonly ILearningCredentialsApiHttpClient learningCredentialsApiHttpClient;
        private readonly IUserApiHttpClient userApiHttpClient;
        private ICacheService cacheService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileService"/> class.
        /// </summary>
        /// <param name="learningCredentialsApiHttpClient">The dspApiHttpClient<see cref="ILearningCredentialsApiHttpClient"/>.</param>
        /// <param name="userApiHttpClient">The userHttpClient<see cref="IUserApiHttpClient"/>.</param>
        /// <param name="cacheService">The CacheService <see cref="ICacheService"/>.</param>
        public UserProfileService(ILearningCredentialsApiHttpClient learningCredentialsApiHttpClient, IUserApiHttpClient userApiHttpClient, ICacheService cacheService)
        {
            this.cacheService = cacheService;
            this.userApiHttpClient = userApiHttpClient;
            this.learningCredentialsApiHttpClient = learningCredentialsApiHttpClient;
        }

        /// <inheritdoc/>
        public async Task<UserViewModel> GetElfhUserByUserIdAsync(int id)
        {
            UserViewModel viewmodel = null;

            var client = await this.userApiHttpClient.GetClientAsync();

            var request = $"ElfhUser/GetByUserId/{id}";
            var response = await client.GetAsync(request).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                viewmodel = JsonConvert.DeserializeObject<UserViewModel>(result);
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized
        ||
                     response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new Exception("AccessDenied");
            }

            return viewmodel;
        }

        /// <inheritdoc/>
        public async Task<UserProfileResponse> GetUserByUserIdAsync(int id)
        {
            UserProfileResponse viewmodel = null;

            var client = await this.learningCredentialsApiHttpClient.GetClientAsync();

            var request = $"User/GetBasicByUserId/{id}";
            var response = await client.GetAsync(request).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                viewmodel = JsonConvert.DeserializeObject<UserProfileResponse>(result);
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized
                    ||
                     response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new Exception("AccessDenied");
            }

            return viewmodel;
        }

        /// <summary>
        /// The StoreUserHistory.
        /// </summary>
        /// <param name="userHistory">The userHistory<see cref="UserHistoryViewModel"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task StoreUserHistory(UserHistoryViewModel userHistory)
        {
            var json = JsonConvert.SerializeObject(userHistory);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var client = await this.userApiHttpClient.GetClientAsync();

            var request = "UserHistory";

            var response = await client.PostAsync(request, stringContent).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(result);

                if (!apiResponse.Success)
                {
                    throw new Exception("Failed to store UserHistory: " + JsonConvert.SerializeObject(userHistory));
                }
            }
        }

        /// <inheritdoc/>
        public async Task SyncUserFromMasterAsync(int userId)
        {
            var masterUser = await this.GetElfhUserByUserIdAsync(userId);
            var ucUser = await this.GetUserByUserIdAsync(userId);

            if (ucUser == null)
            {
                var newUpUser = new UserProfileRequest()
                {
                    Id = masterUser.Id,
                    UserName = masterUser.UserName,
                };
                var result = await this.CreateUserAsync(newUpUser);
            }
            else if (masterUser.UserName != ucUser.UserName)
            {
                var newUpUser = new UserProfileRequest()
                {
                    Id = masterUser.Id,
                    UserName = masterUser.UserName,
                };
                await this.UpdateUserAsync(newUpUser);
            }
        }

        /// <inheritdoc/>
        public async Task<LearningHubValidationResult> UpdateUserAsync(UserProfileRequest userUpdateViewModel)
        {
            ApiResponse apiResponse = null;
            var json = JsonConvert.SerializeObject(userUpdateViewModel);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            var client = await this.learningCredentialsApiHttpClient.GetClientAsync();

            var request = $"User/UpdateUser";

            var response = await client.PutAsync(request, stringContent).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                apiResponse = JsonConvert.DeserializeObject<ApiResponse>(result);

                if (!apiResponse.Success)
                {
                    throw new Exception("save failed!");
                }

                return apiResponse.ValidationResult;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized || response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                throw new Exception("AccessDenied");
            }
            else
            {
                throw new Exception("save failed!");
            }
        }

        /// <inheritdoc/>
        public async Task<LearningHubValidationResult> CreateUserAsync(UserProfileRequest newLcUser)
        {
            ApiResponse apiResponse = null;

            var json = JsonConvert.SerializeObject(newLcUser);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            var client = await this.learningCredentialsApiHttpClient.GetClientAsync();

            var request = $"User/CreateUser";
            var response = await client.PostAsync(request, stringContent).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                apiResponse = JsonConvert.DeserializeObject<ApiResponse>(result);

                if (!apiResponse.Success)
                {
                    throw new Exception("save failed!");
                }

                return apiResponse.ValidationResult;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized || response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                throw new Exception("AccessDenied");
            }
            else
            {
                throw new Exception("save failed!");
            }
        }
    }
}
