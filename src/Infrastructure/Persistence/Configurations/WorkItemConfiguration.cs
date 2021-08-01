using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Api.Domain.Entities;

namespace Infrastructure.Persistence.Configurations
{
    public class WorkItemConfiguration : IEntityTypeConfiguration<WorkItem>
    {
        public void Configure(EntityTypeBuilder<WorkItem> builder)
        {
            builder
                .HasMany(workItem => workItem.ProgressItems)
                .WithOne(progressItem => progressItem.WorkItem);
        }
    }
}
