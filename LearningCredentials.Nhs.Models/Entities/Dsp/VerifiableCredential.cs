// <copyright file="VerifiableCredential.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.LearningCredentials.Models.Entities.Dsp
{
    using LearningHub.Nhs.LearningCredentials.Models.Entities.System;
    using LearningHub.Nhs.LearningCredentials.Models.Enums.Dsp;

    /// <summary>
    /// Definition of verffiable credential.
    /// </summary>
    public class VerifiableCredential : EntityBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VerifiableCredential"/> class.
        /// </summary>
        public VerifiableCredential()
        {
            this.UserVerifiableCredentials = new HashSet<UserVerifiableCredential>();
        }

        /// <summary>
        /// Gets or sets the credential name.
        /// </summary>
        public string CredentialName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the client system id.
        /// </summary>
        public int ClientSystemId { get; set; }

        /// <summary>
        /// Gets or sets the client system credential id.
        /// </summary>
        public int ClientSystemCredentialId { get; set; }

        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        public short Level { get; set; }

        /// <summary>
        /// Gets or sets the scope name.
        /// </summary>
        public string ScopeName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Claim prefix.
        /// </summary>
        public string ClaimPrefix { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the period unit.
        /// </summary>
        public short PeriodUnitId { get; set; }

        /// <summary>
        /// Gets or sets the period quantity.
        /// </summary>
        public short PeriodQty { get; set; }

        /// <summary>
        /// Gets or sets the client system.
        /// </summary>
        public virtual ClientSystem ClientSystem { get; set; } = null!;

        /// <summary>
        /// Gets or sets the user verifiable credentials.
        /// </summary>
        public virtual ICollection<UserVerifiableCredential> UserVerifiableCredentials { get; set; }
    }
}
