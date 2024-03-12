// <copyright file="ClientSystem.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.LearningCredentials.Models.Entities.System
{
    using LearningHub.Nhs.LearningCredentials.Models.Entities.Dsp;

    /// <summary>
    /// Defnition of the client system.
    /// </summary>
    public partial class ClientSystem : EntityBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientSystem"/> class.
        /// </summary>
        public ClientSystem()
        {
            this.VerifiableCredentials = new HashSet<VerifiableCredential>();
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the origin.
        /// </summary>
        public string Origin { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the provider.
        /// </summary>
        public string Provider { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the verifiable credentials.
        /// </summary>
        public virtual ICollection<VerifiableCredential> VerifiableCredentials { get; set; }
    }
}
