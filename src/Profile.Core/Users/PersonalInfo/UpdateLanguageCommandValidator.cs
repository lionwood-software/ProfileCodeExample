using FluentValidation;

namespace Profile.Core.Users.PersonalInfo
{
    public class UpdateLanguageCommandValidator : AbstractValidator<UpdateLanguageCommand>
    {
        public UpdateLanguageCommandValidator()
        {
            RuleFor(x => x.LanguageCode).NotEmpty().MaximumLength(2);
        }
    }
}
