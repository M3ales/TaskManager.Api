using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Api.Application.Common.Exceptions;
using TaskManager.Api.Application.Common.Interfaces;
using TaskManager.Api.Application.WorkItems.Queries.Common;
using TaskManager.Api.Domain.Entities;

namespace TaskManager.Api.Application.WorkItems.Queries.GetWorkItem
{
    public class GetWorkItemQuery : IRequest<WorkItemDto>
    {
        public int Id { get; set; }
    }

    public class GetWorkItemQueryHandler : IRequestHandler<GetWorkItemQuery, WorkItemDto>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        public GetWorkItemQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public Task<WorkItemDto> Handle(GetWorkItemQuery request, CancellationToken cancellationToken)
        {
            var item = _applicationDbContext.WorkItems
                .Where(workItem => workItem.Id == request.Id)
                .FirstOrDefault() ?? throw new NotFoundException(nameof(WorkItem), request.Id);

            return Task.FromResult(new WorkItemDto()
            {
                Id = item.Id,
                AssignedTo = item.AssignedTo?.Id,
                Description = item.Description,
                Name = item.Name
            });
        }
    }
}
