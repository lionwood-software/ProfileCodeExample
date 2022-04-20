using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Profile.Core.Locations.GetCountries
{
    public class GetCountriesQueryHandler : IRequestHandler<GetCountriesQuery, IEnumerable<LocationItem>>
    {
        private readonly ProfileDbContext dbContext;

        public GetCountriesQueryHandler(ProfileDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<LocationItem>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
        {
            return await dbContext.Countries.Select(keyValue =>
                new LocationItem
                {
                    Id = keyValue.Id,
                    Name = keyValue.Translations.FirstOrDefault().Name
                })
                .ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
