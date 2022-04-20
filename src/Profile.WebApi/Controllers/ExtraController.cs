using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Profile.Core.Extra;
using Profile.Core.Extra.GetUserPreferences;
using Profile.Core.Extra.UpdateUserPreferences;
using Profile.Core.Locations.GetActiveLocations;
using Profile.Core.Locations.UpdateActiveLocations;
using Profile.WebApi.Security;

namespace Profile.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Policy = IndividualAuthPolicy.Individual)]
    public class ExtraController : ControllerBase
    {
        private readonly IMediator mediator;
        public ExtraController(IMediator mediator) => this.mediator = mediator;

        [HttpGet]
        [Route("Locations")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IEnumerable<ActiveLocation>> GetActiveLocations() => await mediator.Send(new GetActiveLocationsQuery());

        [HttpPut]
        [Route("Locations")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task UpdateActiveLocations([FromBody] UpdateActiveLocationsCommand command) => await mediator.Send(command);

        [HttpGet]
        [Route("Preferences")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IEnumerable<UserPreference>> GetUserPreferences() => await mediator.Send(new GetUserPreferencesQuery());

        [HttpPut]
        [Route("Preferences")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task UpdateUserPreferences([FromBody] UpdateUserPreferencesCommand command) => await mediator.Send(command);
    }
}
