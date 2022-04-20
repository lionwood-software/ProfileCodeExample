using System;
using Profile.Core.Locations;
using Profile.Core.SharedKernel;

namespace Profile.Core.Extra.User
{
    /// <summary>
    /// Represents where user would be able to work extras
    /// </summary>
    public class UserExtraCity : IAuditable
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int CityId { get; set; }

        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public Users.User User { get; set; }
        public City City { get; set; }
    }
}