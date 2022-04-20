using System.Collections.Generic;
using Profile.Core.SharedKernel;

namespace Profile.Core.Proficiencies
{
    public class Proficiency : IHasTranslation<ProficiencyTranslation>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ProficiencyTranslation> Translations { get; set; }
    }
}
