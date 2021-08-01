using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Api.Application.Common.Exceptions;
using TaskManager.Api.Application.Common.Interfaces;
using TaskManager.Api.Application.WorkItems.Common;
using TaskManager.Api.Application.WorkItems.Queries.GetWorkItem;
using TaskManager.Api.Domain.Entities;

namespace TaskManager.Api.Application.WorkItems.Queries.GetAllWorkItems
{
    public class GetAllWorkItemsQuery : IRequest<List<ShallowWorkItemDto>>
    {
    }

    public class GetAllWorkItemsQueryHandler : IRequestHandler<GetAllWorkItemsQuery, List<ShallowWorkItemDto>>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        public GetAllWorkItemsQueryHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;

        }
        public Task<List<ShallowWorkItemDto>> Handle(GetAllWorkItemsQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(
                _applicationDbContext
                    .WorkItems
                    .AsQueryable()
                    .ProjectTo<ShallowWorkItemDto>(_mapper.ConfigurationProvider)
                    .ToList());
        }
    }
}
