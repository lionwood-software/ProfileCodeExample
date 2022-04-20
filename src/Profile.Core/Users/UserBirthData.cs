using System;
using System.ComponentModel.DataAnnotations.Schema;
using Profile.Core.Locations;
using Profile.Core.SharedKernel;

namespace Profile.Core.Users
{
    public class UserBirthData : IAuditable
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? BirthDate { get; set; }
        public int? CountryId { get; set; }
        public int? RegionId { get; set; }
        public string Place { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public Country Country { get; set; }
        public Region Region { get; set; }
    }
}
