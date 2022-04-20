using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Profile.Adapters;
using Profile.Core.EventBus;
using Profile.Core.Extra.UpdateUserPreferences;
using Profile.Core.Languages;
using Profile.Core.Languages.GetSupportedLanguages;
using Profile.Core.Locations.GetActiveLocations;
using Profile.Core.Locations.UpdateActiveLocations;
using Profile.Core.Users;
using Profile.Core.Users.GetUserInfo;
using Profile.Core.Users.PersonalInfo;
using Profile.Core.Users.PersonalInfo.Summary;
using Profile.Core.Users.PersonalInfo.UpdateNir;
using Profile.Core.Users.PersonalInfo.UpdateUserBirthData;
using Profile.Core.Users.PersonalInfo.UpdateUserCountryWorkPermit;
using Profile.Infrastructure.Extensions;
using Profile.WebApi.Tests.Base;
using Xunit;
using Xunit.Abstractions;

namespace Profile.WebApi.Tests
{
    public sealed class UserActionsIntegrationTests : IClassFixture<ProfileApiFactory<Startup>>, IDisposable
    {
        private readonly ProfileApiFactory<Startup> factory;
        private readonly HttpClient client;

        public UserActionsIntegrationTests(ProfileApiFactory<Startup> factory, ITestOutputHelper testOutput)
        {
            this.factory = factory;
            this.factory.Output = testOutput;
            client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", factory.BuildClientToServerToken());
        }

        public void Dispose()
        {
            factory.ResetMocks();
            factory.CleanDatabase();
        }

        [Fact]
        public async Task UpdateLanguage_ShouldReturnSuccess()
        {
            var updateLanguageCommand = new UpdateLanguageCommand { LanguageCode = "fr" };

            var response = await client.PostAsync("/Users/Language", updateLanguageCommand);
            var (userInfoResponse, user) = await GetUserInfo();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            userInfoResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            user.LanguageCode.Should().Be(updateLanguageCommand.LanguageCode);
            factory.ServiceBusMock.Verify(x => x.Publish(user.Id, ProfileEventType.LanguageUpdated));
        }

        [Fact]
        public async Task GetUserPreferences_CheckInitialState_ShouldReturnOkAndAllCanDoAndWillDoFieldsShouldBeNull()
        {
            var (initialPreferencesResponse, initialPreferences) = await client.GetAsync("Extra/Preferences").TryDeserializeContent<List<UserPreference>>();
            var (userInfoResponse, user) = await GetUserInfo();

            initialPreferencesResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            initialPreferences.Should().NotBeNull();
            initialPreferences.Select(x => x.CanDo).All(x => x is null).Should().BeTrue();
            initialPreferences.Select(x => x.WillDo).All(x => x is null).Should().BeTrue();
        }

        [Fact]
        public async Task UpdateUserPreferences_ShouldReturnOk()
        {
            var command = new UpdateUserPreferencesCommand
            {
                Preferences = new List<UpdateUserPreferenceRecord> { new UpdateUserPreferenceRecord() { JobCategoryId = 1, WillDo = true, CanDo = true } }
            };

            var response = await client.PutAsync("Extra/Preferences", command);
            var (updatedUserPreferencesResponse, updatedUserPreferences) = await client.GetAsync("Extra/Preferences").TryDeserializeContent<List<UserPreference>>();
            var (userInfoResponse, user) = await GetUserInfo();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            updatedUserPreferencesResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            updatedUserPreferences.Should().NotBeNull();
            updatedUserPreferences.FirstOrDefault(x => x.JobCategoryId == 1).CanDo.Should().BeTrue();
            updatedUserPreferences.FirstOrDefault(x => x.JobCategoryId == 1).WillDo.Should().BeTrue();

            updatedUserPreferences.FirstOrDefault(x => x.JobCategoryId == 2).CanDo.Should().BeNull();
            updatedUserPreferences.FirstOrDefault(x => x.JobCategoryId == 2).WillDo.Should().BeNull();

            factory.ServiceBusMock.Verify(x => x.Publish(user.Id, ProfileEventType.JobPreferenceUpdated));
        }

        [Fact]
        public async Task UpdateUserPreferences_BadData_ShouldReturnInternalServerError()
        {
            var request = new[] { new { JobCategoryId = 1, WillDo = (bool?)null, CanDo = (bool?)null } };
            var response = await client.PutAsync("Extra/Preferences", request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdatePhoneNumber_CorrectData_ShouldReturnOkAndRaiseEvent()
        {
            var updatePhoneNumberCommand = new UpdatePhoneNumberCommand { PhoneNumber = "+38097777777777" };

            var response = await client.PostAsync("/Users/Phone", updatePhoneNumberCommand);
            var (userInfoResponse, user) = await GetUserInfo();
            var getNumberResponse = await client.GetAsync("/Users/Phone");
            var number = await getNumberResponse.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            number.Should().Be(updatePhoneNumberCommand.PhoneNumber);
            factory.ServiceBusMock.Verify(x => x.Publish(user.Id, ProfileEventType.PhoneNumberUpdated));
        }

        [Fact]
        public async Task UpdatePhoneNumber_BadData_ShouldReturnBadRequest()
        {
            var updatePhoneNumberCommand = new UpdatePhoneNumberCommand
            {
                PhoneNumber = "+380977777777777777777777777777777777777777777777777777777777777777777777777777777777777777777777777777777777777777777777"
            };

            var response = await client.PostAsync("/Users/Phone", updatePhoneNumberCommand);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdateUserLanguageProficiencies_CorrectData_ShouldReturnOkAndExpectedCount()
        {
            var updateUserLanguageProficienciesCommand = new UpdateUserLanguageProficienciesCommand
            {
                LanguageProficiencies = new List<LanguageProficiency> { new LanguageProficiency { LanguageCode = "eng", ProficiencyLevel = 2 } }
            };

            var response = await client.PostAsync("/Users/LanguageProficiencies", updateUserLanguageProficienciesCommand);
            var (languageProficienciesResponse, languageProficiencies) = await client.GetAsync("/Users/LanguageProficiencies").TryDeserializeContent<List<LanguageProficiency>>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            languageProficiencies.Count.Should().Be(1);
            languageProficiencies.FirstOrDefault().LanguageCode.Should().Be(updateUserLanguageProficienciesCommand.LanguageProficiencies.FirstOrDefault().LanguageCode);

            var (userInfoResponse, user) = await GetUserInfo();
            factory.ServiceBusMock.Verify(x => x.Publish(user.Id, ProfileEventType.LanguageProficienciesUpdated));
        }

        [Fact]
        public async Task UpdateUserLanguageProficiencies_SendEmptyArray_ShouldReturnOkAndEmptyList()
        {
            var updateUserLanguageProficienciesCommand = new UpdateUserLanguageProficienciesCommand
            {
                LanguageProficiencies = new List<LanguageProficiency>()
            };

            var response = await client.PostAsync("/Users/LanguageProficiencies", updateUserLanguageProficienciesCommand);
            var (languageProficienciesResponse, languageProficiencies) = await client.GetAsync("/Users/LanguageProficiencies").TryDeserializeContent<List<LanguageProficiency>>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            languageProficienciesResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            languageProficiencies.Should().BeEmpty();
        }

        [Fact]
        public async Task GetSupportedLanguages_EnCulture_ShouldReturnLanguagesFromConfiguration()
        {
            var (response, languages) = await client.GetAsync("/Languages/Supported").TryDeserializeContent<List<SupportedLanguage>>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            languages.Count.Should().Be(3);
            languages.Select(x => x.Name).Should().BeEquivalentTo(new List<string> { "English", "French", "German" });
        }

        [Fact]
        public async Task GetActiveLocations_CheckInitialState_AllIsSelectedValuesShouldBeFalse()
        {
            var (response, activeLocations) = await client.GetAsync("/Extra/Locations").TryDeserializeContent<List<ActiveLocation>>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            activeLocations.Count.Should().Be(14);
            activeLocations.All(x => x.IsSelected == false).Should().BeTrue();
        }

        [Fact]
        public async Task GetActiveLocations_CheckInitialState_CountriesShouldBeSorted()
        {
            var (response, activeLocations) = await client.GetAsync("/Extra/Locations").TryDeserializeContent<List<ActiveLocation>>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            activeLocations.Select(x => x.CountryOrder).Should().BeInAscendingOrder();
        }

        [Fact]
        public async Task GetActiveLocations_CheckInitialState_ShouldBeSortedCorrectly()
        {
            var (response, activeLocations) = await client.GetAsync("/Extra/Locations").TryDeserializeContent<List<ActiveLocation>>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            activeLocations.Count.Should().Be(14);
            activeLocations.Select(x => x.CountryName).Distinct().Should().BeEquivalentTo(new[] { "Switzerland", "France" }, options => options.WithStrictOrdering());
            activeLocations.GroupBy(x => x.CountryName).ElementAt(0).ToList().Should().BeInAscendingOrder(x => x.CityName);
            activeLocations.GroupBy(x => x.CountryName).ElementAt(1).ToList().Should().BeInAscendingOrder(x => x.CityName);
        }

        [Fact]
        public async Task UpdateActiveLocations_CorrectData_ShouldReturnOk()
        {
            var updateActiveLocationsCommand = new UpdateActiveLocationsCommand
            {
                Locations = new List<ExtrasLocation> { new ExtrasLocation { CityId = 1, IsSelected = true } }
            };

            var response = await client.PutAsync("/Extra/Locations", updateActiveLocationsCommand);
            var (activeLocationsResponse, activeLocations) = await client.GetAsync("/Extra/Locations").TryDeserializeContent<List<ActiveLocation>>();
            var (userInfoResponse, user) = await GetUserInfo();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            activeLocations.Count.Should().Be(14);
            activeLocations.FirstOrDefault(x => x.CityId == 1).IsSelected.Should().BeTrue();

            factory.ServiceBusMock.Verify(x => x.Publish(user.Id, ProfileEventType.CitiesUpdated));
        }

        [Fact]
        public async Task GetLanguages_ShouldReturnLanguagesDepandsOnUserLanguageCode()
        {
            var (response, languages) = await client.GetAsync("/Languages").TryDeserializeContent<List<Language>>();
            var langugeNames = languages.Select(x => x.Name).OrderBy(x => x).ToList();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            languages.Count.Should().Be(389);
            langugeNames.Contains("English").Should().BeTrue();
            langugeNames.Contains("French").Should().BeTrue();
            langugeNames.Contains("German").Should().BeTrue();
        }

        [Fact]
        public async Task GetLanguages_ShouldReturnLanguagesDepandsOnNewLanguageCodeAfterUpdate()
        {
            var newCulture = "de";

            var updateLanguageReponse = await UpdateUserLanguage(newCulture);
            var (userInfoResponse, user) = await GetUserInfo();
            var (getLanguagesResponse, languages) = await client.GetAsync("/Languages").TryDeserializeContent<List<Language>>();
            var langugeNames = languages.Select(x => x.Name).ToList();

            updateLanguageReponse.StatusCode.Should().Be(HttpStatusCode.OK);
            user.LanguageCode.Should().Be(newCulture);
            getLanguagesResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            languages.Count.Should().Be(389);
            langugeNames.Contains("Deutsch").Should().BeTrue();
            langugeNames.Contains("Englisch").Should().BeTrue();
            langugeNames.Contains("Finnisch").Should().BeTrue();
            langugeNames.Contains("Blackfoot-Sprache").Should().BeTrue();
        }

        [Fact]
        public async Task UploadAvatar_ShouldReturnBadRequest()
        {
            var updateAvatarCommand = new UpdateAvatarCommand()
            {
                Content = "",
                FileName = "avatar.txt"
            };

            var response = await UpdateUserAvatar(updateAvatarCommand);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UploadAvatar_ShouldUpdateUserAvatarUrl()
        {
            var avatarFile = "Files/avatar.jpg";
            var byteArrayFile = File.ReadAllBytes(avatarFile);
            var fileExt = Path.GetExtension(avatarFile).ToLowerInvariant();

            var (userInfoResponse, user) = await GetUserInfo();
            user.AvatarUrl.Should().BeNull();

            var returnUrl = $"https://test.net/people-avatar/{user.Id}_{DateTime.Now.Ticks}{fileExt}";
            factory.AvatarStorageMock
                 .Setup(m => m.Upload(byteArrayFile, It.IsAny<string>()).Result)
                 .Returns(returnUrl);

            var updateAvatarCommand = new UpdateAvatarCommand()
            {
                Content = Convert.ToBase64String(byteArrayFile),
                FileName = Path.GetFileName(avatarFile).ToLowerInvariant()
            };

            var response = await UpdateUserAvatar(updateAvatarCommand);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var (userInfoUpdateResponse, userUpdate) = await GetUserInfo();
            userUpdate.AvatarUrl.Should().Be(returnUrl);

            factory.ServiceBusMock.Verify(x => x.Publish(user.Id, ProfileEventType.AvatarUpdated));
        }

        [Fact]
        public async Task GetUserSummary_ShouldReturnUserProfileInformation()
        {
            var (response, userSummary) = await client.GetAsync("/Users/Summary").TryDeserializeContent<UserSummary>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            userSummary.Language.Should().Be("English");
            userSummary.FirstName.Should().Be("Extra");
            userSummary.LastName.Should().Be("User");
            userSummary.PhoneNumber.Should().Be("9379992");
        }

        [Fact]
        public async Task UpdateFullName_ShouldReturnOk()
        {
            var (userInfoResponse, user) = await GetUserInfo();

            var newFirstName = user.FirstName + "Update";
            var newLastName = user.LastName + "Update";

            var updateFullName = new UpdateFullNameCommand
            {
                FirstName = newFirstName,
                LastName = newLastName
            };

            var response = await client.PostAsync("/Users/FullName", updateFullName);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var (userInfoResponseUpdate, userUpdate) = await GetUserInfo();
            userUpdate.FirstName.Should().Be(newFirstName);
            userUpdate.LastName.Should().Be(newLastName);

            factory.ServiceBusMock.Verify(x => x.Publish(user.Id, ProfileEventType.FullNameUpdated));
        }

        [Fact]
        public async Task UpdateUserNir_ShouldReturnOk()
        {
            var (userInfoResponse, user) = await GetUserInfo();
            var updateUserNir = new UpdateNirCommand { Nir = "222222222222222" };

            var response = await client.PostAsync("/Users/Nir", updateUserNir);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var (responseSummary, userSummary) = await client.GetAsync("/Users/Summary").TryDeserializeContent<UserSummary>();
            userSummary.Nir.Should().Be(updateUserNir.Nir);

            factory.ServiceBusMock.Verify(x => x.Publish(user.Id, ProfileEventType.NirUpdated));
        }

        [Fact]
        public async Task UpdateUserNir_ShouldReturnBadRequest()
        {
            var updateUserNirWrongFirstNumber = new UpdateNirCommand { Nir = "422222222222222" };

            var response = await client.PostAsync("/Users/Nir", updateUserNirWrongFirstNumber);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var updateUserNirWrongLength = new UpdateNirCommand { Nir = "222222222222" };

            var response2 = await client.PostAsync("/Users/Nir", updateUserNirWrongLength);
            response2.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdateUserCountryWorkPermit_ShouldReturnOk()
        {
            var countryWorkPermit = new CountryWorkPermit { CountryId = 250, HasWorkPermit = true };
            var updateWorkPermit = new UpdateUserCountryWorkPermitCommand { CountryWorkPermits = new List<CountryWorkPermit> { countryWorkPermit } };

            var response = await client.PostAsync("/Users/WorkPermit", updateWorkPermit);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var (responseSummary, userSummary) = await client.GetAsync("/Users/Summary").TryDeserializeContent<UserSummary>();
            var userWorkPermit = userSummary.UserCountryWorkPermits.SingleOrDefault(x => x.CountryId == countryWorkPermit.CountryId);
            userWorkPermit.HasWorkPermit.Should().Be(countryWorkPermit.HasWorkPermit);
        }

        [Fact]
        public async Task UpdateUserCountryWorkPermit_ShouldReturnBadRequest()
        {
            var countryWorkPermit = new CountryWorkPermit { };
            var updateWorkPermit = new UpdateUserCountryWorkPermitCommand { CountryWorkPermits = new List<CountryWorkPermit> { countryWorkPermit } };

            var response = await client.PostAsync("/Users/WorkPermit", updateWorkPermit);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdateUserBirthData_ShouldReturnOk()
        {
            var (userInfoResponse, user) = await GetUserInfo();
            var userBirthDataRequest = new UpdateUserBirthDataCommand { BirthDate = DateTime.UtcNow.AddYears(-20).Date, CountryId = 250, RegionId = 1, Place = "test place" };

            var response = await client.PostAsync("/Users/BirthData", userBirthDataRequest);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var (responseData, userBirthDataResponse) = await client.GetAsync("/Users/BirthData").TryDeserializeContent<UserBirthData>();

            userBirthDataResponse.BirthDate.Should().Be(userBirthDataRequest.BirthDate);
            userBirthDataResponse.CountryId.Should().Be(userBirthDataRequest.CountryId);
            userBirthDataResponse.RegionId.Should().Be(userBirthDataRequest.RegionId);
            userBirthDataResponse.Place.Should().Be(userBirthDataRequest.Place);

            factory.ServiceBusMock.Verify(x => x.Publish(user.Id, ProfileEventType.BirthDataUpdated));
        }

        [Fact]
        public async Task DeleteUser_ShouldReturnOk()
        {
            var userId = ProfileApiFactory<Startup>.ExecutionUser.Id;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", factory.BuildServerToServerToken());
            var response = await client.DeleteAsync($"/Internal/users?UserId={userId}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        private async Task<(HttpResponseMessage responseData, UserInfo user)> GetUserInfo()
            => await client.GetAsync("/Users/Me").TryDeserializeContent<UserInfo>();

        private async Task<HttpResponseMessage> UpdateUserLanguage(string culture)
            => await client.PostAsync("/Users/Language", new UpdateLanguageCommand { LanguageCode = culture });

        private async Task<HttpResponseMessage> UpdateUserAvatar(UpdateAvatarCommand command)
            => await client.PostAsync("/Users/Avatar", command);
    }
}