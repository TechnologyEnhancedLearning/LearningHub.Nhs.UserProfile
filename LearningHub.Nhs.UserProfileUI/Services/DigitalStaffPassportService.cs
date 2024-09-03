// <copyright file="DigitalStaffPassportService.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserProfileUI.Services
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using LearningHub.Nhs.Caching;
    using LearningHub.Nhs.LearningCredentials.Models.Dsp;
    using LearningHub.Nhs.LearningCredentials.Models.Entities.Dsp;
    using LearningHub.Nhs.LearningCredentials.Models.Enums.Dsp;
    using LearningHub.Nhs.Models.Common;
    using LearningHub.Nhs.Models.Search;
    using LearningHub.Nhs.UserProfileUI.Configuration;
    using LearningHub.Nhs.UserProfileUI.Helper;
    using LearningHub.Nhs.UserProfileUI.Interfaces;
    using LearningHub.Nhs.UserProfileUI.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using Newtonsoft.Json;

    /// <summary>
    /// Defines the <see cref="UserProfileService" />.
    /// </summary>
    public class DigitalStaffPassportService : IDigitalStaffPassportService
    {
        private readonly ILearningCredentialsApiFacade lcApiFacade;
        private readonly IDspGatewayApiHttpClient dspGatewayApiHttpClient;
        private readonly ICacheService cacheService;
        private readonly WebSettings webSettings;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DigitalStaffPassportService"/> class.
        /// </summary>
        /// <param name="lcApiFacade">The dspGatewayApiHttpClient<see cref="ILearningCredentialsApiFacade"/>.</param>
        /// <param name="dspGatewayApiHttpClient">The dspGatewayApiHttpClient<see cref="IDspGatewayApiHttpClient"/>.</param>
        /// <param name="cacheService">The CacheService <see cref="ICacheService"/>.</param>
        /// <param name="webSettings">The webSettings <see cref="WebSettings"/>.</param>
        /// <param name="logger">The logger.</param>
        public DigitalStaffPassportService(
            ILearningCredentialsApiFacade lcApiFacade,
            IDspGatewayApiHttpClient dspGatewayApiHttpClient,
            ICacheService cacheService,
            IOptions<WebSettings> webSettings,
            ILogger<DigitalStaffPassportService> logger)
        {
            this.lcApiFacade = lcApiFacade;
            this.cacheService = cacheService;
            this.dspGatewayApiHttpClient = dspGatewayApiHttpClient;
            this.webSettings = webSettings.Value;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<string> GetAuthUrl(int verifiableCredentialId, int currentUserId)
        {
            var verifiableCredential = await this.GetVerifiableCredentialById(verifiableCredentialId);
            var userClientSystemCredential = await this.GetClientSystemCredentialForCurrentUser(verifiableCredentialId);

            var userVerifiableCredential = new UserVerifiableCredentialRequest()
            {
                VerifiableCredentialId = verifiableCredentialId,
                ActivityDate = userClientSystemCredential.ActivityDate,
                AttainmentStatus = userClientSystemCredential.AttainmentStatus,
            };

            var claims = await this.PopulateCertificateClaimsAsync(verifiableCredential, userClientSystemCredential, currentUserId);
            var entry = string.Join(", ", claims.Select(kvp => $"{kvp.Key}: {kvp.Value}"));
            this.logger.LogError($"claims-{entry}");
            DspAuthorisationResponse dspAuthorisationResponse = await this.CreateCredential(verifiableCredential, claims);
            string fullRedirectUrl = string.Format("{0}/{1}", this.webSettings.DspSettings.DspGatewayUrl.TrimEnd('/'), this.webSettings.DspSettings.AuthorisationRedirectUrl.TrimStart('/'));

            var cacheModel = new DspCacheProgressModel()
            {
                UserVerifiableCredentialRequest = userVerifiableCredential,
                Nonce = dspAuthorisationResponse.Nonce,
            };

            _ = await this.cacheService.SetAsync<DspCacheProgressModel>($"{currentUserId}:DspProcessCredential", cacheModel);

            return string.Format(fullRedirectUrl, this.webSettings.DspSettings.ClientId, dspAuthorisationResponse.RequestUri);
        }

        /// <inheritdoc/>
        public async Task<UserVerifiableCredentialResponse> ProcessTokenResponse(string code, int currentUserId) ////, UserVerifiableCredentialResponse userVerifiableCredentialResponse)
        {
            var tokenResponse = await this.RequestToken(code);

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = (JwtSecurityToken)tokenHandler.ReadToken(tokenResponse.IdToken);
            var claims = securityToken.Claims;
            var nonceValue = claims.Where(c => c.Type.ToLower() == "nonce")?.FirstOrDefault()?.Value ?? string.Empty;

            var (cacheExists, cacheModel) = await this.cacheService.TryGetAsync<DspCacheProgressModel>($"{currentUserId}:DspProcessCredential");

            if (nonceValue == cacheModel.Nonce)
            {
                var userVerifiableCredentialResponse = new UserVerifiableCredentialResponse()
                {
                    VerifiableCredentialId = cacheModel.UserVerifiableCredentialRequest.VerifiableCredentialId,
                    ActivityDate = cacheModel.UserVerifiableCredentialRequest.ActivityDate,
                    AttainmentStatus = cacheModel.UserVerifiableCredentialRequest.AttainmentStatus,
                    UserId = currentUserId,
                    AddedToDSPDate = DateTimeOffset.Now,
                    DSPCode = code,
                    SerialNumber = claims.Where(c => c.Type == "SerialNumber")?.FirstOrDefault()?.Value ?? string.Empty,
                    Status = UserVerifiableCredentialStatusEnum.Created,
                };

                var response = await this.UpdateUserVerifiableCredential(userVerifiableCredentialResponse);

                if (response.Success)
                {
                    userVerifiableCredentialResponse.UserVerifiableCredentialId = response?.ValidationResult?.CreatedId ?? 0;
                    return userVerifiableCredentialResponse;
                }
                else
                {
                    throw new Exception("Failed to process DSP token response");
                }
            }
            else
            {
                throw new Exception("DSP validation failure!");
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Claim>> GetCredentialDetails(string code)
        {
            var tokenResponse = await this.RequestToken(code);

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = (JwtSecurityToken)tokenHandler.ReadToken(tokenResponse.IdToken);
            var claims = securityToken.Claims;

            return claims.OrderBy(c => c.Type);
        }

        /// <inheritdoc/>
        public async Task<VerifiableCredentialResponse> GetVerifiableCredentialById(int verifiableCredentialId)
        {
            return await this.lcApiFacade.GetAsync<VerifiableCredentialResponse>($"VerifiableCredential/GetById/{verifiableCredentialId}");
        }

        /// <inheritdoc/>
        public async Task<List<VerifiableCredentialResponse>> GetVerifiableCredentials()
        {
            return await this.lcApiFacade.GetAsync<List<VerifiableCredentialResponse>>($"VerifiableCredential/GetAll");
        }

        /// <inheritdoc/>
        public async Task<List<UserVerifiableCredentialResponse>> GetCurrentUserUserVerifiableCredentials(int id)
        {
            return await this.lcApiFacade.GetAsync<List<UserVerifiableCredentialResponse>>($"VerifiableCredential/GetAll");
        }

        /// <inheritdoc/>
        public async Task<UserVerifiableCredentialResponse> GetUserVerifiableCredentialById(int userVerifiableCredentialId)
        {
            return await this.lcApiFacade.GetAsync<UserVerifiableCredentialResponse>($"VerifiableCredential/GetUserVerifiableCredentialById/{userVerifiableCredentialId}");
        }

        /// <inheritdoc/>
        public async Task<List<UserVerifiableCredentialResponse>> GetCurrentUserVerifiableCredentialsSummary()
        {
            return await this.lcApiFacade.GetAsync<List<UserVerifiableCredentialResponse>>($"VerifiableCredential/GetSummaryForCurrentUser");
        }

        /// <inheritdoc/>
        public async Task<List<UserVerifiableCredentialResponse>> GetCurrentUserVerifiableCredentials()
        {
            return await this.lcApiFacade.GetAsync<List<UserVerifiableCredentialResponse>>($"VerifiableCredential/GetForCurrentUser");
        }

        /// <inheritdoc/>
        public async Task<List<UserVerifiableCredentialResponse>> GetCurrentUserVerifiableCredentialsById(int verifiableCredentialId)
        {
            return await this.lcApiFacade.GetAsync<List<UserVerifiableCredentialResponse>>($"VerifiableCredential/GetCurrentUserVerifiableCredentialsById/{verifiableCredentialId}");
        }

        /// <inheritdoc/>
        public async Task<ClientSystemCredentialResult> GetClientSystemCredentialForCurrentUser(int verifiableCredentialId)
        {
            return await this.lcApiFacade.GetAsync<ClientSystemCredentialResult>($"VerifiableCredential/GetClientSystemCredentialForCurrentUser/{verifiableCredentialId}");
        }

        /// <inheritdoc/>
        public async Task<ApiResponse> UpdateUserVerifiableCredential(UserVerifiableCredentialRequest userVerifiableCredentialRequest)
        {
            if (userVerifiableCredentialRequest.UserVerifiableCredentialId == 0)
            {
                return await this.lcApiFacade.PostAsync<ApiResponse, UserVerifiableCredentialRequest>($"VerifiableCredential/CreateForUser", userVerifiableCredentialRequest);
            }
            else
            {
                return await this.lcApiFacade.PutAsync<ApiResponse, UserVerifiableCredentialRequest>($"VerifiableCredential/UpdateForUser", userVerifiableCredentialRequest);
            }
        }

        /// <inheritdoc/>
        public string GetVerifyCredentialUrl(string credentialType)
        {
            string fullRedirectUrl = string.Format("{0}/{1}", this.webSettings.DspSettings.DspGatewayUrl.TrimEnd('/'), this.webSettings.DspSettings.VerifyCredentialUrl.TrimStart('/'));
            Uri userProfileUri = new Uri(this.webSettings.UserProfileUrl);
            Uri redirectUrl = new Uri(userProfileUri, "dspreturn");

            var codeChallenge = "mGpLgDQ0WpocpKqbDEOvwnEOd4DaWkZue346M936HEE="; // TODO is the static or passed back for verification?

            return string.Format(fullRedirectUrl, this.webSettings.DspSettings.ClientId, redirectUrl, credentialType, codeChallenge);
        }

        /// <inheritdoc/>
        public async Task RevokeUserVerifiableCredentials(int verifiableCredentialId)
        {
            var userVerifiableCredentials = await this.GetCurrentUserVerifiableCredentialsById(verifiableCredentialId);

            foreach (var uvc in userVerifiableCredentials.Where(uvc => uvc.Status == UserVerifiableCredentialStatusEnum.Created))
            {
                await this.RevokeVerifiableCredential(uvc);
            }
        }

        private async Task<bool> RevokeVerifiableCredential(UserVerifiableCredentialResponse userVerifiableCredential)
        {
            var verifiableCredential = await this.GetVerifiableCredentialById(userVerifiableCredential.VerifiableCredentialId);

            var payload = new
            {
                OrganisationId = this.webSettings.DspSettings.HEEOrgIdentifier,
                CredentialTemplateName = verifiableCredential.ScopeName,
                SerialNumber = userVerifiableCredential.SerialNumber,
                RevocationReason = "Revoked by user",
                client_id = this.webSettings.DspSettings.ClientId,
                client_secret = this.webSettings.DspSettings.ClientSecret,
            };
            var jsonData = JsonConvert.SerializeObject(payload);

            HttpClient client = new HttpClient();
            string url = this.webSettings.DspSettings.DspGatewayUrl + this.webSettings.DspSettings.RevokeCredentialUrl;

            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, stringContent).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                userVerifiableCredential.Status = UserVerifiableCredentialStatusEnum.Revoked;
                await this.UpdateUserVerifiableCredential(userVerifiableCredential);

                return true;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized || response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                throw new Exception("AccessDenied");
            }
            else
            {
                throw new Exception("save failed!");
            }
        }

        private async Task<Dictionary<string, string>> PopulateCertificateClaimsAsync(VerifiableCredentialResponse verifiableCredential, ClientSystemCredentialResult clientSystemCredentialResult, int currentUserId)
        {
            var claims = new Dictionary<string, string>();

            var dateAwarded = clientSystemCredentialResult.ActivityDate;
            var toDate = dateAwarded.AddYears(verifiableCredential.PeriodQty);
            var dspIdentityCacheKey = $"DspIdentity:{currentUserId}";
            var dspResult = await this.cacheService.GetAsync<string>(dspIdentityCacheKey);

            claims.Add($"{verifiableCredential.ClaimPrefix}-StatMandSubject", verifiableCredential.CredentialName);
            claims.Add($"{verifiableCredential.ClaimPrefix}-Level", verifiableCredential.Level.ToString());
            claims.Add($"{verifiableCredential.ClaimPrefix}-AttainmentStatus", clientSystemCredentialResult.AttainmentStatus);
            claims.Add($"{verifiableCredential.ClaimPrefix}-CompetencyFramework", "CSTF");
            claims.Add($"{verifiableCredential.ClaimPrefix}-CompetencyName", verifiableCredential.CredentialName);
            claims.Add($"{verifiableCredential.ClaimPrefix}-LastDateAwarded", dateAwarded.ToString("o"));
            claims.Add($"{verifiableCredential.ClaimPrefix}-RenewalPeriod", verifiableCredential.PeriodQty.ToString());
            claims.Add($"{verifiableCredential.ClaimPrefix}-Units", verifiableCredential.PeriodUnit.ToString());
            claims.Add($"{verifiableCredential.ClaimPrefix}-LearningMethod", "elearning");
            claims.Add($"{verifiableCredential.ClaimPrefix}-DateTo", toDate.ToString("o"));
            claims.Add($"{verifiableCredential.ClaimPrefix}-Origin", "elfh Hub");
            claims.Add($"{verifiableCredential.ClaimPrefix}-AssurancePolicy", "Core Skills Training Framework (CSTF)");
            claims.Add($"{verifiableCredential.ClaimPrefix}-AssuranceOutcome", "Evidence of completion of topic");
            claims.Add($"{verifiableCredential.ClaimPrefix}-Provider", "NHS England TEL");
            claims.Add($"{verifiableCredential.ClaimPrefix}-Verifier", "NHS England TEL");
            claims.Add($"{verifiableCredential.ClaimPrefix}-VerificationMethod", "elearning completion");
            claims.Add($"{verifiableCredential.ClaimPrefix}-LastRefresh", DateTimeOffset.Now.ToString("o"));

            claims.Add("UniqueIdentifier", dspResult);

            return claims;
        }

        private async Task<DspAuthorisationResponse> CreateCredential(VerifiableCredentialResponse verifiableCredential, Dictionary<string, string> claims)
        {
            var idTokenHint = this.Id_Token_Hint_Raw(claims);
            var noncevalue = Guid.NewGuid().ToString();

            Uri userProfileUri = new Uri(this.webSettings.UserProfileUrl);
            Uri redirectUrl = new Uri(userProfileUri, "dspreturn");

            string payload = $"client_id={this.webSettings.DspSettings.ClientId}"
                           + $"&client_secret={this.webSettings.DspSettings.ClientSecret}"
                           + $"&redirect_uri={redirectUrl}"
                           + $"&scope=issue.{verifiableCredential.ScopeName}"
                           + $"&id_token_hint={idTokenHint}"
                           + "&state=credentialCreation"
                           + "&response_type=code"
                           + "&response_mode=query"
                           + $"&nonce={noncevalue}";

            this.logger.LogError($"queryString--{payload}");
            HttpClient client = new HttpClient();
            string url = this.webSettings.DspSettings.DspGatewayUrl + this.webSettings.DspSettings.AuthorisationRequestUrl;
            this.logger.LogError($"url--{url}");
            StringContent stringContent = new StringContent(payload, Encoding.UTF8, "application/x-www-form-urlencoded");
            try
            {
                var dataPayload = await stringContent.ReadAsStringAsync();
                this.logger.LogError($"payload-{dataPayload}");
            }
            catch (Exception)
            {
            }

            var response = await client.PostAsync(url, stringContent).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                DspAuthorisationResponse apiResponse = JsonConvert.DeserializeObject<DspAuthorisationResponse>(result) !;
                apiResponse.Nonce = noncevalue;

                return apiResponse;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized || response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                throw new Exception("AccessDenied");
            }
            else
            {
                this.logger.LogError(response.ReasonPhrase);
                throw new Exception("save failed!");
            }
        }

        private async Task<DspGetTokenResponse> RequestToken(string code)
        {
            Uri userProfileUri = new Uri(this.webSettings.UserProfileUrl);
            Uri redirectUrl = new Uri(userProfileUri, "dspreturn");

            string payload = $"grant_type=authorization_code"
                           + $"&client_id={this.webSettings.DspSettings.ClientId}"
                           + $"&client_secret={this.webSettings.DspSettings.ClientSecret}"
                           + $"&redirect_uri={redirectUrl}"
                           + $"&code={code}";

            HttpClient client = new HttpClient();
            string url = this.webSettings.DspSettings.DspGatewayUrl + this.webSettings.DspSettings.TokenRequestUrl;

            StringContent stringContent = new StringContent(payload, Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await client.PostAsync(url, stringContent).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                DspGetTokenResponse apiResponse = JsonConvert.DeserializeObject<DspGetTokenResponse>(result) !;

                return apiResponse;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized || response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                throw new Exception("AccessDenied");
            }
            else
            {
                throw new Exception("save failed!");
            }
        }

        private string Id_Token_Hint_Raw(Dictionary<string, string> model)
        {
            string issuingUrl = this.webSettings.DspSettings.DspGatewayUrl + this.webSettings.DspSettings.IssuingUrl;

            var exp = DateTime.Now.AddMinutes(this.webSettings.DspSettings.HttpExpiryMins);
            var attributes = new Dictionary<string, string>()
                                {
                                    { "exp", exp.Ticks.ToString() },
                                    { "iss", this.webSettings.DspSettings.ClientId },
                                    { "aud", issuingUrl },
                                };
            attributes = attributes.Union(model).ToDictionary(pair => pair.Key, pair => pair.Value);
            return this.GenerateToken(attributes);
        }

        private string GenerateToken(Dictionary<string, string> credential)
        {
            var mySecurityKey = new SymmetricSecurityKey(Convert.FromBase64String(this.webSettings.DspSettings.SigningKey));
            this.logger.LogError($"SigningKey-{this.webSettings.DspSettings.SigningKey}");
            var tokenHandler = new JwtSecurityTokenHandler();
            var claimidentity = new ClaimsIdentity();
            foreach (var claim in credential)
            {
                claimidentity.AddClaim(new Claim(claim.Key, claim.Value ?? string.Empty));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimidentity,
                SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
