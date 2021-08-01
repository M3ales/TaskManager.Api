using System.Collections.Generic;
using TaskManager.Api.Application.Common.Mappings;
using TaskManager.Api.Domain.Entities;

namespace TaskManager.Api.Application.TeamMembers.Common
{
    public class ShallowTeamMemberDto : IMapFrom<TeamMember>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> WorkItems { get; set; }
    }
}
