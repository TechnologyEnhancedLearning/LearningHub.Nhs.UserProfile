// <copyright file="LogoutUserManager.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserProfileUI.Handlers
{
    using System.Collections.Generic;
    using System.Linq;
    using LearningHub.Nhs.Caching;

    /// <summary>
    /// Defines the <see cref="LogoutUserManager" />.
    /// </summary>
    public class LogoutUserManager
    {
        private readonly ICacheService cacheService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogoutUserManager"/> class.
        /// </summary>
        /// <param name="cacheService">The cache service.</param>
        public LogoutUserManager(ICacheService cacheService)
        {
            this.cacheService = cacheService;
        }

        /// <summary>
        /// The Add.
        /// </summary>
        /// <param name="sub">The sub<see cref="string"/>.</param>
        /// <param name="sid">The sid<see cref="string"/>.</param>
        public void Add(string sub, string sid)
        {
            this.cacheService.SetAsync($"LoggedOutUserSession:{sub}_{sid}", true);
        }

        /// <summary>
        /// The IsLoggedOut.
        /// </summary>
        /// <param name="sub">The sub<see cref="string"/>.</param>
        /// <param name="sid">The sid<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public async Task<bool> IsLoggedOut(string sub, string sid)
        {
            var (cacheExists, result) = await this.cacheService.TryGetAsync<bool>($"LoggedOutUserSession:{sub}_{sid}");
            return cacheExists && result;
        }

        /// <summary>
        /// Defines the <see cref="User" />.
        /// </summary>
        private class User
        {
            /// <summary>
            /// Gets or sets the Sid.
            /// </summary>
            public string Sid { get; set; }

            /// <summary>
            /// Gets or sets the Sub.
            /// </summary>
            public string Sub { get; set; }

            /// <summary>
            /// The IsMatch.
            /// </summary>
            /// <param name="sub">The sub<see cref="string"/>.</param>
            /// <param name="sid">The sid<see cref="string"/>.</param>
            /// <returns>The <see cref="bool"/>.</returns>
            public bool IsMatch(string sub, string sid)
            {
                return (this.Sid == sid && this.Sub == sub) ||
                       (this.Sid == sid && this.Sub == null) ||
                       (this.Sid == null && this.Sub == sub);
            }
        }
    }
}
