using System.Collections.Generic;
using MediatR;

namespace Profile.Core.Locations.GetActiveLocations
{
    public class GetActiveLocationsQuery : IRequest<IEnumerable<ActiveLocation>>
    {
    }
}
