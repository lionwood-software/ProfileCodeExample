using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Profile.Core.SharedKernel;

namespace Profile.Core.Extra.GetUserPreferences
{
    public class GetUserPreferencesQueryHandler : IRequestHandler<GetUserPreferencesQuery, IEnumerable<UserPreference>>
    {
        private readonly ProfileDbContext dbContext;
        private readonly IProfileUserContext profileUserContext;

        public GetUserPreferencesQueryHandler(ProfileDbContext dbContext, IProfileUserContext profileUserContext)
        {
            this.dbContext = dbContext;
            this.profileUserContext = profileUserContext;
        }

        public async Task<IEnumerable<UserPreference>> Handle(GetUserPreferencesQuery request, CancellationToken cancellationToken)
            => await dbContext.JobCategories
            .Select(jobCategory => new UserPreference
            {
                JobCategoryId = jobCategory.Id,
                JobCategoryName = jobCategory.Translations.FirstOrDefault().Name,
                JobRoles = dbContext.JobRoles.Where(role => role.JobCategoryId == jobCategory.Id).Select(role => role.Translations.FirstOrDefault().Name).ToList(),
                CanDo = dbContext.UserExtraPreferences.Where(x => x.UserId == profileUserContext.UserId && x.JobCategoryId == jobCategory.Id).Select(x => (bool?)x.CanDo).SingleOrDefault(),
                WillDo = dbContext.UserExtraPreferences.Where(x => x.UserId == profileUserContext.UserId && x.JobCategoryId == jobCategory.Id).Select(x => (bool?)x.WillDo).SingleOrDefault()
            })
            .OrderBy(up => up.JobCategoryName)
            .ToListAsync(cancellationToken);
    }
}