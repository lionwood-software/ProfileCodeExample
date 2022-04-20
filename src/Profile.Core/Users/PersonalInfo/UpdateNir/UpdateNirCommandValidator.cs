using FluentValidation;
using System.Linq;

namespace Profile.Core.Users.PersonalInfo.UpdateNir
{
    public class UpdateNirCommandValidator : AbstractValidator<UpdateNirCommand>
    {
        public UpdateNirCommandValidator()
        {
            RuleFor(x => x.Nir)
                .Must(x => x.All(char.IsDigit) && !x.All(char.IsSymbol))
                .Must(nir => nir.StartsWith("1") || nir.StartsWith("2"))
                .Length(15);
        }
    }
}
