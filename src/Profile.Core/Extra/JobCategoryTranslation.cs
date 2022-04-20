using Profile.Core.SharedKernel;

namespace Profile.Core.Extra
{
    public class JobCategoryTranslation : IEntityTranslation
    {
        public int Id { get; set; }
        public int JobCategoryId { get; set; }
        public string Culture { get; set; }
        public string Name { get; set; }
    }
}
