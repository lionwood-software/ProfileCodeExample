using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Profile.Core.SharedKernel;
using Profile.Core.Users.PersonalInfo;

namespace Profile.Core.Extra.GetUserExtraSummary
{
    public class GetUserExtraSummaryQueryHandler : IRequestHandler<GetUserExtraSummaryQuery, UserExtraSummary>
    {
        private readonly ProfileDbContext dbContext;
        private readonly IProfileUserContext profileUserContext;

        public GetUserExtraSummaryQueryHandler(
            ProfileDbContext dbContext,
            IProfileUserContext profileUserContext)
        {
            this.dbContext = dbContext;
            this.profileUserContext = profileUserContext;
        }

        public async Task<UserExtraSummary> Handle(GetUserExtraSummaryQuery request, CancellationToken cancellationToken)
        {
            var userId = request.UserId ?? profileUserContext.UserId;

            return await (from user in dbContext.Users
                          let cities = dbContext.UserExtraCities.Where(city => city.UserId == userId).ToList()
                          let preferences = dbContext.UserExtraPreferences.Where(preference => preference.UserId == userId).ToList()
                          let languageProficiencies = dbContext.UserLanguageProficiencies
                              .Where(proficiency => proficiency.UserId == userId).ToList()
                          let userBirthDataSummary = dbContext.UserBirthData.SingleOrDefault(x => x.UserId == user.Id)
                          let userWorkPermits = dbContext.UserCountryWorkPermits.Where(x => x.UserId == user.Id).ToList()
                          let activeCountries = dbContext.ActiveCountries.OrderBy(o => o.ItemOrder).ToList()
                          where user.Id == userId
                          select new UserExtraSummary
                          {
                              UserId = userId,
                              LanguageCode = user.LanguageCode,
                              Nir = user.Nir,
                              CityIds = cities.Select(x => x.CityId).ToList(),
                              LanguageProficiencies =
                                  languageProficiencies.Select(lp => new LanguageProficiency
                                  {
                                      LanguageCode = lp.LanguageCode,
                                      ProficiencyLevel = lp.ProficiencyId
                                  }).ToList(),
                              ExtraPreferences = preferences.Select(ep => new UserExtraPreference
                              {
                                  JobCategoryId = ep.JobCategoryId,
                                  WillDo = ep.WillDo,
                                  CanDo = ep.CanDo,
                                  RoleIds = ep.JobCategory.JobRoles.Select(x => x.Id).ToList()
                              }).ToList(),
                              UserBirthData = userBirthDataSummary != null ?
                               new UserExtraBirthDataSummary
                               {
                                   BirthDate = userBirthDataSummary.BirthDate,
                                   CountryId = userBirthDataSummary.CountryId
                               } : null,
                              UserCountryWorkPermits = activeCountries.Select(c => new UserExtraCountryWorkPermitSummary
                              {
                                  CountryId = c.Id,
                                  HasWorkPermit = userWorkPermits.SingleOrDefault(p => p.CountryId == c.Id) != null ?
                                       userWorkPermits.SingleOrDefault(p => p.CountryId == c.Id).HasWorkPermit : null
                              })
                          })
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
    }
}
