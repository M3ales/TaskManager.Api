using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Api.Application.WorkItems.Queries.Common;
using TaskManager.Api.Application.WorkItems.Queries.GetAllWorkItems;
using TaskManager.Api.Application.WorkItems.Queries.GetWorkItem;

namespace WebApi.Controllers
{
    public class WorkItemController : ApiControllerBase
    {
        [HttpGet("all")]
        public async Task<ActionResult<List<ShallowWorkItemDto>>> Get([FromQuery]GetAllWorkItemsQuery request) => await Mediator.Send(request);
        [HttpGet]
        public async Task<ActionResult<ShallowWorkItemDto>> Get([FromQuery] GetWorkItemQuery request) => await Mediator.Send(request);
    }
}
