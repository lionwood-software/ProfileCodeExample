using System.Collections.Generic;
using Profile.Core.SharedKernel;

namespace Profile.Core.Extra
{
    public class JobCategory : IHasTranslation<JobCategoryTranslation>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<JobRole> JobRoles { get; set; }
        public ICollection<JobCategoryTranslation> Translations { get; set; }
    }
}