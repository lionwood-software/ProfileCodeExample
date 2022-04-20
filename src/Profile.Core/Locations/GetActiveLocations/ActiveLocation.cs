namespace Profile.Core.Locations.GetActiveLocations
{
    public record ActiveLocation
    {
        public int CountryId { get; init; }
        public string CountryName { get; init; }
        public int CountryOrder { get; init; }
        public int CityId { get; init; }
        public string CityName { get; init; }
        public bool IsSelected { get; init; }
        public bool IsUpcoming { get; set; }
    }
}