// <copyright file="BaseController.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserProfileUI.Controllers
{
    using LearningHub.Nhs.Models.Extensions;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Defines the <see cref="BaseController" />.
    /// </summary>
    public class BaseController : Controller
    {
        /// <summary>
        /// Gets the current user id.
        /// </summary>
        protected int CurrentUserId => this.User.Identity.GetCurrentUserId();
    }
}
