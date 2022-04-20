using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Profile.Core.SharedKernel;

namespace Profile.Core.Users.PersonalInfo.UpdateUserCountryWorkPermit
{
    public class UpdateUserCountryWorkPermitHandler : IRequestHandler<UpdateUserCountryWorkPermitCommand, Unit>
    {
        private readonly ProfileDbContext dbContext;
        private readonly IProfileUserContext profileUserContext;

        public UpdateUserCountryWorkPermitHandler(IProfileUserContext profileUserContext, ProfileDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.profileUserContext = profileUserContext;
        }

        public async Task<Unit> Handle(UpdateUserCountryWorkPermitCommand request, CancellationToken cancellationToken)
        {
            await CheckActiveCountry(request, cancellationToken);

            var dbPermits = await dbContext.UserCountryWorkPermits
                .Where(x => x.UserId == profileUserContext.UserId)
                .ToListAsync(cancellationToken);

            foreach (var requestPermit in request.CountryWorkPermits)
            {
                var dbPermit = dbPermits.SingleOrDefault(x => x.CountryId == requestPermit.CountryId);
                if (dbPermit == null)
                {
                    dbPermit = new UserCountryWorkPermit
                    {
                        CountryId = requestPermit.CountryId,
                        UserId = profileUserContext.UserId,
                        HasWorkPermit = requestPermit.HasWorkPermit
                    };
                    await dbContext.UserCountryWorkPermits.AddAsync(dbPermit);
                }
                else
                {
                    dbPermit.HasWorkPermit = requestPermit.HasWorkPermit;
                    dbContext.Update(dbPermit);
                }
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private async Task CheckActiveCountry(UpdateUserCountryWorkPermitCommand request, CancellationToken cancellationToken)
        {
            var activeCountryIds = await dbContext.ActiveCountries.Select(x => x.Id).ToListAsync(cancellationToken);

            foreach (var countryId in request.CountryWorkPermits.Select(x => x.CountryId))
            {
                if (!activeCountryIds.Contains(countryId))
                {
                    throw new ValidationException($"Country {countryId} isn't active.");
                }
            }
        }
    }
}
