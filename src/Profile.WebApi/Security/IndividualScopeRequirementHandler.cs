using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Profile.WebApi.Security
{
    public class IndividualScopeRequirementHandler : AuthorizationHandler<IndividualScopeRequirement>
    {
        private const string OneLoginScopeClaim = "scope";
        private const string IndividualScope = "individual";

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IndividualScopeRequirement requirement)
        {
            var scopeClaim = context.User.Claims.FirstOrDefault(c => c.Type == OneLoginScopeClaim)?.Value;
            var scopes = scopeClaim?.Split(" ");
            if (scopes != null && scopes.Contains(IndividualScope))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}