using Profile.Core.SharedKernel;

namespace Profile.Core.Locations
{
    public class CountryTranslation : IEntityTranslation
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public string Culture { get; set; }
        public string Name { get; set; }
    }
}