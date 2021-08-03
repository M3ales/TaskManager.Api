using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Api.Application.Common.Attributes;
using TaskManager.Api.Application.Common.Interfaces;
using TaskManager.Api.Domain.Entities;

namespace TaskManager.Api.Application.TeamMembers.Commands.CreateTeamMemberCommand
{
    [Authorise]
    public class CreateTeamMemberCommand : IRequest<int> {
        public string Name { get; set; }
    }
    public class CreateTeamMemberCommandHandler : IRequestHandler<CreateTeamMemberCommand, int>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        public CreateTeamMemberCommandHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }
        public async Task<int> Handle(CreateTeamMemberCommand request, CancellationToken cancellationToken)
        {
            var teamMember = new TeamMember()
            {
                Name = request.Name
            };
            _applicationDbContext.TeamMembers.Add(teamMember);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return teamMember.Id;
        }
    }
}
