using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Api.Application.Common.Mappings;
using TaskManager.Api.Domain.Entities;

namespace TaskManager.Api.Application.ProgressItems.Common
{
    public class ProgressItemDto : IMapFrom<ProgressItem>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Complete { get; set; }
    }
}
