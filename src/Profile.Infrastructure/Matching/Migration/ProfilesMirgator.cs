using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Profile.Core;
using Profile.Core.EventBus;

namespace Profile.Infrastructure.Matching.Migration
{
    public class ProfilesMigrator
    {
        private readonly ProfileDbContext dbContext;
        private readonly ILogger<ProfilesMigrator> logger;
        private readonly IServiceScopeFactory serviceScopeFactory;

        public ProfilesMigrator(
            ProfileDbContext dbContext,
            ILogger<ProfilesMigrator> logger,
            IServiceScopeFactory serviceScopeFactory
            )
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public async Task SendProfiles(CancellationToken cancellationToken)
        {
            var userIds = await dbContext.Users.Select(u => u.Id).ToListAsync(cancellationToken);

            _ = Task.Run(async () =>
              {
                  logger.LogInformation("User profiles migration to ServiceBus for matching extras started.");

                  using var scope = serviceScopeFactory.CreateScope();
                  var eventBusPort = scope.ServiceProvider.GetRequiredService<IEventBusPort>();

                  foreach (var userId in userIds)
                  {
                      try
                      {
                          await eventBusPort.Publish(userId, ProfileEventType.JobPreferenceUpdated);
                          await eventBusPort.Publish(userId, ProfileEventType.SyncProfile);
                          logger.LogInformation("Sent user {userId} to ServiceBus.", userId);
                      }
                      catch (Exception e)
                      {
                          logger.LogWarning("Exception for sent user {userId} to ServiceBus. Exception: {message}", userId, e.ToString());
                      }
                  };

                  logger.LogInformation("User profiles migration to ServiceBus for matching extras ended.");
              },
              cancellationToken);
        }
    }
}
