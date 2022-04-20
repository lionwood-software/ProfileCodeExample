using System.Collections.Generic;
using Profile.Core.SharedKernel;

namespace Profile.Core.Languages
{
    public class Language : IHasTranslation<LanguageTranslation>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public ICollection<LanguageTranslation> Translations { get; set; }
    }
}
