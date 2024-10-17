// <copyright file="VersionService.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserProfileUI.Services
{
    using LearningHub.Nhs.UserProfileUI.Interfaces;

    /// <summary>
    /// Defines the <see cref="VersionService" />.
    /// </summary>
    public class VersionService
    {
        /// <summary>
        /// The GetVersion.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetVersion()
        {
            return typeof(Program).Assembly.GetName().Version?.ToString() ?? "1.0.0";
        }
    }
}