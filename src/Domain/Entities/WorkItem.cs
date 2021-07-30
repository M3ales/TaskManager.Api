using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Api.Domain.Entities
{
    public class WorkItem
    {
        public WorkItem()
        {
            ProgressItems = new List<ProgressItem>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TeamMember AssignedTo { get; set; }
        public List<ProgressItem> ProgressItems { get; set; }
    }
}
