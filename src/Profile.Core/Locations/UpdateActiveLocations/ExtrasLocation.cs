namespace Profile.Core.Locations.UpdateActiveLocations
{
    public record ExtrasLocation
    {
        public int CityId { get; init; }
        public bool IsSelected { get; init; }
    }
}
