using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Profile.Core.EventBus;
using Profile.Core.SharedKernel;

namespace Profile.Core.Users.CompleteOnboarding
{
    public class CompleteOnboardingCommandHandler : IRequestHandler<CompleteOnboardingCommand>
    {
        private readonly ProfileDbContext dbContext;
        private readonly IProfileUserContext profileUserContext;
        private readonly IEventBusPort eventBusPort;

        public CompleteOnboardingCommandHandler(ProfileDbContext dbContext, IProfileUserContext profileUserContext, IEventBusPort eventBusPort)
        {
            this.dbContext = dbContext;
            this.profileUserContext = profileUserContext;
            this.eventBusPort = eventBusPort;
        }

        public async Task<Unit> Handle(CompleteOnboardingCommand request, CancellationToken cancellationToken)
        {
            var user = await dbContext.Users.FindAsync(profileUserContext.UserId);

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.IsOnboarded = true;

            await dbContext.SaveChangesAsync(cancellationToken);

            await eventBusPort.Publish(user.Id, ProfileEventType.OnboardingCompleted);

            return Unit.Value;
        }
    }
}
