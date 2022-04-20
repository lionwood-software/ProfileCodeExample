using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Profile.Core.Locations;
using Profile.Core.Locations.GetCountries;
using Profile.Core.Locations.GetRegions;
using Profile.Core.Options;
using Profile.Core.SharedKernel.Exceptions;
using Profile.Core.Users;
using Profile.Core.Users.CompleteOnboarding;
using Profile.Core.Users.GetUserInfo;
using Profile.Core.Users.PersonalInfo;
using Profile.Core.Users.PersonalInfo.GetUserBirthData;
using Profile.Core.Users.PersonalInfo.Summary;
using Profile.Core.Users.PersonalInfo.UpdateNir;
using Profile.Core.Users.PersonalInfo.UpdateUserBirthData;
using Profile.Core.Users.PersonalInfo.UpdateUserCountryWorkPermit;
using Profile.WebApi.Localization;
using Profile.WebApi.Security;

namespace Profile.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Policy = IndividualAuthPolicy.Individual)]
    public class UsersController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly AzureBlobOptions azureBlobOptions;

        public UsersController(IMediator mediator, IOptions<AzureBlobOptions> azureBlobOptions)
        {
            this.mediator = mediator;
            this.azureBlobOptions = azureBlobOptions.Value;
        }

        [HttpGet]
        [Route("Me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<UserInfo> GetUserInfo() => await mediator.Send(new GetUserInfoQuery());

        [HttpGet]
        [Route("Summary")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<UserSummary> GetSummary() => await mediator.Send(new GetUserSummaryQuery());

        [HttpPost]
        [Route("Onboarding")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task CompleteOnboarding([FromBody] CompleteOnboardingCommand command) => await mediator.Send(command);

        [HttpPost]
        [Route("Phone")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task UpdatePhoneNumber([FromBody] UpdatePhoneNumberCommand command) => await mediator.Send(command);

        [HttpGet]
        [Route("Phone")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<string> GetPhoneNumber() => await mediator.Send(new GetPhoneNumberQuery());

        [HttpGet]
        [Route("LanguageProficiencies")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IEnumerable<LanguageProficiency>> GetLanguageProficiencies() => await mediator.Send(new GetLanguageProficiencyQuery());

        [HttpPost]
        [Route("LanguageProficiencies")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task UpdateLanguageProficiencies([FromBody] UpdateUserLanguageProficienciesCommand command) => await mediator.Send(command);

        [HttpPost]
        [Route("Language")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task UpdateLanguage([FromBody] UpdateLanguageCommand command)
        {
            var culture = await mediator.Send(command);
            CultureInfo.CurrentUICulture = new CultureInfo(culture);
            CultureCookieSetter.SetCurrentUiCulture(HttpContext);
        }

        [HttpPost]
        [Route("Avatar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task UpdateAvatar(UpdateAvatarCommand command)
        {
            var fileExt = Path.GetExtension(command.FileName).ToLowerInvariant();
            var fileSizeMb = Convert.FromBase64String(command.Content).Length / (1024.0 * 1024);

            if (azureBlobOptions.AllowedExtensions.Contains(fileExt) && fileSizeMb < azureBlobOptions.MaxFileSize)
            {
                await mediator.Send(command);
            }
            else
            {
                throw new UpdateAvatarException($"An error occurred while uploading the file: name: {command.FileName}, size: {fileSizeMb} Mb");
            }
        }

        [HttpPost]
        [Route("FullName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task UpdateFullName([FromBody] UpdateFullNameCommand command) => await mediator.Send(command);

        [HttpGet]
        [Route("Countries")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IEnumerable<LocationItem>> GetCountries() => await mediator.Send(new GetCountriesQuery());

        [HttpGet]
        [Route("Regions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IEnumerable<LocationItem>> GetRegions([FromQuery] GetRegionsQuery query) => await mediator.Send(query);

        [HttpGet]
        [Route("BirthData")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<UserBirthData> GetBirthData() => await mediator.Send(new GetUserBirthDataQuery());

        [HttpPost]
        [Route("BirthData")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task UpdateBirthData([FromBody] UpdateUserBirthDataCommand command) => await mediator.Send(command);

        [HttpPost]
        [Route("Nir")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task UpdateNir([FromBody] UpdateNirCommand command) => await mediator.Send(command);

        [HttpPost]
        [Route("WorkPermit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task UpdateWorkPermit([FromBody] UpdateUserCountryWorkPermitCommand command) => await mediator.Send(command);
    }
}
