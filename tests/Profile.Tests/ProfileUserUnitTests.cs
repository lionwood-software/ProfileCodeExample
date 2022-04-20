using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityFrameworkCore.Testing.Moq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Profile.Core;
using Profile.Core.EventBus;
using Profile.Core.Extra;
using Profile.Core.FileManager;
using Profile.Core.Locations;
using Profile.Core.SharedKernel;
using Profile.Core.Users;
using Profile.Core.Users.CreateUser;
using Profile.Core.Users.DeleteUser;
using Profile.Core.Users.PersonalInfo;
using Profile.Core.Users.PersonalInfo.UpdateNeoLmsUser;
using Profile.Core.Users.PersonalInfo.UpdateNir;
using Profile.Core.Users.PersonalInfo.UpdateUserBirthData;
using Profile.Core.Users.PersonalInfo.UpdateUserCountryWorkPermit;
using Profile.Tests.Base;
using Xunit;

namespace Profile.Tests
{
    public class ProfileUserUnitTests
    {
        private readonly ProfileDbContext dbContext;
        private readonly IProfileUserContext profileUserContext;
        private readonly IEventBusPort eventBusPort;
        private readonly IAvatarStorage avatarStorage;

        public ProfileUserUnitTests()
        {
            profileUserContext = new UserTestContext();
            eventBusPort = Mock.Of<IEventBusPort>();
            avatarStorage = Mock.Of<IAvatarStorage>();

            var dbContextOptions = new DbContextOptionsBuilder<ProfileDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .AddInterceptors(new AuditInterceptor(new UserTestContext()))
                .Options;

            dbContext = Create.MockedDbContextFor<ProfileDbContext>(dbContextOptions, profileUserContext);
        }

        [Fact]
        public async Task Create_User()
        {
            var mockedLogger = Mock.Of<ILogger<CreateUserHandler>>();
            var createUserHandler = new CreateUserHandler(dbContext, mockedLogger);
            var testUserId = Guid.NewGuid();
            var model = new CreateUserRecordCommand { UserId = testUserId, Email = "test@email.com", FirstName = "FirstName", LastName = "LastName", LanguageCode = "en" };

            await createUserHandler.Handle(model);

            var dbModel = dbContext.Users.Single(x => x.Id == testUserId);
            dbModel.Id.Should().Be(testUserId);
            dbModel.Email.Should().Be(model.Email);
            dbModel.FirstName.Should().Be(model.FirstName);
            dbModel.LastName.Should().Be(model.LastName);
            dbModel.LanguageCode.Should().Be(model.LanguageCode);
        }

        [Fact]
        public async Task UpdateFullName_User()
        {
            AddTestUserInDb();
            var updateFullNameUserHandler = new PersonalInfoUpdateHandler(dbContext, profileUserContext, eventBusPort, avatarStorage);
            var model = new UpdateFullNameCommand { FirstName = "FirstNameNew", LastName = "LastNameNew" };

            await updateFullNameUserHandler.Handle(model, CancellationToken.None);

            var dbModel = dbContext.Users.Single(x => x.Id == profileUserContext.UserId);
            dbModel.FirstName.Should().Be(model.FirstName);
            dbModel.LastName.Should().Be(model.LastName);
        }

        [Fact]
        public async Task UpdatePhoneNumber_User()
        {
            AddTestUserInDb();
            var updatePhoneNumberHandler = new PersonalInfoUpdateHandler(dbContext, profileUserContext, eventBusPort, avatarStorage);
            var model = new UpdatePhoneNumberCommand { PhoneNumber = "123456789" };

            await updatePhoneNumberHandler.Handle(model, CancellationToken.None);

            var dbModel = dbContext.Users.Single(x => x.Id == profileUserContext.UserId);
            dbModel.PhoneNumber.Should().Be(model.PhoneNumber);
        }

        [Fact]
        public async Task UpdateLanguage_User()
        {
            AddTestUserInDb();
            var updateLanguageHandler = new PersonalInfoUpdateHandler(dbContext, profileUserContext, eventBusPort, avatarStorage);
            var model = new UpdateLanguageCommand { LanguageCode = "fr" };

            await updateLanguageHandler.Handle(model, CancellationToken.None);

            var dbModel = dbContext.Users.Single(x => x.Id == profileUserContext.UserId);
            dbModel.LanguageCode.Should().Be(model.LanguageCode);
        }

        [Fact]
        public async Task UpdateUserLanguageProficiencies_User()
        {
            var updateFullNameUserHandler = new PersonalInfoUpdateHandler(dbContext, profileUserContext, eventBusPort, avatarStorage);
            var proficiencies = new List<LanguageProficiency>();
            var proficiency = new LanguageProficiency { LanguageCode = "en", ProficiencyLevel = 2 };
            proficiencies.Add(proficiency);
            var model = new UpdateUserLanguageProficienciesCommand { LanguageProficiencies = proficiencies };

            await updateFullNameUserHandler.Handle(model, CancellationToken.None);

            var proficiencyDb = dbContext.UserLanguageProficiencies.First(x => x.UserId == profileUserContext.UserId);
            proficiencyDb.ProficiencyId.Should().Be(proficiency.ProficiencyLevel);
            proficiencyDb.LanguageCode.Should().Be(proficiency.LanguageCode);
        }

        [Fact]
        public async Task Create_Update_UserBirthData()
        {
            FillInRegions();
            var updateBirthHandler = new UpdateUserBirthDataHandle(profileUserContext, dbContext, eventBusPort);
            var model = new UpdateUserBirthDataCommand { BirthDate = DateTime.Now, CountryId = 250, Place = "test place", RegionId = 5 };

            await updateBirthHandler.Handle(model, CancellationToken.None);

            var dbModel = dbContext.UserBirthData.Single(x => x.UserId == profileUserContext.UserId);
            dbModel.Id.Should().NotBe(0);
            dbModel.BirthDate.Should().Be(model.BirthDate);
            dbModel.CountryId.Should().Be(model.CountryId);
            dbModel.RegionId.Should().Be(model.RegionId);
            dbModel.Place.Should().Be(model.Place);
        }

        [Fact]
        public async Task Update_UserNir()
        {
            AddTestUserInDb();
            var dbModel = dbContext.Users.Single(x => x.Id == profileUserContext.UserId);
            dbModel.Nir.Should().BeNull();

            var updateUserNir = new UpdateNirCommandHandler(dbContext, profileUserContext, eventBusPort);
            var model = new UpdateNirCommand { Nir = "111111111111111" };
            await updateUserNir.Handle(model, CancellationToken.None);

            var dbUpdateModel = dbContext.Users.Single(x => x.Id == profileUserContext.UserId);
            dbUpdateModel.Nir.Should().Be(model.Nir);
        }


        [Fact]
        public async Task Update_UserCountryWorkPermit()
        {
            await FillInActiveCountry();

            var countryWorkPermit = new CountryWorkPermit { CountryId = 250, HasWorkPermit = true };
            var updateWorkPermit = new UpdateUserCountryWorkPermitCommand { CountryWorkPermits = new List<CountryWorkPermit> { countryWorkPermit } };

            var updateUserPerminHandler = new UpdateUserCountryWorkPermitHandler(profileUserContext, dbContext);
            await updateUserPerminHandler.Handle(updateWorkPermit, CancellationToken.None);

            var dbUpdateModel = await dbContext.UserCountryWorkPermits
                .SingleOrDefaultAsync(x => x.UserId == profileUserContext.UserId && x.CountryId == countryWorkPermit.CountryId);

            dbUpdateModel?.HasWorkPermit.Should().Be(countryWorkPermit.HasWorkPermit);
        }

        [Fact]
        public async Task Update_UserNeoId()
        {
            AddTestUserInDb();
            var dbUser = dbContext.Users.Single(x => x.Id == profileUserContext.UserId);
            dbUser.NeoId.Should().BeNull();

            var updateUserNeo = new UpdateNeoUserCommandHandler(dbContext);
            var model = new UpdateNeoIdCommand { UserId = profileUserContext.UserId, NeoId = 1 };
            await updateUserNeo.Handle(model, CancellationToken.None);

            var dbUpdateUser = dbContext.Users.Single(x => x.Id == profileUserContext.UserId);
            dbUpdateUser.NeoId.Should().Be(model.NeoId);
        }

        [Fact]
        public async Task Delete_User()
        {
            AddTestUserInDb();

            var mockedLogger = Mock.Of<ILogger<DeleteUserCommandHandler>>();
            var deleteUserHandler = new DeleteUserCommandHandler(dbContext, mockedLogger);
            await deleteUserHandler.Handle(new DeleteUserCommand { UserId = profileUserContext.UserId }, CancellationToken.None);

            var dbModel = dbContext.Users.SingleOrDefault(x => x.Id == profileUserContext.UserId);
            dbModel.Should().BeNull();
        }

        private void FillInRegions()
        {
            var region = new Region()
            {
                Id = 5,
                CountryId = 250,
                Name = "Hautes-Alpes",
                Code = "5"
            };

            dbContext.Regions.Add(region);
            dbContext.SaveChanges();
        }

        private async Task FillInActiveCountry()
        {
            var activeCountry1 = new ActiveCountry() { Id = 756, ItemOrder = 1 };
            await dbContext.ActiveCountries.AddAsync(activeCountry1);

            var activeCountry2 = new ActiveCountry() { Id = 250, ItemOrder = 2 };
            await dbContext.ActiveCountries.AddAsync(activeCountry2);

            await dbContext.SaveChangesAsync();
        }

        private void AddTestUserInDb()
        {
            var user = new User
            {
                Id = profileUserContext.UserId,
                Email = "test@email.com",
                FirstName = "FirstName",
                LastName = "LastName",
                LanguageCode = "en"
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();
        }
    }
}
