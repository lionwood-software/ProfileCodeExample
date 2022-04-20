using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Profile.Core.Users.PersonalInfo.UpdateNeoLmsUser
{
    public class UpdateNeoUserCommandHandler : IRequestHandler<UpdateNeoIdCommand, Unit>
    {
        private readonly ProfileDbContext dbContext;

        public UpdateNeoUserCommandHandler(ProfileDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Unit> Handle(UpdateNeoIdCommand request, CancellationToken cancellationToken)
        {
            var user = await dbContext.Users.FindAsync(request.UserId);
            user.NeoId = request.NeoId;

            await dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
