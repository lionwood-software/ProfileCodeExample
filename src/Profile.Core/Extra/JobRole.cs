using System.Collections.Generic;
using Profile.Core.SharedKernel;

namespace Profile.Core.Extra
{
    public class JobRole : IHasTranslation<JobRoleTranslation>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int JobCategoryId { get; set; }

        public JobCategory JobCategory { get; set; }
        public ICollection<JobRoleTranslation> Translations { get; set; }
    }
}