using System.Collections.Generic;
using MediatR;

namespace Profile.Core.Locations.UpdateActiveLocations
{
    public class UpdateActiveLocationsCommand : IRequest
    {
        public IEnumerable<ExtrasLocation> Locations { get; set; }
    }
}
