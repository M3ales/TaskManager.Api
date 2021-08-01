using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        [HttpGet]
        public async Task<ActionResult<ShallowWorkItemDto>> Get([FromQuery] GetWorkItemQuery request) => await Mediator.Send(request);

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateWorkItemCommand request) => await Mediator.Send(request);

        [HttpPut]
        public async Task<ActionResult<ShallowWorkItemDto>> Update([FromBody] UpdateWorkItemCommand request) => await Mediator.Send(request);

        [HttpDelete]
        public async Task<ActionResult> Delete([FromBody] DeleteWorkItemCommand request)
        {
            await Mediator.Send(request);
            return NoContent();
        }
    }
}