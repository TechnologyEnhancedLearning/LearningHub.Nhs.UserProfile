// <copyright file="UserProfileResponse.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.LearningCredentials.Models.User
{
    /// <summary>
    /// Definition of basic user response.
    /// </summary>
    public class UserProfileResponse
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string UserName { get; set; } = string.Empty;
    }
}
