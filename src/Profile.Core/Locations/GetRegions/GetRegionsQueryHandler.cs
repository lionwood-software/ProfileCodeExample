using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Profile.Core;
using Profile.Core.Locations;

namespace Profile.Core.Locations.GetRegions
{
    public class GetRegionsQueryHandler : IRequestHandler<GetRegionsQuery, IEnumerable<LocationItem>>
    {
        private readonly ProfileDbContext dbContext;

        public GetRegionsQueryHandler(ProfileDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<LocationItem>> Handle(GetRegionsQuery request, CancellationToken cancellationToken)
        {
            return await dbContext.Regions.Where(x => x.CountryId == request.CountryId)
                .Select(keyValue =>
                    new LocationItem
                    {
                        Id = keyValue.Id,
                        Name = keyValue.Name
                    })
                .ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
