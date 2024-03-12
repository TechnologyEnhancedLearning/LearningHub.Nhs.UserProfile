// <copyright file="EntityBase.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.LearningCredentials.Models
{
    using global::System.ComponentModel.DataAnnotations;

   /// <summary>
    /// The entity base.
    /// </summary>
    public class EntityBase
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether deleted.
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Gets or sets the create user id.
        /// </summary>
        public int CreateUserId { get; set; }

        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        public DateTimeOffset CreateDate { get; set; }

        /// <summary>
        /// Gets or sets the amend user id.
        /// </summary>
        public int AmendUserId { get; set; }

        /// <summary>
        /// Gets or sets the amend date.
        /// </summary>
        public DateTimeOffset AmendDate { get; set; }

        /// <summary>
        /// The is new.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool IsNew()
        {
            return this.Id == 0 && !this.Deleted;
        }
    }
}
