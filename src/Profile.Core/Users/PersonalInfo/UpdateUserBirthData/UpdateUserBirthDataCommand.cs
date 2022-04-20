using System;
using MediatR;

namespace Profile.Core.Users.PersonalInfo.UpdateUserBirthData
{
    public class UpdateUserBirthDataCommand : IRequest
    {
        public DateTime BirthDate { get; set; }
        public int CountryId { get; set; }
        public int? RegionId { get; set; }
        public string Place { get; set; }
    }
}
