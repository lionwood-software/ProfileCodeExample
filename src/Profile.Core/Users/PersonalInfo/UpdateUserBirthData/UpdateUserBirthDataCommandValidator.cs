using System;
using FluentValidation;

namespace Profile.Core.Users.PersonalInfo.UpdateUserBirthData
{
    public class UpdateUserBirthDataCommandValidator : AbstractValidator<UpdateUserBirthDataCommand>
    {
        public UpdateUserBirthDataCommandValidator()
        {
            RuleFor(x => x.BirthDate).NotEmpty()
                .InclusiveBetween(DateTime.UtcNow.AddYears(-110).Date, DateTime.UtcNow.AddYears(-16).Date);
            RuleFor(x => x.CountryId).NotEmpty();
            RuleFor(x => x.Place).NotEmpty();
        }
    }
}
