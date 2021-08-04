using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Api.Application.WorkItems.Commands.CreateWorkItem;
using TaskManager.Api.Application.WorkItems.Commands.DeleteWorkItem;
using TaskManager.Api.Application.WorkItems.Commands.UpdateWorkItem;
using TaskManager.Api.Application.WorkItems.Common;
using TaskManager.Api.Application.WorkItems.Queries.GetAllWorkItems;
using TaskManager.Api.Application.WorkItems.Queries.GetWorkItem;

namespace WebApi.Controllers
{
    public class WorkItemController : ApiControllerBase
    {
        /// <summary>
        /// Get a list of all work items
        /// </summary>
        /// <returns>A list of <see cref="ShallowWorkItemDto"/></returns>
        [HttpGet("all")]
        public async Task<ActionResult<List<ShallowWorkItemDto>>> Get() => await Mediator.Send(new GetAllWorkItemsQuery());

        /// <summary>
        /// Get a specific WorkItem
        /// </summary>
        /// <param name="request"></param>
        /// <returns>A <see cref="ShallowWorkItemDto"/> representation of a WorkItem</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ShallowWorkItemDto>> Get([FromRoute] GetWorkItemQuery request) => await Mediator.Send(request);

        /// <summary>
        /// Create a new Work Item, progress items must be added separately to this work item once created. The response will return the Id of the created work item.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The Id of the created WorkItem</returns>
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateWorkItemCommand request) => await Mediator.Send(request);

        /// <summary>
        /// Updates a given work item, and returns the updated item.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The updated item</returns>
        [HttpPut]
        public async Task<ActionResult<ShallowWorkItemDto>> Update([FromBody] UpdateWorkItemCommand request) => await Mediator.Send(request);
        
        /// <summary>
        /// Updates a given work item, and returns the updated item. Both provided Ids MUST match.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The updated item</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ShallowWorkItemDto>> Update([FromRoute]int id, [FromBody] UpdateWorkItemCommand request)
        {
            if (id != request?.Updated?.Id)
            {
                return BadRequest();
            }
            return await Mediator.Send(request);
        }

        /// <summary>
        /// Deletes a given work item.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult> Delete([FromBody] DeleteWorkItemCommand request)
        {
            await Mediator.Send(request);
            return NoContent();
        }
    }
}