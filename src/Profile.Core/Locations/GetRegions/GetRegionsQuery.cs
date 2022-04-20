using System.Collections.Generic;
using MediatR;

namespace Profile.Core.Locations.GetRegions
{
    public class GetRegionsQuery : IRequest<IEnumerable<LocationItem>>
    {
        public int CountryId { get; set; }
    }
}
