using FluentValidation;

namespace Profile.Core.Users.CompleteOnboarding
{
    public class CompleteOnboardingCommandValidator : AbstractValidator<CompleteOnboardingCommand>
    {
        public CompleteOnboardingCommandValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(32);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(32);
        }
    }
}
