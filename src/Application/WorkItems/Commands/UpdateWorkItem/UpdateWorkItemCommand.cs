using AutoMapper;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Api.Application.Common.Exceptions;
using TaskManager.Api.Application.Common.Interfaces;
using TaskManager.Api.Application.WorkItems.Queries.Common;
using TaskManager.Api.Domain.Entities;

namespace TaskManager.Api.Application.WorkItems.Commands.UpdateWorkItem
{
    public class UpdateWorkItemCommand : IRequest<ShallowWorkItemDto>
    {
        public ShallowWorkItemDto Updated { get; set; }
    }

    public class UpdateWorkItemCommandHandler : IRequestHandler<UpdateWorkItemCommand, ShallowWorkItemDto>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        public UpdateWorkItemCommandHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<ShallowWorkItemDto> Handle(UpdateWorkItemCommand request, CancellationToken cancellationToken)
        {
            var workItem = _applicationDbContext.WorkItems
                .Where(workItem => workItem.Id == request.Updated.Id)
                .FirstOrDefault() ?? throw new NotFoundException(nameof(WorkItem), request.Updated.Id);
            workItem.Name = request.Updated.Name;
            workItem.Description = request.Updated.Description;
            workItem.AssignedTo = _applicationDbContext.TeamMembers
                .Where(teamMember => teamMember.Id == request.Updated.AssignedTo)
                .FirstOrDefault() ?? throw new NotFoundException(nameof(TeamMember), request.Updated.Id);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return _mapper.Map<ShallowWorkItemDto>(workItem);
        }
    }
}
