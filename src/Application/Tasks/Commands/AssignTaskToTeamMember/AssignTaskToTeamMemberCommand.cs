using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Api.Application.Common.Exceptions;
using TaskManager.Api.Application.Common.Interfaces;
using TaskManager.Api.Domain.Entities;

namespace TaskManager.Api.Application.Tasks.Commands.AssignTaskToTeamMember
{
    public class AssignTaskToTeamMemberCommand : IRequest
    {
        public int TaskId { get; set; }
        public int TeamMemberId { get; set; }
    }

    public class AssignTaskToTeamMemberCommandHandler : IRequestHandler<AssignTaskToTeamMemberCommand, Unit>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        public AssignTaskToTeamMemberCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<Unit> Handle(AssignTaskToTeamMemberCommand request, CancellationToken cancellationToken)
        {
            // Going to pretend its hot path code and do a https://stackoverflow.com/questions/8663897/why-is-linq-wherepredicate-first-faster-than-firstpredicate
            var task = _applicationDbContext.TaskItems
                .Where(task => task.Id == request.TaskId)
                .First();
            var teamMember = _applicationDbContext.TeamMembers
                .Where(teamMember => teamMember.Id == request.TeamMemberId)
                .FirstOrDefault() ?? throw new NotFoundException(nameof(TeamMember), request.TeamMemberId);
            task.AssignedTo = teamMember;
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
