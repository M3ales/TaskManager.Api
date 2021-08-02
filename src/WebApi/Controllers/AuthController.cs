using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskManager.Api.Application.Authentication.Commands.Authenticate;

namespace WebApi.Controllers
{
    public class AuthController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<string>> Authenticate([FromBody]AuthenticateCommand request) => await Mediator.Send(request);
    }
}
