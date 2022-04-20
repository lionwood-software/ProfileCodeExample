using FluentValidation;

namespace Profile.Core.Extra.UpdateUserPreferences
{
    public class UpdateUserPreferencesCommandValidator : AbstractValidator<UpdateUserPreferencesCommand>
    {
        public UpdateUserPreferencesCommandValidator()
        {
            RuleForEach(x => x.Preferences).ChildRules(child =>
            {
                child.RuleFor(preference => preference.CanDo).NotNull();
                child.RuleFor(preference => preference.WillDo).NotNull();
            });
        }
    }
}
