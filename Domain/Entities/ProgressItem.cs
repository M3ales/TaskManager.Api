using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Api.Domain.Entities
{
    public class ProgressItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Complete { get; set; }
        public WorkItem WorkItem { get; set; }
    }
}
