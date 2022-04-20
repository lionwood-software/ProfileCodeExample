using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Profile.Core.Extensions;

namespace Profile.Core.Users.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly ProfileDbContext dbContext;
        private readonly ILogger<DeleteUserCommandHandler> logger;

        public DeleteUserCommandHandler(ProfileDbContext dbContext, ILogger<DeleteUserCommandHandler> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await dbContext.Users
                .FindAsync(request.UserId)
                .ThrowIfNotFound();

            var userBirthDataSummary = await dbContext.UserBirthData.SingleOrDefaultAsync(x => x.UserId == user.Id);
            var userCities = dbContext.UserExtraCities.Where(city => city.UserId == user.Id);
            var userWorkPermits = dbContext.UserCountryWorkPermits.Where(x => x.UserId == user.Id);
            var preferences = dbContext.UserExtraPreferences.Where(preference => preference.UserId == user.Id);
            var languageProficiencies = dbContext.UserLanguageProficiencies.Where(proficiency => proficiency.UserId == user.Id);

            if (userBirthDataSummary != null)
            {
                dbContext.UserBirthData.Remove(userBirthDataSummary);
            }

            dbContext.UserExtraCities.RemoveRange(userCities);
            dbContext.UserCountryWorkPermits.RemoveRange(userWorkPermits);
            dbContext.UserExtraPreferences.RemoveRange(preferences);
            dbContext.UserLanguageProficiencies.RemoveRange(languageProficiencies);
            dbContext.Users.Remove(user);

            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("User data by id = {UserId} was deleted. ", request.UserId);

            return Unit.Value;
        }
    }
}
