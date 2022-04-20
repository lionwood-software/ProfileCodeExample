using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Profile.Core.EventBus;
using Profile.Core.SharedKernel;

namespace Profile.Core.Users.PersonalInfo.UpdateUserBirthData
{
    public class UpdateUserBirthDataHandle : IRequestHandler<UpdateUserBirthDataCommand, Unit>
    {
        private readonly ProfileDbContext dbContext;
        private readonly IProfileUserContext profileUserContext;
        private readonly IEventBusPort eventBusPort;

        public UpdateUserBirthDataHandle(IProfileUserContext profileUserContext, ProfileDbContext dbContext, IEventBusPort eventBusPort)
        {
            this.dbContext = dbContext;
            this.profileUserContext = profileUserContext;
            this.eventBusPort = eventBusPort;
        }

        public async Task<Unit> Handle(UpdateUserBirthDataCommand request, CancellationToken cancellationToken)
        {
            await RegionValidation(request);

            var birthData = await dbContext.UserBirthData.SingleOrDefaultAsync(x => x.UserId == profileUserContext.UserId, cancellationToken);

            if (birthData == null)
            {
                var data = new UserBirthData
                {
                    UserId = profileUserContext.UserId,
                    BirthDate = request.BirthDate,
                    CountryId = request.CountryId,
                    RegionId = request.RegionId,
                    Place = request.Place
                };

                await dbContext.UserBirthData.AddAsync(data);
            }
            else
            {
                birthData.BirthDate = request.BirthDate;
                birthData.CountryId = request.CountryId;
                birthData.RegionId = request.RegionId;
                birthData.Place = request.Place;

                dbContext.UserBirthData.Update(birthData);
            };

            await dbContext.SaveChangesAsync(cancellationToken);

            await eventBusPort.Publish(profileUserContext.UserId, ProfileEventType.BirthDataUpdated);

            return Unit.Value;
        }

        private async Task RegionValidation(UpdateUserBirthDataCommand request)
        {
            if (request.RegionId is null)
            {
                var countryRegions = dbContext.Regions.Where(x => x.CountryId == request.CountryId);
                if (countryRegions.Any())
                {
                    throw new ValidationException($"Region field shouldn't be null for Country {request.CountryId}.");
                }

                return;
            }

            var region = await dbContext.Regions.SingleAsync(x => x.Id == request.RegionId);

            if (region.CountryId != request.CountryId)
            {
                throw new ValidationException($"Region field hasn't {request.RegionId} correct value.");
            }
        }
    }
}
