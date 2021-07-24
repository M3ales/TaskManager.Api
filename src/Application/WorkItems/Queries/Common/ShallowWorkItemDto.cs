using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Api.Application.Common.Mappings;
using TaskManager.Api.Domain.Entities;

namespace TaskManager.Api.Application.WorkItems.Queries.Common
{
    public class ShallowWorkItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? AssignedTo { get; set; }
    }
}
