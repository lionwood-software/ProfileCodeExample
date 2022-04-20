using System;
using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Profile.Core;
using Profile.Core.SharedKernel;

namespace Profile.WebApi.Infrastructure
{
    public class ProfileUserContext : UserContext, IProfileUserContext
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public ProfileUserContext(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string Email => httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value
            ?? throw new Exception("An error occured while receiving email from user claims");

        public bool HasUserId() => httpContextAccessor.HttpContext.User.FindFirst(ProjectClaimType.ProjectSub) != null;

        public void SetCulture(string culture)
        {
            CultureInfo.CurrentUICulture = new CultureInfo(culture);
        }

        public string Culture => CultureInfo.CurrentUICulture.Name;
    }
}