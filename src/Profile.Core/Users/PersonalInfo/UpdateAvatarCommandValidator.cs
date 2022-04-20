using FluentValidation;

namespace Profile.Core.Users.PersonalInfo
{
    public class UpdateAvatarCommandValidator : AbstractValidator<UpdateAvatarCommand>
    {
        public UpdateAvatarCommandValidator()
        {
            RuleFor(x => x.Content).NotEmpty();
            RuleFor(x => x.FileName).NotEmpty();
        }
    }
}
