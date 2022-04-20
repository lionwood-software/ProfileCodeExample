namespace Profile.Core.Locations
{
    public class Region
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public Country Country { get; set; }
    }
}
