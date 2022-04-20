using System.Collections.Generic;

namespace Profile.Core.Extra
{
    public record UserPreference
    {
        public int JobCategoryId { get; init; }
        public string JobCategoryName { get; init; }
        public IEnumerable<string> JobRoles { get; init; }
        public bool? WillDo { get; init; }
        public bool? CanDo { get; init; }
    }
}