using System;
using Profile.Core.Locations;
using Profile.Core.SharedKernel;

namespace Profile.Core.Users
{
    public class UserCountryWorkPermit : IAuditable
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public int CountryId { get; set; }
        public bool? HasWorkPermit { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public Country Country { get; set; }
    }
}
