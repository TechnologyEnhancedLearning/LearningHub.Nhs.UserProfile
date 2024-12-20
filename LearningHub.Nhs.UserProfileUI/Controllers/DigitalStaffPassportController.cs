﻿// <copyright file="DigitalStaffPassportController.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserProfileUI.Controllers
{
    using System;
    using LearningHub.Nhs.Caching;
    using LearningHub.Nhs.LearningCredentials.Models.Dsp;
    using LearningHub.Nhs.LearningCredentials.Models.Entities.Dsp;
    using LearningHub.Nhs.Models.Entities.Hierarchy;
    using LearningHub.Nhs.UserProfileUI.Interfaces;
    using LearningHub.Nhs.UserProfileUI.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;

    /// <summary>
    /// Defines the <see cref="DigitalStaffPassportController" />.
    /// </summary>
    [Authorize]
    public class DigitalStaffPassportController : BaseController
    {
        private readonly ILogger<DigitalStaffPassportController> logger;
        private readonly IDigitalStaffPassportService digitalStaffPassportService;
        private readonly IUserService userService;
        private readonly ICacheService cacheService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DigitalStaffPassportController"/> class.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILogger{HomeController}"/>.</param>
        /// <param name="digitalStaffPassportService">The digital staff passport service<see cref="IDigitalStaffPassportService"/>.</param>
        /// <param name="userService">The user service.</param>
        /// <param name="cacheService">The cache service.</param>
        public DigitalStaffPassportController(ILogger<DigitalStaffPassportController> logger, IDigitalStaffPassportService digitalStaffPassportService, IUserService userService, ICacheService cacheService)
        {
            this.logger = logger;
            this.digitalStaffPassportService = digitalStaffPassportService;
            this.userService = userService;
            this.cacheService = cacheService;
        }

        /// <summary>
        /// The Index.
        /// </summary>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public IActionResult Index()
        {
            return this.RedirectToAction("Credentials", "DigitalStaffPassport");
        }

        /// <summary>
        /// Displays the verifiable credentials.
        /// </summary>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> Credentials()
        {
            var verifiableCredentials = await this.digitalStaffPassportService.GetCurrentUserVerifiableCredentials();
            var (cacheExists, result) = await this.cacheService.TryGetAsync<string>(this.DspIdentity);
            if (cacheExists && !string.IsNullOrWhiteSpace(result))
            {
                return this.View(new UserVerifiableCredentialandIdentityResponse { IdentityVerified = true, UserVerifiableCredentialResponse = verifiableCredentials });
            }

            return this.View(new UserVerifiableCredentialandIdentityResponse { UserVerifiableCredentialResponse = verifiableCredentials });
        }

        /// <summary>
        /// Displays the verifiable credentials.
        /// </summary>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> FullList()
        {
            var verifiableCredentials = await this.digitalStaffPassportService.GetVerifiableCredentials();
            return this.View(verifiableCredentials);
        }

        /// <summary>
        /// Displays the user's verifiable credentials.
        /// </summary>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> CredentialsSummary()
        {
            var verifiableCredentials = await this.digitalStaffPassportService.GetCurrentUserVerifiableCredentialsSummary();
            return this.View(verifiableCredentials);
        }

        /// <summary>
        /// Displays a resend confirmation screen.
        /// </summary>
        /// <param name="id">The verifiable credential id.</param>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public IActionResult ResendConfirmation(int id)
        {
            this.ViewBag.CredentialId = id;
            return this.View();
        }

        /// <summary>
        /// The Send Credential method.
        /// </summary>
        /// <param name="verifiableCredentialId">The verifiable credial id.</param>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        [HttpPost]
        public async Task<IActionResult> SendCredential(int verifiableCredentialId)
        {
            try
            {
                var url = await this.digitalStaffPassportService.GetAuthUrl(verifiableCredentialId, this.CurrentUserId);
                return this.Redirect(url);
            }
            catch (Exception ex)
            {
                this.ViewBag.Error = ex.Message;
                return this.View("DspError");
            }
        }

        /// <summary>
        /// The Error.
        /// </summary>
        /// <param name="code">The returned code parameter.</param>
        /// <param name="state">The returned state parameter.</param>
        /// <param name="error">The returned error type.</param>
        /// <param name="error_description">The returned error message.</param>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        [Route("/dspreturn")]
        public async Task<IActionResult> DspReturn(string code, string state, string error, string error_description)
        {
            if (error != null || error_description != null)
            {
                this.logger.LogError($"Error: {error}/rDescription: {error_description}");
                if (!string.IsNullOrWhiteSpace(error_description) && !error_description.ToLower().Contains("user cancelled"))
                {
                    this.TempData["Notification"] = $"{error_description}";
                }

                return this.RedirectToAction("Credentials");
            }

            if (code == null)
            {
                return this.RedirectToAction("Credentials");
            }

            if (state == "credentialCreation")
            {
                return await this.RequestToken(code);
            }
            else if (state == "verifyCreation")
            {
                return await this.PersistIdentityVerification(code);
            }
            else
            {
                return this.View("Error");
            }
        }

        /// <summary>
        /// Display the page on the gateway to verify credential.
        /// </summary>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        [HttpGet]
        public async Task<IActionResult> VerifyCredential()
        {
            var credentialTypeList = await this.digitalStaffPassportService.GetVerifiableCredentials();
            return this.View(credentialTypeList.Select(c => c.ScopeName));
        }

        /// <summary>
        /// Display the page on the gateway to verify identity.
        /// </summary>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        [HttpGet]
        public IActionResult VerifyIdentity()
        {
            var url = this.digitalStaffPassportService.GetVerifyCredentialUrl("Identity");
            return this.Redirect(url);
        }

        /// <summary>
        /// Display the page on the gateway to verify credential.
        /// </summary>
        /// <param name="credentialType">The type of parameter.</param>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        [HttpPost]
        public IActionResult VerifyCredential(string credentialType)
        {
            var url = this.digitalStaffPassportService.GetVerifyCredentialUrl(credentialType);
            return this.Redirect(url);
        }

        private string UpdateCredentialNameWithLevel(string input, int level)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            int dashIndex = input.IndexOf('-');
            string modifiedInput = dashIndex >= 0
                ? input.Insert(dashIndex, $" - Level {level}")
                : $"{input} - Level {level}";
            return char.ToUpper(modifiedInput[0]) + modifiedInput.Substring(1).ToLower();
        }

        private async Task<IActionResult> RequestToken(string code)
        {
            var userVerifiableCredential = await this.digitalStaffPassportService.ProcessTokenResponse(code, this.CurrentUserId);
            if (userVerifiableCredential != null && userVerifiableCredential.UserVerifiableCredentialId > 0)
            {
                var userVerifiableCredentials = await this.digitalStaffPassportService.GetCurrentUserVerifiableCredentialsById(userVerifiableCredential.VerifiableCredentialId);
                if (userVerifiableCredentials != null && userVerifiableCredentials.Any())
                {
                    var verifiableCredential = await this.digitalStaffPassportService.GetVerifiableCredentialById(userVerifiableCredential.VerifiableCredentialId);
                    var credentialNameWithLevel = this.UpdateCredentialNameWithLevel(verifiableCredential.CredentialName, verifiableCredential.Level);
                    string message = userVerifiableCredentials.Count() > 1
                        ? $"{credentialNameWithLevel} credential readded to wallet"
                        : $"{credentialNameWithLevel} credential added to wallet";

                    this.TempData["Notification"] = $"Success: {message}";
                }
            }

            return this.RedirectToAction("Credentials");
        }

        private async Task<IActionResult> PersistIdentityVerification(string code)
        {
            var claims = await this.digitalStaffPassportService.GetCredentialDetails(code);
            if (claims != null && claims.Any())
            {
                var firstName = claims.FirstOrDefault(x => x.Type == "Identity.ID-LegalFirstName")?.Value;
                var lastName = claims.FirstOrDefault(x => x.Type == "Identity.ID-LegalSurname")?.Value;

                var userDetails = await this.userService.GetElfhUserByUserIdAsync(this.CurrentUserId);
                if (firstName?.ToString().ToUpper() == userDetails.FirstName.ToUpper() &&
                    lastName?.ToString().ToUpper() == userDetails.LastName.ToUpper())
                {
                    var uniqueIdentifier = claims.FirstOrDefault(x => x.Type == "Identity.UniqueIdentifier");
                    if (uniqueIdentifier != null)
                    {
                        await this.cacheService.SetAsync(this.DspIdentity, uniqueIdentifier.Value);
                        this.TempData["Notification"] = "Success: Identity verified";
                    }
                }
                else
                {
                    this.TempData["Notification"] = "Identity verification failed";
                }
            }

            return this.RedirectToAction("Credentials");
        }
    }
}
