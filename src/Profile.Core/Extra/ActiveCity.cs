using Profile.Core.Locations;

namespace Profile.Core.Extra
{
    /// <summary>
    /// Represents active cities which is managed by app admin
    /// </summary>
    public class ActiveCity
    {
        public int Id { get; set; }
        public bool IsUpcoming { get; set; }

        public City City { get; set; }
    }
}