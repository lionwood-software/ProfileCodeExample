using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Profile.Core;
using Profile.Core.EventBus;
using Profile.Core.Extra.User;
using Profile.Core.SharedKernel;

namespace Profile.Core.Locations.UpdateActiveLocations
{
    public class UpdateActiveLocationsCommandHandler : IRequestHandler<UpdateActiveLocationsCommand>
    {
        private readonly ProfileDbContext dbContext;
        private readonly IProfileUserContext profileUserContext;
        private readonly IEventBusPort eventBusPort;

        public UpdateActiveLocationsCommandHandler(ProfileDbContext dbContext, IProfileUserContext profileUserContext, IEventBusPort eventBusPort)
        {
            this.dbContext = dbContext;
            this.profileUserContext = profileUserContext;
            this.eventBusPort = eventBusPort;
        }

        public async Task<Unit> Handle(UpdateActiveLocationsCommand request, CancellationToken cancellationToken)
        {
            var newExtrasLocations = request.Locations
                .Where(x => x.IsSelected)
                .Select(x => new UserExtraCity { UserId = profileUserContext.UserId, CityId = x.CityId })
                .ToList();

            var currentExtrasLocations = await dbContext.UserExtraCities
                .Where(x => x.UserId == profileUserContext.UserId)
                .ToListAsync(cancellationToken);

            DeleteOutdatedExtrasLocations(currentExtrasLocations, newExtrasLocations);
            AddNewExtrasLocations(currentExtrasLocations, newExtrasLocations);

            await dbContext.SaveChangesAsync(cancellationToken);

            await eventBusPort.Publish(profileUserContext.UserId, ProfileEventType.CitiesUpdated);

            return Unit.Value;
        }

        private void DeleteOutdatedExtrasLocations(IEnumerable<UserExtraCity> currentExtrasLocations, IEnumerable<UserExtraCity> newExtrasLocations)
        {
            var shouldBeDeleted = currentExtrasLocations.ExceptBy(newExtrasLocations.Select(x => x.CityId), x => x.CityId).ToList();
            dbContext.UserExtraCities.RemoveRange(shouldBeDeleted);
        }

        private void AddNewExtrasLocations(IEnumerable<UserExtraCity> currentExtrasLocations, IEnumerable<UserExtraCity> newExtrasLocations)
        {
            var shouldBeAdded = newExtrasLocations.ExceptBy(currentExtrasLocations.Select(x => x.CityId), x => x.CityId).ToList();
            dbContext.UserExtraCities.AddRange(shouldBeAdded);
        }
    }
}
