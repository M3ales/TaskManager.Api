using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Api.Domain.Entities
{
    public class TaskItem
    {
        public int Id { get; set; }
        public TeamMember AssignedTo { get; set; }
    }
}
