using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Profile.Core.Languages.GetLanguages;
using Profile.Core.Languages.GetSupportedLanguages;
using Profile.Core.Proficiencies.GetProficiency;
using Profile.Core.SharedKernel;
using Profile.WebApi.Security;

namespace Profile.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Policy = IndividualAuthPolicy.Individual)]
    public class LanguagesController : ControllerBase
    {
        private readonly IMediator mediator;
        public LanguagesController(IMediator mediator) => this.mediator = mediator;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IEnumerable<Language>> GetLanguages() => await mediator.Send(new GetLanguagesQuery());

        [HttpGet]
        [Route("Supported")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<List<SupportedLanguage>> GetSupportedLanguages() => await mediator.Send(new GetSupportedLanguagesQuery());

        [HttpGet]
        [Route("Proficiencies")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IEnumerable<ListItem>> GetProficiencies() => await mediator.Send(new GetProficienciesQuery());
    }
}
