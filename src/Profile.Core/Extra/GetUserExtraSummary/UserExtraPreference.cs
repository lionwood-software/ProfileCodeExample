using System.Collections.Generic;

namespace Profile.Core.Extra.GetUserExtraSummary
{
    public record UserExtraPreference
    {
        public int JobCategoryId { get; init; }
        public IEnumerable<int> RoleIds { get; init; }
        public bool WillDo { get; init; }
        public bool CanDo { get; init; }
    }
}