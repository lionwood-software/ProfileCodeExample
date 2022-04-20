using System.Collections.Generic;
using Profile.Core.SharedKernel;

namespace Profile.Core.Locations
{
    public class City : IHasTranslation<CityTranslation>
    {
        public int Id { get; set; }
        public string GeonamesId { get; set; }
        public int CountryId { get; set; }
        public string Name { get; set; }
        public string AsciiName { get; set; }
        public string Timezone { get; set; }
        public string AlternateNames { get; set; }

        public Country Country { get; set; }
        public ICollection<CityTranslation> Translations { get; set; }
    }
}