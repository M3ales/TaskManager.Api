using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Api.Domain.Entities;
using TaskManager.Api.Domain.Entities.Auth;

namespace TaskManager.Api.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<TeamMember> TeamMembers { get; }
        DbSet<WorkItem> WorkItems { get; }
        DbSet<ProgressItem> ProgressItems { get; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Claim> Claims { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
