// <copyright file="DigitalStaffPassportController.cs" company="HEE.nhs.uk">
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
        /// Displays the verifiable credentials.
        /// </summary>
        /// <param name="id">Verifiable credential id.</param>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> ListUserCredentials(int id)
        {
            // TODO - Remove once thisis no longer redirected to
            var verifiableCredential = await this.digitalStaffPassportService.GetVerifiableCredentialById(id);
            var userVerifiableCredentials = await this.digitalStaffPassportService.GetCurrentUserVerifiableCredentialsById(id);

            var model = new UserVerifiableCredentialModel()
            {
                Id = verifiableCredential.Id,
                CredentialName = verifiableCredential.CredentialName,
                UserVerifiableCredentials = userVerifiableCredentials,
            };

            return this.View(model);
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
        /// Displays a single user verifiable credential.
        /// </summary>
        /// <param name="id">The verifiable credial id.</param>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> Details(int id)
        {
            var verifiableCredential = await this.digitalStaffPassportService.GetUserVerifiableCredentialById(id);
            return this.View(verifiableCredential);
        }

        /// <summary>
        /// Displays a resend confirmation screen.
        /// </summary>
        /// <param name="id">The verifiable credial id.</param>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public IActionResult ResendConfirmation(int id)
        {
            return this.View();
        }

        /// <summary>
        /// The Confirm Credential method.
        /// </summary>
        /// <param name="id">The verifiable credial id.</param>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> ConfirmCredential(int id)
        {
            var verifiableCredential = await this.digitalStaffPassportService.GetVerifiableCredentialById(id);
            var userClientSystemCredential = await this.digitalStaffPassportService.GetClientSystemCredentialForCurrentUser(id);

            var userVerifiableCredential = new UserVerifiableCredentialResponse()
            {
                VerifiableCredentialId = id,
                CredentialName = verifiableCredential.CredentialName,
                ActivityDate = userClientSystemCredential.ActivityDate,
                ExpiryDate = userClientSystemCredential.ActivityDate.AddYears(verifiableCredential.PeriodQty),
                RenewalPeriodText = verifiableCredential.PeriodQty.ToString() + " " + verifiableCredential.PeriodUnit.ToString().ToLower() + (verifiableCredential.PeriodQty > 1 ? "s" : string.Empty),
                AttainmentStatus = userClientSystemCredential.AttainmentStatus,
            };

            return this.View(userVerifiableCredential);
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
                this.TempData["Notification"] = $"Error: {error}: {error_description}";
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
        /// Revokes a credential.
        /// </summary>
        /// <param name="id">The User Verifiable Credential id.</param>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        [HttpGet]
        public async Task<IActionResult> Revoke(int id)
        {
            await this.digitalStaffPassportService.RevokeUserVerifiableCredentials(id);
            return this.RedirectToAction("ListUserCredentials", new { id = id });
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

        private async Task<IActionResult> RequestToken(string code)
        {
            var userVerifiableCredential = await this.digitalStaffPassportService.ProcessTokenResponse(code, this.CurrentUserId);
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
                    }
                }
                else
                {
                    this.TempData["Notification"] = "Identity Verification Failed";
                }
            }

            return this.RedirectToAction("Credentials");
        }
    }
}
