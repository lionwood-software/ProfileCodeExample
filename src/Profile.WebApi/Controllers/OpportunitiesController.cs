using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Profile.Core.Matching.Extra;
using Profile.WebApi.Security;

namespace Profile.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Policy = IndividualAuthPolicy.Individual)]
    public class OpportunitiesController : ControllerBase
    {
        private readonly IMediator mediator;
        public OpportunitiesController(IMediator mediator) => this.mediator = mediator;

        [HttpGet]
        [Route("Featured")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<FeaturedOpportunitiesResult> GetFeatured() => await mediator.Send(new GetFeaturedOpportunitiesQuery());
    }
}