using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Api.Application.Common.Attributes;
using TaskManager.Api.Application.Common.Exceptions;
using TaskManager.Api.Application.Common.Interfaces;
using TaskManager.Api.Domain.Entities;

namespace TaskManager.Api.Application.TeamMembers.Commands.DeleteTeamMemberCommand
{
    [Authorise]
    public class DeleteTeamMemberCommand : IRequest
    {
        public int Id { get; set; }
    }
    public class DeleteTeamMemberCommandHandler : IRequestHandler<DeleteTeamMemberCommand, Unit>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        public DeleteTeamMemberCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<Unit> Handle(DeleteTeamMemberCommand request, CancellationToken cancellationToken)
        {
            var teamMember = _applicationDbContext.TeamMembers
                .Where(teamMember => teamMember.Id == request.Id)
                .FirstOrDefault() ?? throw new NotFoundException(nameof(TeamMember), request.Id);
            _applicationDbContext.TeamMembers.Remove(teamMember);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
