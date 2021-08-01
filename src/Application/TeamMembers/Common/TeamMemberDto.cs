using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Api.Application.Common.Mappings;
using TaskManager.Api.Application.WorkItems.Common;
using TaskManager.Api.Domain.Entities;

namespace TaskManager.Api.Application.TeamMembers.Common
{
    public class TeamMemberDto : IMapFrom<TeamMember>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ShallowWorkItemDto> WorkItems { get; set; }
    }
}
