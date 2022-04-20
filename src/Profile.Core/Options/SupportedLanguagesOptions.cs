using System.Collections.Generic;

namespace Profile.Core.Options
{
    public class SupportedLanguagesOptions
    {
        public const string SectionName = "SupportedLanguages";

        public List<string> SupportedLanguages { get; set; }
    }
}
