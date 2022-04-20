using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Profile.Core.Extra.GetUserExtraSummary;
using Profile.Core.Users.DeleteUser;
using Profile.Core.Users.GetNeoLmsUser;
using Profile.Infrastructure.Matching.Migration;

namespace Profile.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize(Policy = AuthPolicy.Maintenance)]
    public class InternalController : ControllerBase
    {
        private readonly IMediator mediator;
        public InternalController(IMediator mediator) => this.mediator = mediator;

        [HttpGet]
        [Route("/Internal/Extra/Summary")]
        //[Authorize(Policy = AuthPolicy.Maintenance)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<UserExtraSummary> GetInternalExtraSummary([Required] Guid userId) => await mediator.Send(new GetUserExtraSummaryQuery { UserId = userId });

        [HttpGet]
        [Route("/Internal/Profile/Neolms")]
        //[Authorize(Policy = AuthPolicy.Maintenance)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<NeoLmsUserInfo> GetInternalNeoLmsUser([Required] Guid userId) => await mediator.Send(new GetNeoLmsUserQuery { UserId = userId });

        [HttpPost]
        [Route("/Internal/Profile/SendProfiles")]
        //[Authorize(Policy = AuthPolicy.Maintenance)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task SendProfiles([FromServices] ProfilesMigrator profilesMigrator, CancellationToken cancellationToken) => await profilesMigrator.SendProfiles(cancellationToken);

        [HttpDelete]
        [Route("/Internal/users")]
        //[Authorize(Policy = AuthPolicy.Maintenance)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task DeleteUser([FromQuery] DeleteUserCommand command) => await mediator.Send(command);
    }
}