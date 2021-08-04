using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskManager.Api.Application.Authentication.Commands.Authenticate;
using TaskManager.Api.Application.Authentication.Commands.Regenerate;

namespace WebApi.Controllers
{
    public class AuthController : ApiControllerBase
    {
        /// <summary>
        /// Exchange a refresh token for a Json Web Token which you can use to perform priveleged actions.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("token")]
        public async Task<ActionResult<string>> Authenticate([FromBody]AuthenticateCommand request) => await Mediator.Send(request);
        /// <summary>
        /// Generate a new refresh token for the specified account. For now there is only Account Id 1.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("regenerate")]
        public async Task<ActionResult<string>> Regenerate([FromBody] RegenerateCommand request) => await Mediator.Send(request);
    }
}
