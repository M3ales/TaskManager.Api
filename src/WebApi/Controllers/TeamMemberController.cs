using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Api.Application.TeamMembers.Commands.CreateTeamMemberCommand;
using TaskManager.Api.Application.TeamMembers.Commands.DeleteTeamMemberCommand;
using TaskManager.Api.Application.TeamMembers.Commands.UpdateTeamMemberCommand;
using TaskManager.Api.Application.TeamMembers.Common;
using TaskManager.Api.Application.TeamMembers.Queries.GetAllTeamMembersQuery;
using TaskManager.Api.Application.TeamMembers.Queries.GetTeamMemberQuery;
using TaskManager.Api.Application.WorkItems.Commands.CreateWorkItem;
using TaskManager.Api.Application.WorkItems.Commands.DeleteWorkItem;
using TaskManager.Api.Application.WorkItems.Commands.UpdateWorkItem;
using TaskManager.Api.Application.WorkItems.Common;
using TaskManager.Api.Application.WorkItems.Queries.GetWorkItem;

namespace WebApi.Controllers
{
    public class TeamMemberController : ApiControllerBase
    {
        [HttpGet("all")]
        public async Task<ActionResult<List<ShallowTeamMemberDto>>> Get() => await Mediator.Send(new GetAllTeamMembersQuery());

        [HttpGet("{id}")]
        public async Task<ActionResult<ShallowTeamMemberDto>> Get([FromRoute] GetTeamMemberQuery request) => await Mediator.Send(request);

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateTeamMemberCommand request) => await Mediator.Send(request);

        [HttpPut]
        public async Task<ActionResult<ShallowTeamMemberDto>> Update([FromBody] UpdateTeamMemberCommand request) => await Mediator.Send(request);

        [HttpDelete]
        public async Task<ActionResult> Delete([FromBody] DeleteTeamMemberCommand request)
        {
            await Mediator.Send(request);
            return NoContent();
        }
    }
}
