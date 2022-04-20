using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Profile.Core.Extensions;
using Profile.Core.Languages;
using Profile.Core.SharedKernel;
using Profile.Core.Users.PersonalInfo.Summary;

namespace Profile.Core.Users.PersonalInfo
{
    public class PersonalInfoHandler : IRequestHandler<GetPhoneNumberQuery, string>,
        IRequestHandler<GetLanguageProficiencyQuery, List<LanguageProficiency>>,
        IRequestHandler<GetUserSummaryQuery, UserSummary>
    {
        private readonly ProfileDbContext dbContext;
        private readonly IProfileUserContext profileUserContext;

        public PersonalInfoHandler(ProfileDbContext dbContext, IProfileUserContext profileUserContext)
        {
            this.dbContext = dbContext;
            this.profileUserContext = profileUserContext;
        }

        public async Task<string> Handle(GetPhoneNumberQuery request, CancellationToken cancellationToken)
            => await dbContext.Users
            .Where(x => x.Id == profileUserContext.UserId)
            .Select(x => x.PhoneNumber)
            .SingleOrDefaultAsync(cancellationToken);

        public async Task<List<LanguageProficiency>> Handle(GetLanguageProficiencyQuery request, CancellationToken cancellationToken)
            => await dbContext.UserLanguageProficiencies
                .Where(x => x.UserId == profileUserContext.UserId)
                .Select(x => new LanguageProficiency
                {
                    LanguageCode = x.LanguageCode,
                    ProficiencyLevel = x.ProficiencyId,
                })
                .ToListAsync(cancellationToken);

        public async Task<UserSummary> Handle(GetUserSummaryQuery request, CancellationToken cancellationToken)
        {
            var summary = await (from user in dbContext.Users
                                 let cities = dbContext.UserExtraCities.Where(city => city.UserId == user.Id).ToList()
                                 let preferences = dbContext.UserExtraPreferences.Where(preference => preference.UserId == user.Id).ToList()
                                 let languageProficiencies = dbContext.UserLanguageProficiencies.Where(proficiency => proficiency.UserId == user.Id).ToList()
                                 let userBirthDataSummary = dbContext.UserBirthData.SingleOrDefault(x => x.UserId == user.Id)
                                 let userWorkPermits = dbContext.UserCountryWorkPermits.Where(x => x.UserId == user.Id).ToList()
                                 let activeCountries = dbContext.ActiveCountries.OrderBy(o => o.ItemOrder).ToList()
                                 where user.Id == profileUserContext.UserId
                                 select new UserSummary
                                 {
                                     AvatarUrl = user.AvatarUrl,
                                     FirstName = user.FirstName,
                                     LastName = user.LastName,
                                     PhoneNumber = user.PhoneNumber,
                                     Nir = user.Nir,
                                     ExtraCities = cities.Select(ec => new UserExtraCitySummary
                                     {
                                         CityName = ec.City.Translations.FirstOrDefault().Name,
                                         CountryName = ec.City.Country.Translations.FirstOrDefault().Name,
                                     }),
                                     LanguageProficiencies = languageProficiencies.Select(lp => new UserLanguageProficiencySummary
                                     {
                                         Language = lp.Language.Translations.FirstOrDefault().Name,
                                         ProficiencyLevel = lp.Proficiency.Translations.FirstOrDefault().Name
                                     }),
                                     ExtraPreferences = preferences.Select(ep => new UserExtraPreferenceSummary
                                     {
                                         JobCategoryName = ep.JobCategory.Translations.FirstOrDefault().Name,
                                         WillDo = ep.WillDo,
                                         CanDo = ep.CanDo
                                     }),
                                     UserBirthData = userBirthDataSummary != null ?
                                         new UserBirthDataSummary
                                         {
                                             BirthDate = userBirthDataSummary.BirthDate,
                                             Country = userBirthDataSummary.CountryId != null ? userBirthDataSummary.Country.Name : null,
                                             Region = userBirthDataSummary.RegionId != null ? userBirthDataSummary.Region.Name : null,
                                             Place = userBirthDataSummary.Place
                                         } : null,
                                     UserCountryWorkPermits = activeCountries.Select(c => new UserCountryWorkPermitSummary
                                     {
                                         CountryId = c.Id,
                                         Country = c.Country.Translations.FirstOrDefault().Name,
                                         HasWorkPermit = userWorkPermits.SingleOrDefault(p => p.CountryId == c.Id) != null ?
                                              userWorkPermits.SingleOrDefault(p => p.CountryId == c.Id).HasWorkPermit : null
                                     })
                                 })
                .SingleOrDefaultAsync(cancellationToken)
                .ThrowIfNotFound();

            summary.Language = LanguagesHelper.GetSupportedLanguages()[profileUserContext.Culture]
                .FirstOrDefault(x => x.Culture == profileUserContext.Culture).Name;

            return summary;
        }
    }
}