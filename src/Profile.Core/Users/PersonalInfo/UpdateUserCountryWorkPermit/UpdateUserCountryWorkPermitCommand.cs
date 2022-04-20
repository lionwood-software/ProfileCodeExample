using System.Collections.Generic;
using MediatR;

namespace Profile.Core.Users.PersonalInfo.UpdateUserCountryWorkPermit
{
    public class UpdateUserCountryWorkPermitCommand : IRequest
    {
        public List<CountryWorkPermit> CountryWorkPermits { get; set; }
    }
}
