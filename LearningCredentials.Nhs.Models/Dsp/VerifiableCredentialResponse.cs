// <copyright file="VerifiableCredentialResponse.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.LearningCredentials.Models.Dsp
{
    using LearningHub.Nhs.LearningCredentials.Models.Enums.Dsp;

    /// <summary>
    /// Definition of the verifiable credention response.
    /// </summary>
    public class VerifiableCredentialResponse
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the verifiable credential name.
        /// </summary>
        public string CredentialName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the scope name.
        /// </summary>
        public string ScopeName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Claim prefix.
        /// </summary>
        public string ClaimPrefix { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the client system credential id.
        /// </summary>
        public int ClientSystemCredentialId { get; set; }

        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        public short Level { get; set; }

        /// <summary>
        /// Gets or sets the period unit.
        /// </summary>
        public PeriodUnitEnum PeriodUnit { get; set; }

        /// <summary>
        /// Gets or sets the period quantity.
        /// </summary>
        public int PeriodQty { get; set; }
    }
}
