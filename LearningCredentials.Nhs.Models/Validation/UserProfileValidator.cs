// <copyright file="UserProfileValidator.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.LearningCredentials.Models.Validation
{
    using FluentValidation;
    using LearningHub.Nhs.LearningCredentials.Models.Entities.User;

    /// <summary>
    /// The user validator.
    /// </summary>
    public class UserProfileValidator : AbstractValidator<UserProfile>
    {
        /// <summary>
        /// The name max length.
        /// </summary>
        public const int UserNameMaxLength = 50;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileValidator"/> class.
        /// </summary>
        public UserProfileValidator()
        {
            this.RuleFor(x => x.Id)
            .NotEqual(0)
            .WithMessage("Id is mandatory.");

            this.RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("UserName is mandatory.")
            .MaximumLength(UserNameMaxLength)
            .WithMessage(string.Format("Name length cannot exceed {0} characters.", UserNameMaxLength));
        }
    }
}
