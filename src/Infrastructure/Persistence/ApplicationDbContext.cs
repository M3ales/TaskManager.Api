using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TaskManager.Api.Application.Common.Interfaces;
using TaskManager.Api.Domain.Entities;
using TaskManager.Api.Domain.Entities.Auth;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<TeamMember> TeamMembers { get; set; }

        public DbSet<WorkItem> WorkItems { get; set; }

        public DbSet<ProgressItem> ProgressItems { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Claim> Claims { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }
    }
}
