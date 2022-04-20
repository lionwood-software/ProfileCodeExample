using Profile.Core.SharedKernel;

namespace Profile.Core.Languages
{
    public class LanguageTranslation : IEntityTranslation
    {
        public int Id { get; set; }
        public string LanguageCode { get; set; }
        public string Culture { get; set; }
        public string Name { get; set; }
    }
}