using FluentValidation;

namespace Profile.Core.Users.PersonalInfo
{
    public class UpdatePhoneNumberCommandValidator : AbstractValidator<UpdatePhoneNumberCommand>
    {
        public UpdatePhoneNumberCommandValidator()
        {
            RuleFor(x => x.PhoneNumber).MaximumLength(50);
        }
    }
}
