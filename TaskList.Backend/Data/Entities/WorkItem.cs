using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TaskList.Backend.Data.Entities;

public class WorkItem
{
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public string? Description { get; set; }

    public Guid Id { get; set; } = CombGuid.NewGuid();

    public bool IsComplete { get; set; }

    public required string Summary { get; set; }

    public void Update(WorkItem source)
    {
        Description = source.Description;
        IsComplete = source.IsComplete;
        Summary = source.Summary;
    }

    public class Configuration : IEntityTypeConfiguration<WorkItem>
    {
        public void Configure(EntityTypeBuilder<WorkItem> builder)
        {
            builder
                .Property(x => x.Description)
                .HasMaxLength(1000);

            builder
                .Property(x => x.Summary)
                .HasMaxLength(500);
        }
    }
}