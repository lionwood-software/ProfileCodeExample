using Profile.Core.SharedKernel;

namespace Profile.Core.Extra
{
    public class JobRoleTranslation : IEntityTranslation
    {
        public int Id { get; set; }
        public int JobRoleId { get; set; }
        public string Culture { get; set; }
        public string Name { get; set; }
    }
}
