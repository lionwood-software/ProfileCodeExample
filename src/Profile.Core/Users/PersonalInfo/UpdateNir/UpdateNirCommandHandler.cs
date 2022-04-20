using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Profile.Core.EventBus;
using Profile.Core.SharedKernel;

namespace Profile.Core.Users.PersonalInfo.UpdateNir
{
    public class UpdateNirCommandHandler : IRequestHandler<UpdateNirCommand, Unit>
    {
        private readonly ProfileDbContext dbContext;
        private readonly IProfileUserContext userContext;
        private readonly IEventBusPort eventBusPort;

        public UpdateNirCommandHandler(ProfileDbContext dbContext, IProfileUserContext userContext, IEventBusPort eventBusPort)
        {
            this.dbContext = dbContext;
            this.userContext = userContext;
            this.eventBusPort = eventBusPort;
        }

        public async Task<Unit> Handle(UpdateNirCommand request, CancellationToken cancellationToken)
        {
            var user = await dbContext.Users.FindAsync(userContext.UserId);

            user.Nir = request.Nir;

            await dbContext.SaveChangesAsync(cancellationToken);

            await eventBusPort.Publish(user.Id, ProfileEventType.NirUpdated);

            return Unit.Value;
        }
    }
}
