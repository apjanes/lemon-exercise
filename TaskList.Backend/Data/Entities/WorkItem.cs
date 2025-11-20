using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskList.Backend.Data.Dtos;

namespace TaskList.Backend.Data.Entities;

public class WorkItem
{
    public static WorkItem FromDto(WorkItemDto dto)
    {
        return new WorkItem
        {
            Id = dto.Id ?? CombGuid.NewGuid(),
            Summary = dto.Summary,
            Description = dto.Description,
            CreatedAt = dto.CreatedAt ?? DateTimeOffset.UtcNow
        };
    }

    public WorkItemDto ToDto()
    {
        return new WorkItemDto
        {
            CreatedAt = CreatedAt,
            Description = Description,
            Id = Id,
            IsComplete = IsComplete,
            Summary = Summary,
        };
    }


    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;

    public string? Description { get; set; }

    public Guid Id { get; private set; } = CombGuid.NewGuid();

    public bool IsComplete { get; set; }

    public required string Summary { get; set; }

    public void UpdateFrom(WorkItem source)
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
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder
                .Property(x => x.Description)
                .HasMaxLength(1000);

            builder
                .Property(x => x.Summary)
                .HasMaxLength(500);
        }
    }
}