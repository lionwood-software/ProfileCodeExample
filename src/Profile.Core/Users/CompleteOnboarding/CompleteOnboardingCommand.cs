using MediatR;

namespace Profile.Core.Users.CompleteOnboarding
{
    public class CompleteOnboardingCommand : IRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
