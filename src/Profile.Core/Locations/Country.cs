using System.Collections.Generic;
using Profile.Core.SharedKernel;

namespace Profile.Core.Locations
{
    public class Country : IHasTranslation<CountryTranslation>
    {
        public int Id { get; set; }

        /// <summary>
        /// The ISO 3166 official short names in English
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The ISO 3166-1 alpha-2 two-letter country codes
        /// </summary>
        public string Alpha2 { get; set; }

        /// <summary>
        /// The ISO 3166-1 alpha-3 three-letter country codes2
        /// </summary>
        public string Alpha3 { get; set; }

        public ICollection<CountryTranslation> Translations { get; set; }
    }
}