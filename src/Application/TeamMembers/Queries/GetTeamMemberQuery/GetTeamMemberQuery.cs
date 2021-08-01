using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Api.Application.Common.Interfaces;
using TaskManager.Api.Application.TeamMembers.Common;

namespace TaskManager.Api.Application.TeamMembers.Queries.GetTeamMemberQuery
{
    public class GetTeamMemberQuery : IRequest<ShallowTeamMemberDto> { }
    public class GetTeamMemberQueryHandler : IRequestHandler<GetTeamMemberQuery, ShallowTeamMemberDto>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        public GetTeamMemberQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public Task<ShallowTeamMemberDto> Handle(GetTeamMemberQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
