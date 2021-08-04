using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskManager.Api.Application.ProgressItems.Commands.AddProgressItemToWorkItem;
using TaskManager.Api.Application.ProgressItems.Commands.UpdateProgressItem;
using TaskManager.Api.Application.WorkItems.Commands.CreateWorkItem;
using TaskManager.Api.Application.WorkItems.Commands.DeleteWorkItem;
using TaskManager.Api.Application.WorkItems.Commands.UpdateWorkItem;
using TaskManager.Api.Application.WorkItems.Common;

namespace WebApi.Controllers
{
    public class ProgressItemController : ApiControllerBase
    {
        /// <summary>
        /// Adds a given progress item to an already existing work item
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] AddProgressItemToWorkItemCommand request) => await Mediator.Send(request);

        /// <summary>
        /// Allows you to update the complete status of a task. You may not edit the name after creation.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] UpdateProgressItemCommand request)
        {
            await Mediator.Send(request);
            return NoContent();
        }

        /// <summary>
        /// Deletes a given progress item from it's linked work item
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
