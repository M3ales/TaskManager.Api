using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Api.Domain.Entities;

namespace TaskManager.Api.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        ICollection<TeamMember> TeamMembers { get; }
        ICollection<TaskItem> TaskItems { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
