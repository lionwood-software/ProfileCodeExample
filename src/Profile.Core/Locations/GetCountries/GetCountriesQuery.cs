using System.Collections.Generic;
using MediatR;

namespace Profile.Core.Locations.GetCountries
{
    public class GetCountriesQuery : IRequest<IEnumerable<LocationItem>>
    {
    }
}
