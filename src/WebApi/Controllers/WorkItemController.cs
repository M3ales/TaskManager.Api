using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Api.Application.WorkItems.Commands.CreateWorkItem;
using TaskManager.Api.Application.WorkItems.Commands.DeleteWorkItem;
using TaskManager.Api.Application.WorkItems.Commands.UpdateWorkItem;
using TaskManager.Api.Application.WorkItems.Queries.Common;
using TaskManager.Api.Application.WorkItems.Queries.GetAllWorkItems;
using TaskManager.Api.Application.WorkItems.Queries.GetWorkItem;

namespace WebApi.Controllers
{
    public class WorkItemController : ApiControllerBase
    {
        [HttpGet("all")]
        public async Task<ActionResult<List<ShallowWorkItemDto>>> Get() => await Mediator.Send(new GetAllWorkItemsQuery());

        [HttpGet("{id}")]
        public async Task<ActionResult<ShallowWorkItemDto>> Get([FromRoute] GetWorkItemQuery request) => await Mediator.Send(request);

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateWorkItemCommand request) => await Mediator.Send(request);

        [HttpPut]
        public async Task<ActionResult<ShallowWorkItemDto>> Update([FromBody] UpdateWorkItemCommand request) => await Mediator.Send(request);

        [HttpPut("{id}")]
        public async Task<ActionResult<ShallowWorkItemDto>> Update([FromRoute]int id, [FromBody] UpdateWorkItemCommand request)
        {
            if (id != request?.Updated?.Id)
            {
                return BadRequest();
            }
            return await Mediator.Send(request);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete([FromBody] DeleteWorkItemCommand request)
        {
            await Mediator.Send(request);
            return NoContent();
        }
    }
}