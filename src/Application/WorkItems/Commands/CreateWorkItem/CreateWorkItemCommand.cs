using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Api.Application.Common.Exceptions;
using TaskManager.Api.Application.Common.Interfaces;
using TaskManager.Api.Domain.Entities;

namespace TaskManager.Api.Application.WorkItems.Commands.CreateWorkItem
{

    public class CreateWorkItemCommand : IRequest<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? AssignedTo { get; set; }
    }

    public class CreateWorkItemCommandHandler : IRequestHandler<CreateWorkItemCommand, int>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        public CreateWorkItemCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<int> Handle(CreateWorkItemCommand request, CancellationToken cancellationToken)
        {
            TeamMember assignedTo = null;
            if (request.AssignedTo != null)
            {
                assignedTo = _applicationDbContext.TeamMembers
                    .Where(teamMember => teamMember.Id == request.AssignedTo)
                    .FirstOrDefault() ?? throw new NotFoundException(nameof(TeamMember), request.AssignedTo);
            }
            // TODO: Add AutoMapper and remove this direct assignment
            var workItem = new WorkItem()
            {
                Name = request.Name,
                Description = request.Description
            };
            // This assignment cannot be done by automapper so we'll do it separately
            workItem.AssignedTo = assignedTo;
            _applicationDbContext.WorkItems.Add(workItem);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return workItem.Id;
        }
    }
}
