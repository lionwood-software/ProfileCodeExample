using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Profile.Core.SharedKernel;

namespace Profile.Core.Locations.GetActiveLocations
{
    public class GetActiveLocationsQueryHandler : IRequestHandler<GetActiveLocationsQuery, IEnumerable<ActiveLocation>>
    {
        private readonly ProfileDbContext dbContext;
        private readonly IProfileUserContext profileUserContext;
        public GetActiveLocationsQueryHandler(ProfileDbContext dbContext, IProfileUserContext profileUserContext)
        {
            this.dbContext = dbContext;
            this.profileUserContext = profileUserContext;
        }

        public async Task<IEnumerable<ActiveLocation>> Handle(GetActiveLocationsQuery request, CancellationToken cancellationToken)
            => await dbContext.ActiveCities
            .Join(dbContext.ActiveCountries, c => c.City.CountryId, c => c.Id, (city, country) => new { activeCity = city, activeCountry = country })
            .OrderBy(x => x.activeCountry.ItemOrder).ThenBy(x => x.activeCity.IsUpcoming).ThenBy(x => x.activeCity.City.Translations.FirstOrDefault().Name)
            .Select(join => new ActiveLocation
            {
                CityId = join.activeCity.Id,
                CityName = join.activeCity.City.Translations.FirstOrDefault().Name,
                CountryId = join.activeCity.City.CountryId,
                CountryName = join.activeCity.City.Country.Translations.FirstOrDefault().Name,
                CountryOrder = join.activeCountry.ItemOrder,
                IsSelected = dbContext.UserExtraCities.Where(c => c.UserId == profileUserContext.UserId).Select(c => c.CityId).Contains(join.activeCity.Id),
                IsUpcoming = join.activeCity.IsUpcoming,
            })
            .ToListAsync(cancellationToken);
    }
}
