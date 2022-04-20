using Profile.Core.Locations;
using Profile.Core.SharedKernel;

namespace Profile.Core.Extra
{
    public class ActiveCountry : IOrderable
    {
        public int Id { get; set; }
        public int ItemOrder { get; set; }

        public Country Country { get; set; }
    }
}