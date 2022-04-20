using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Profile.Core.SharedKernel;

namespace Profile.Core.Matching.Extra
{

    public class MatchingRequirementsChecker : IMatchingRequirementsChecker
    {
        private readonly ProfileDbContext profileDbContext;
        private readonly IProfileUserContext profileUserContext;

        public MatchingRequirementsChecker(ProfileDbContext profileDbContext, IProfileUserContext profileUserContext)
        {
            this.profileDbContext = profileDbContext;
            this.profileUserContext = profileUserContext;
        }

        public async Task<MatchingFailure> Check(CancellationToken cancellationToken = default)
        {
            var missedRequirements = new List<MatchingRequirement>();
            await foreach (var requirementCheck in Requirements().WithCancellation(cancellationToken))
            {
                if (!requirementCheck.isValid)
                {
                    missedRequirements.Add(requirementCheck.requirement);
                }
            }

            return missedRequirements.Any() ? new MatchingFailure(missedRequirements) : null;
        }

        private async Task<(MatchingRequirement, bool)> CheckLocation()
        {
            return (
                MatchingRequirement.Location,
                await profileDbContext.UserExtraCities.Where(x => x.UserId == profileUserContext.UserId).AnyAsync()
            );
        }

        private async Task<(MatchingRequirement, bool)> CheckLanguagePreferences()
        {
            return (
                MatchingRequirement.Language,
                await profileDbContext.UserLanguageProficiencies.Where(x => x.UserId == profileUserContext.UserId).AnyAsync()
            );
        }

        private async Task<(MatchingRequirement, bool)> CheckUserExtraPreferences()
        {
            return (
                MatchingRequirement.JobPreference,
                await profileDbContext.UserExtraPreferences.Where(x => x.UserId == profileUserContext.UserId).AnyAsync(x => x.WillDo || x.CanDo)
            );
        }

        private async IAsyncEnumerable<(MatchingRequirement requirement, bool isValid)> Requirements()
        {
            yield return await CheckLocation();
            yield return await CheckLanguagePreferences();
            yield return await CheckUserExtraPreferences();
        }
    }
}