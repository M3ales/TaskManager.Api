using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskManager.Api.Application.Authentication.Commands.Authenticate;
using TaskManager.Api.Application.Authentication.Commands.Regenerate;

namespace WebApi.Controllers
{
    public class AuthController : ApiControllerBase
    {
        [HttpPost("token")]
        public async Task<ActionResult<string>> Authenticate([FromBody]AuthenticateCommand request) => await Mediator.Send(request);
        [HttpPost("regenerate")]
        public async Task<ActionResult<string>> Regenerate([FromBody] RegenerateCommand request) => await Mediator.Send(request);
    }
}
