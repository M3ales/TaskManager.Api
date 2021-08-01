using System.Collections.Generic;
using TaskManager.Api.Application.ProgressItems.Common;

namespace TaskManager.Api.Application.WorkItems.Common
{
    public class ShallowWorkItemDto
    {
        public ShallowWorkItemDto()
        {
            ProgressItems = new List<ProgressItemDto>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? AssignedTo { get; set; }
        public List<ProgressItemDto> ProgressItems { get; set; }
    }
}
