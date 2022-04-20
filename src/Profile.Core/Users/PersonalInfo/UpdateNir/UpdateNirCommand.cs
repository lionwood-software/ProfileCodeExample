using MediatR;

namespace Profile.Core.Users.PersonalInfo.UpdateNir
{
    public class UpdateNirCommand : IRequest
    {
        public string Nir { get; set; }
    }
}
