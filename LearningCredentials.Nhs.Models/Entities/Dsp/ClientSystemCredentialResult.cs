// <copyright file="ClientSystemCredentialResult.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.LearningCredentials.Models.Entities.Dsp
{
    using global::System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the result object of the ELFH stored procedure [dsp].[proc_LatestCompletedActivityByComponents].
    /// </summary>
    public class ClientSystemCredentialResult
    {
        /// <summary>
        /// Gets or sets the component id.
        /// </summary>
        [Key]
        public int ComponentId { get; set; }

        /// <summary>
        /// Gets or sets the user component id.
        /// </summary>
        public int UserComponentId { get; set; }

        /// <summary>
        /// Gets or sets the component type id.
        /// </summary>
        public int ComponentTypeId { get; set; }

        /// <summary>
        /// Gets or sets the component type id.
        /// </summary>
        public string ComponentName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the attainment status.
        /// </summary>
        public string AttainmentStatus { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the activist date.
        /// </summary>
        public DateTimeOffset ActivityDate { get; set; }
    }
}
