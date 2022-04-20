using FluentValidation;

namespace Profile.Core.Users.PersonalInfo
{
    public class UpdateFullNameCommandValidator : AbstractValidator<UpdateFullNameCommand>
    {
        public UpdateFullNameCommandValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(32);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(32);
        }
    }
}
