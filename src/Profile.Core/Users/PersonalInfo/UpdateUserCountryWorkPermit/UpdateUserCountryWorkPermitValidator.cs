using FluentValidation;

namespace Profile.Core.Users.PersonalInfo.UpdateUserCountryWorkPermit
{
    public class UpdateUserCountryWorkPermitCommandValidator : AbstractValidator<UpdateUserCountryWorkPermitCommand>
    {
        public UpdateUserCountryWorkPermitCommandValidator()
        {
            RuleForEach(x => x.CountryWorkPermits).ChildRules(permit =>
            {
                permit.RuleFor(p => p.CountryId).NotEmpty();
            });
        }
    }
}
