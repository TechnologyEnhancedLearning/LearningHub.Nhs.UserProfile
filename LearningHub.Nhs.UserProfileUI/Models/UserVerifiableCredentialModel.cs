// <copyright file="UserVerifiableCredentialModel.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserProfileUI.Models
{
    using LearningHub.Nhs.LearningCredentials.Models.Dsp;

    /// <summary>
    /// Defines the <see cref="UserVerifiableCredentialModel" />.
    /// </summary>
    public class UserVerifiableCredentialModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserVerifiableCredentialModel"/> class.
        /// </summary>
        public UserVerifiableCredentialModel()
        {
            this.UserVerifiableCredentials = new List<UserVerifiableCredentialResponse>();
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the verifiable credential name.
        /// </summary>
        public string CredentialName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user verifiable credentials.
        /// </summary>
        public List<UserVerifiableCredentialResponse> UserVerifiableCredentials { get; set; }
    }
}
