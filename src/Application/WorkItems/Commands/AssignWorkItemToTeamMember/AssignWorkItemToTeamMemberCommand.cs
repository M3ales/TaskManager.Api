using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Api.Application.Common.Exceptions;
using TaskManager.Api.Application.Common.Interfaces;
using TaskManager.Api.Domain.Entities;

namespace TaskManager.Api.Application.WorkItems.Commands.AssignWorkItemToTeamMember
{
    public class AssignWorkItemToTeamMemberCommand : IRequest
    {
        public int WorkItemId { get; set; }
        public int TeamMemberId { get; set; }
    }

    public class AssignWorkItemToTeamMemberCommandHandler : IRequestHandler<AssignWorkItemToTeamMemberCommand, Unit>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        public AssignWorkItemToTeamMemberCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<Unit> Handle(AssignWorkItemToTeamMemberCommand request, CancellationToken cancellationToken)
        {
            // Going to pretend its hot path code and do a https://stackoverflow.com/questions/8663897/why-is-linq-wherepredicate-first-faster-than-firstpredicate
            var workItem = _applicationDbContext.WorkItems
                .Where(workItem => workItem.Id == request.WorkItemId)
                .FirstOrDefault() ?? throw new NotFoundException(nameof(WorkItem), request.WorkItemId);
            var teamMember = _applicationDbContext.TeamMembers
                .Where(teamMember => teamMember.Id == request.TeamMemberId)
                .FirstOrDefault() ?? throw new NotFoundException(nameof(TeamMember), request.TeamMemberId);
            workItem.AssignedTo = teamMember;
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
