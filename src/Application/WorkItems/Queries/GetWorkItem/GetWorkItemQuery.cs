using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Api.Application.Common.Exceptions;
using TaskManager.Api.Application.Common.Interfaces;
using TaskManager.Api.Application.WorkItems.Common;
using TaskManager.Api.Domain.Entities;

namespace TaskManager.Api.Application.WorkItems.Queries.GetWorkItem
{
    public class GetWorkItemQuery : IRequest<ShallowWorkItemDto>
    {
        public int Id { get; set; }
    }

    public class GetWorkItemQueryHandler : IRequestHandler<GetWorkItemQuery, ShallowWorkItemDto>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        public GetWorkItemQueryHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;

        }
        public Task<ShallowWorkItemDto> Handle(GetWorkItemQuery request, CancellationToken cancellationToken)
        {
            var item = _applicationDbContext.WorkItems
                .Include(workItem => workItem.ProgressItems)
                .Where(workItem => workItem.Id == request.Id)
                .FirstOrDefault() ?? throw new NotFoundException(nameof(WorkItem), request.Id);
            return Task.FromResult(_mapper.Map<ShallowWorkItemDto>(item));
        }
    }
}
