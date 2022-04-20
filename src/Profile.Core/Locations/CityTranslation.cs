using Profile.Core.SharedKernel;

namespace Profile.Core.Locations
{
    public class CityTranslation : IEntityTranslation
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public string Culture { get; set; }
        public string Name { get; set; }
    }
}