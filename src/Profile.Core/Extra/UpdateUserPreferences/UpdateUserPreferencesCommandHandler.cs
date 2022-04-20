using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Profile.Core.EventBus;
using Profile.Core.Extra.User;
using Profile.Core.SharedKernel;

namespace Profile.Core.Extra.UpdateUserPreferences
{
    public class UpdateUserPreferencesCommandHandler : IRequestHandler<UpdateUserPreferencesCommand>
    {
        private readonly ProfileDbContext dbContext;
        private readonly IProfileUserContext profileUserContext;
        private readonly IEventBusPort eventBusPort;

        public UpdateUserPreferencesCommandHandler(ProfileDbContext dbContext, IProfileUserContext profileUserContext, IEventBusPort eventBusPort)
        {
            this.dbContext = dbContext;
            this.profileUserContext = profileUserContext;
            this.eventBusPort = eventBusPort;
        }

        public async Task<Unit> Handle(UpdateUserPreferencesCommand request, CancellationToken cancellationToken)
        {
            var newPreferences = request.Preferences
                .Select(x => new UserExtraPreference
                {
                    JobCategoryId = x.JobCategoryId,
                    CanDo = x.CanDo,
                    WillDo = x.WillDo,
                    UserId = profileUserContext.UserId,
                })
                .ToList();

            var currentPreferences = await dbContext.UserExtraPreferences
                .Where(x => x.UserId == profileUserContext.UserId)
                .ToListAsync(cancellationToken);

            UpdateExistingPreferences(currentPreferences, newPreferences);
            AddNewPreferences(currentPreferences, newPreferences);

            await dbContext.SaveChangesAsync(cancellationToken);

            await eventBusPort.Publish(profileUserContext.UserId, ProfileEventType.JobPreferenceUpdated);

            return Unit.Value;
        }

        private void UpdateExistingPreferences(IEnumerable<UserExtraPreference> currentPreferences, IEnumerable<UserExtraPreference> newPreferences)
        {
            var shouldBeUpdated = newPreferences.IntersectBy(currentPreferences.Select(x => x.JobCategoryId), x => x.JobCategoryId).ToList();
            foreach (var newPreference in shouldBeUpdated)
            {
                var current = currentPreferences.Single(c => c.JobCategoryId == newPreference.JobCategoryId);
                current.WillDo = newPreference.WillDo;
                current.CanDo = newPreference.CanDo;
            }
        }

        private void AddNewPreferences(IEnumerable<UserExtraPreference> currentPreferences, IEnumerable<UserExtraPreference> newPreferences)
        {
            var shouldBeAdded = newPreferences.ExceptBy(currentPreferences.Select(x => x.JobCategoryId), x => x.JobCategoryId).ToList();
            dbContext.UserExtraPreferences.AddRange(shouldBeAdded);
        }
    }
}
