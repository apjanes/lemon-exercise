using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskList.Domain.Entities;

namespace TaskList.Infrastructure;

public class WorkItemConfiguration : IEntityTypeConfiguration<WorkItem>
{
    public void Configure(EntityTypeBuilder<WorkItem> builder)
    {
        builder
            .Property(x => x.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder
            .Property(x => x.Description)
            .HasMaxLength(1000);

        builder
            .Property(x => x.Title)
            .HasMaxLength(500);
    }
}