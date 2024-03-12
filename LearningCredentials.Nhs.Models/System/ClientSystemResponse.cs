// <copyright file="ClientSystemResponse.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.LearningCredentials.Models.System
{
    /// <summary>
    /// Definitial of the client system response.
    /// </summary>
    public class ClientSystemResponse
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the client system name.
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
