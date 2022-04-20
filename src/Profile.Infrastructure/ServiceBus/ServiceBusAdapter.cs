using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Profile.Core;
using Profile.Core.EventBus;
using Profile.Infrastructure.ServiceBus.MatchedPayload;
using Profile.Infrastructure.ServiceBus.Options;

namespace Profile.Infrastructure.ServiceBus
{
    public class ServiceBusAdapter : IEventBusPort
    {
        private readonly ProfileDbContext dbContext;
        private readonly ILogger<ServiceBusAdapter> logger;
        private readonly IServiceBusClient serviceBusClient;
        private readonly ServiceBusOptions serviceBusOptions;

        private readonly List<ProfileEventType> summaryUpdateTypes = new()
        {
            ProfileEventType.LanguageUpdated,
            ProfileEventType.CitiesUpdated,
            ProfileEventType.LanguageProficienciesUpdated,
            ProfileEventType.JobPreferenceUpdated
        };

        private readonly List<ProfileEventType> profileUpdateTypes = new()
        {
            ProfileEventType.PhoneNumberUpdated,
            ProfileEventType.OnboardingCompleted,
            ProfileEventType.LanguageUpdated,
            ProfileEventType.AvatarUpdated,
            ProfileEventType.LanguageProficienciesUpdated,
            ProfileEventType.FullNameUpdated,
            ProfileEventType.NirUpdated,
            ProfileEventType.BirthDataUpdated
        };

        public ServiceBusAdapter(
            ProfileDbContext dbContext,
            ILogger<ServiceBusAdapter> logger,
            IServiceBusClient serviceBusClient,
            IOptions<ServiceBusOptions> serviceBusOptions)
        {
            this.logger = logger;
            this.dbContext = dbContext;
            this.serviceBusClient = serviceBusClient;
            this.serviceBusOptions = serviceBusOptions.Value;
        }

        public async Task Publish(Guid userId, ProfileEventType type)
        {
            if (profileUpdateTypes.Contains(type))
            {
                await PublishProfileUpdate(userId, type);
            }

            if (summaryUpdateTypes.Contains(type))
            {
                await PublishSummaryUpdate(userId, type);
            }

            if (type.Equals(ProfileEventType.SyncProfile))
            {
                await PublishProfileSyncUpdate(userId);
            }
        }

        private async Task PublishProfileUpdate(Guid userId, ProfileEventType type)
        {
            var payload = await dbContext.Users.Where(x => x.Id == userId)
                .Select(user => new UserPayload
                {
                    UserId = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    LanguageCode = user.LanguageCode,
                    PhoneNumber = user.PhoneNumber,
                    AvatarUrl = user.AvatarUrl,
                    Nir = user.Nir,
                    NeoId = user.NeoId,
                    LanguageProficiencies = dbContext.UserLanguageProficiencies
                        .Where(prof => prof.UserId == userId)
                        .Select(x => new LanguageProficiencyPayload
                        {
                            LanguageCode = x.LanguageCode,
                            ProficiencyId = x.ProficiencyId
                        }).ToList(),
                    BirthData = dbContext.UserBirthData
                        .Where(x => x.UserId == userId)
                        .Select(x => new BirthDataPayload
                        {
                            BirthDate = x.BirthDate,
                            CountryId = x.CountryId,
                            RegionId = x.RegionId,
                            Place = x.Place
                        })
                        .SingleOrDefault()
                })
                .SingleOrDefaultAsync();

            try
            {
                await serviceBusClient.SendMessage(payload, type.ToString(), serviceBusOptions.Topics.ProfileEvents);
            }
            catch (ServiceBusException ex)
            {
                logger.LogError(ex, "An error occurred while processing a message: {message}", JsonConvert.SerializeObject(payload));
                throw;
            }
        }

        private async Task PublishSummaryUpdate(Guid userId, ProfileEventType type)
        {
            var payload = await dbContext.Users.Where(x => x.Id == userId)
                .Select(user => new UserSummaryPayload
                {
                    UserId = user.Id,
                    LanguageCode = user.LanguageCode,
                    CityIds = dbContext.UserExtraCities
                        .Where(city => city.UserId == userId)
                        .Select(c => c.CityId)
                        .ToList(),
                    LanguageProficiencies = dbContext.UserLanguageProficiencies
                        .Where(prof => prof.UserId == userId)
                        .Select(x => new UserLanguageProficiencyPayload
                        {
                            Code = x.LanguageCode,
                            LevelId = x.ProficiencyId
                        })
                        .ToList(),
                    ExtraPreferences = dbContext.UserExtraPreferences
                        .Where(pref => pref.UserId == userId)
                        .Select(p => new UserPreferencePayload
                        {
                            CategoryId = p.JobCategoryId,
                            RolesIds = p.JobCategory.JobRoles.Select(r => r.Id),
                            CanDo = p.CanDo,
                            WillDo = p.WillDo
                        })
                        .ToList()
                })
                .SingleOrDefaultAsync();

            try
            {
                await serviceBusClient.SendMessage(payload, type.ToString(), serviceBusOptions.Topics.MatchingProfileUpdates);
            }
            catch (ServiceBusException ex)
            {
                logger.LogError(ex, "An error occurred while processing a message: {message}", JsonConvert.SerializeObject(payload));
                throw;
            }
        }

        private async Task PublishProfileSyncUpdate(Guid userId)
        {
            var payload = await dbContext.Users.Where(x => x.Id == userId)
                .Select(user => new UserSyncPayload()
                {
                    UserId = user.Id,
                    LanguageCode = user.LanguageCode,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    AvatarUrl = user.AvatarUrl,
                    Email = user.Email
                })
                .SingleOrDefaultAsync();
            try
            {
                await serviceBusClient.SendMessage(payload, ProfileEventType.SyncProfile.ToString(), serviceBusOptions.Topics.SyncProfiles);
                await serviceBusClient.SendMessage(payload, ProfileEventType.SyncProfile.ToString(), serviceBusOptions.Topics.ProfileEvents);
            }
            catch (ServiceBusException ex)
            {
                logger.LogError(ex, "An error occurred while processing a message: {message}", JsonConvert.SerializeObject(payload));
                throw;
            }
        }
    }
}