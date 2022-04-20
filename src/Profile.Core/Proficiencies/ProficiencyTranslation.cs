using Profile.Core.SharedKernel;

namespace Profile.Core.Proficiencies
{
    public class ProficiencyTranslation : IEntityTranslation
    {
        public int Id { get; set; }
        public int ProficiencyId { get; set; }
        public string Culture { get; set; }
        public string Name { get; set; }
    }
}
