using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Api.Application.Common.Interfaces;
using TaskManager.Api.Domain.Entities;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<TeamMember> TeamMembers => throw new NotImplementedException();

        public DbSet<WorkItem> WorkItems => throw new NotImplementedException();

        public DbSet<ProgressItem> ProgressItems => throw new NotImplementedException();
    }
}
