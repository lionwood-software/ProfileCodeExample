using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Profile.Core.EventBus;
using Profile.Core.FileManager;
using Profile.Core.SharedKernel;

namespace Profile.Core.Users.PersonalInfo
{
    public class PersonalInfoUpdateHandler : IRequestHandler<UpdatePhoneNumberCommand, Unit>,
        IRequestHandler<UpdateUserLanguageProficienciesCommand, Unit>,
        IRequestHandler<UpdateLanguageCommand, string>,
        IRequestHandler<UpdateAvatarCommand, Unit>,
        IRequestHandler<UpdateFullNameCommand, Unit>
    {
        private readonly ProfileDbContext dbContext;
        private readonly IProfileUserContext userContext;
        private readonly IEventBusPort eventBusPort;
        private readonly IAvatarStorage avatarStorage;

        public PersonalInfoUpdateHandler(ProfileDbContext dbContext, IProfileUserContext userContext, IEventBusPort eventBusPort, IAvatarStorage avatarStorage)
        {
            this.dbContext = dbContext;
            this.userContext = userContext;
            this.eventBusPort = eventBusPort;
            this.avatarStorage = avatarStorage;
        }

        public async Task<Unit> Handle(UpdatePhoneNumberCommand request, CancellationToken cancellationToken)
        {
            var user = await dbContext.Users.FindAsync(userContext.UserId);

            user.PhoneNumber = request.PhoneNumber;

            await dbContext.SaveChangesAsync(cancellationToken);

            await eventBusPort.Publish(user.Id, ProfileEventType.PhoneNumberUpdated);

            return Unit.Value;
        }

        public async Task<Unit> Handle(UpdateUserLanguageProficienciesCommand request, CancellationToken cancellationToken)
        {
            var newProficiencies = request.LanguageProficiencies
                .Select(x => new UserLanguageProficiency { UserId = userContext.UserId, LanguageCode = x.LanguageCode, ProficiencyId = x.ProficiencyLevel })
                .ToList();

            var currentProficiencies = await dbContext.UserLanguageProficiencies
                .Where(x => x.UserId == userContext.UserId)
                .ToListAsync(cancellationToken);

            UpdateExistingUserLanguageProficiencies(currentProficiencies, newProficiencies);
            DeleteOutdatedUserLanguageProficiencies(currentProficiencies, newProficiencies);
            AddNewUserLanguageProficiencies(currentProficiencies, newProficiencies);

            await dbContext.SaveChangesAsync(cancellationToken);

            await eventBusPort.Publish(userContext.UserId, ProfileEventType.LanguageProficienciesUpdated);

            return Unit.Value;
        }

        public async Task<string> Handle(UpdateLanguageCommand request, CancellationToken cancellationToken)
        {
            var user = await dbContext.Users.FindAsync(userContext.UserId);

            user.LanguageCode = request.LanguageCode;
            userContext.SetCulture(request.LanguageCode);

            await dbContext.SaveChangesAsync(cancellationToken);

            await eventBusPort.Publish(user.Id, ProfileEventType.LanguageUpdated);

            return request.LanguageCode;
        }

        public async Task<Unit> Handle(UpdateAvatarCommand request, CancellationToken cancellationToken)
        {
            var user = await dbContext.Users.FindAsync(userContext.UserId);

            if (!string.IsNullOrEmpty(user.AvatarUrl))
            {
                var oldFileName = Path.GetFileName(user.AvatarUrl).ToLowerInvariant();
                await avatarStorage.Delete(oldFileName);
            }

            var fileExt = Path.GetExtension(request.FileName).ToLowerInvariant();
            user.AvatarUrl = await avatarStorage.Upload(Convert.FromBase64String(request.Content), $"{userContext.UserId}_{DateTime.Now.Ticks}{fileExt}");
            await dbContext.SaveChangesAsync(cancellationToken);

            await eventBusPort.Publish(user.Id, ProfileEventType.AvatarUpdated);

            return Unit.Value;
        }

        public async Task<Unit> Handle(UpdateFullNameCommand request, CancellationToken cancellationToken)
        {
            var user = await dbContext.Users.FindAsync(userContext.UserId);

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;

            await dbContext.SaveChangesAsync(cancellationToken);

            await eventBusPort.Publish(user.Id, ProfileEventType.FullNameUpdated);

            return Unit.Value;
        }

        private void UpdateExistingUserLanguageProficiencies(IEnumerable<UserLanguageProficiency> currentProficiencies, IEnumerable<UserLanguageProficiency> newProficiencies)
        {
            var shouldBeUpdated = newProficiencies.IntersectBy(currentProficiencies.Select(x => x.LanguageCode), x => x.LanguageCode).ToList();
            foreach (var newProficiency in shouldBeUpdated)
            {
                var current = currentProficiencies.Single(c => c.LanguageCode == newProficiency.LanguageCode);
                current.ProficiencyId = newProficiency.ProficiencyId;
            }
        }

        private void DeleteOutdatedUserLanguageProficiencies(IEnumerable<UserLanguageProficiency> currentProficiencies, IEnumerable<UserLanguageProficiency> newProficiencies)
        {
            var shouldBeDeleted = currentProficiencies.ExceptBy(newProficiencies.Select(x => x.LanguageCode), x => x.LanguageCode).ToList();
            dbContext.UserLanguageProficiencies.RemoveRange(shouldBeDeleted);
        }

        private void AddNewUserLanguageProficiencies(IEnumerable<UserLanguageProficiency> currentProficiencies, IEnumerable<UserLanguageProficiency> newProficiencies)
        {
            var shouldBeAdded = newProficiencies.ExceptBy(currentProficiencies.Select(x => x.LanguageCode), x => x.LanguageCode).ToList();
            dbContext.UserLanguageProficiencies.AddRange(shouldBeAdded);
        }
    }
}
