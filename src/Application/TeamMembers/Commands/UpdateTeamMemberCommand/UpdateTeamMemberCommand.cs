using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Api.Application.Common.Exceptions;
using TaskManager.Api.Application.Common.Interfaces;
using TaskManager.Api.Application.TeamMembers.Common;
using TaskManager.Api.Domain.Entities;

namespace TaskManager.Api.Application.TeamMembers.Commands.UpdateTeamMemberCommand
{
    public class UpdateTeamMemberCommand : ShallowTeamMemberDto, IRequest<ShallowTeamMemberDto> { }
    public class UpdateTeamMemberCommandHandler : IRequestHandler<UpdateTeamMemberCommand, ShallowTeamMemberDto>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        public UpdateTeamMemberCommandHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }
        public async Task<ShallowTeamMemberDto> Handle(UpdateTeamMemberCommand request, CancellationToken cancellationToken)
        {
            var teamMember = _applicationDbContext.TeamMembers
                .Where(teamMember => teamMember.Id == request.Id)
                .FirstOrDefault() ?? throw new NotFoundException(nameof(TeamMember), request.Id);
            _applicationDbContext.TeamMembers.Update(teamMember);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return _mapper.Map<ShallowTeamMemberDto>(teamMember);
        }
    }
}
