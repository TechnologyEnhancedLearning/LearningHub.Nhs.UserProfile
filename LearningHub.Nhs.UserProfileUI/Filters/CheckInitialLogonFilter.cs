// <copyright file="CheckInitialLogonFilter.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserProfileUI.Filters
{
    using System.Threading.Tasks;
    using LearningHub.Nhs.Caching;
    using LearningHub.Nhs.Models.Extensions;
    using LearningHub.Nhs.UserProfileUI.Helper;
    using Microsoft.AspNetCore.Mvc.Filters;

    /// <summary>
    /// Defines the <see cref="CheckInitialLogonFilter" />.
    /// </summary>
    public class CheckInitialLogonFilter : ActionFilterAttribute, IActionFilter
    {
        private readonly ICacheService cacheService;
        private readonly IUserSessionHelper userSessionHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckInitialLogonFilter"/> class.
        /// </summary>
        /// <param name="cacheService">The cacheService<see cref="ICacheService"/>.</param>
        /// <param name="userSessionHelper">The userSessionHelper<see cref="IUserSessionHelper"/>.</param>
        public CheckInitialLogonFilter(ICacheService cacheService, IUserSessionHelper userSessionHelper)
        {
            this.cacheService = cacheService;
            this.userSessionHelper = userSessionHelper;
        }

        /// <summary>
        /// The OnActionExecutionAsync.
        /// </summary>
        /// <param name="context">The context<see cref="ActionExecutingContext"/>.</param>
        /// <param name="next">The next<see cref="ActionExecutionDelegate"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = context.HttpContext.User;

            if (user.Identity.IsAuthenticated)
            {
                var loginProcessCacheKey = $"{user.Identity.GetCurrentUserId()}:LoginProcess";
                var dspIdentityCacheKey = $"DspIdentity:{user.Identity.GetCurrentUserId()}";

                var (cacheExists, result) = await this.cacheService.TryGetAsync<string>(loginProcessCacheKey);

                if (cacheExists)
                {
                    var userId = user.Identity.GetCurrentUserId();
                    await this.userSessionHelper.StartSession(userId);
                    await this.cacheService.RemoveAsync(loginProcessCacheKey);

                    var (dspExists, dspResult) = await this.cacheService.TryGetAsync<string>(dspIdentityCacheKey);
                    if (dspExists)
                    {
                        await this.cacheService.RemoveAsync(dspIdentityCacheKey);
                    }
                }
            }

            await next();
        }
    }
}
