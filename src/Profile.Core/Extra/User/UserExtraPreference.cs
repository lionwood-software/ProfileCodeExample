using System;
using Profile.Core.SharedKernel;

namespace Profile.Core.Extra.User
{
    public class UserExtraPreference : IAuditable
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int JobCategoryId { get; set; }
        public bool WillDo { get; set; }
        public bool CanDo { get; set; }

        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public Users.User User { get; set; }
        public JobCategory JobCategory { get; set; }
    }
}