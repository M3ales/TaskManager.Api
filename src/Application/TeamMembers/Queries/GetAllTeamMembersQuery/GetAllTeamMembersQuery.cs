using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Api.Application.Common.Interfaces;
using TaskManager.Api.Application.TeamMembers.Common;

namespace TaskManager.Api.Application.TeamMembers.Queries.GetAllTeamMembersQuery
{
    public class GetAllTeamMembersQuery : IRequest<List<ShallowTeamMemberDto>> { }
    public class GetTeamMembersQueryHandler : IRequestHandler<GetAllTeamMembersQuery, List<ShallowTeamMemberDto>>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        public GetTeamMembersQueryHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public Task<List<ShallowTeamMemberDto>> Handle(GetAllTeamMembersQuery request, CancellationToken cancellationToken)
        {
            return _applicationDbContext
                .TeamMembers
                .AsNoTracking()
                .ProjectTo<ShallowTeamMemberDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
