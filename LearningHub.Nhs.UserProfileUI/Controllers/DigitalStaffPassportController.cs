// <copyright file="DigitalStaffPassportController.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserProfileUI.Controllers
{
    using LearningHub.Nhs.UserProfileUI.Interfaces;
    using LearningHub.Nhs.UserProfileUI.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Defines the <see cref="DigitalStaffPassportController" />.
    /// </summary>
    //[Authorize]
    public class DigitalStaffPassportController : BaseController
    {
        private readonly ILogger<DigitalStaffPassportController> logger;
        private readonly IDigitalStaffPassportService digitalStaffPassportService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DigitalStaffPassportController"/> class.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILogger{HomeController}"/>.</param>
        /// <param name="digitalStaffPassportService">The digital staff passport service<see cref="IDigitalStaffPassportService"/>.</param>
        public DigitalStaffPassportController(ILogger<DigitalStaffPassportController> logger, IDigitalStaffPassportService digitalStaffPassportService)
        {
            this.logger = logger;
            this.digitalStaffPassportService = digitalStaffPassportService;
        }

        /// <summary>
        /// Displays the verifiable credentials.
        /// </summary>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> Credentials()
        {
            var verifiableCredentials = await this.digitalStaffPassportService.GetCurrentUserVerifiableCredentials();
            return this.View(verifiableCredentials);
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
        /// The Send Credential method.
        /// </summary>
        /// <param name="id">The verifiable credial id.</param>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> SendCredential(int id)
        {
            var url = await this.digitalStaffPassportService.GetAuthUrl(id, this.CurrentUserId);
            return this.Redirect(url);
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
                this.ViewBag.Error = $"Error: {error}: {error_description}";
                return this.View("DspError");
            }

            if (code == null)
            {
                return this.RedirectToAction("Credentials");
                ////return this.RedirectToAction("ListCredentials");
            }

            if (state == "credentialCreation")
            {
                return await this.RequestToken(code);
            }
            else if (state == "verifyCreation")
            {
                var claims = await this.digitalStaffPassportService.GetCredentialDetails(code);
                return this.View("ShowCredentialClaims", claims);
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

            // TODO - return to main summary page after testing is complete
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

            // TODO - return to main summary page after testing is complete
            return this.RedirectToAction("ListUserCredentials", new { id = userVerifiableCredential.VerifiableCredentialId });
        }
    }
}
