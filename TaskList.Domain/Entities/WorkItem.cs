using System.Diagnostics.CodeAnalysis;

namespace TaskList.Domain.Entities;

public class WorkItem
{
    [SetsRequiredMembers]
    public WorkItem(Guid id, string summary, string? description, DateTime createdAt, bool isComplete = false)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(summary);

        if (id == Guid.Empty)
        {
            throw new ArgumentException(nameof(id), "The id cannot be empty.");
        }

        Id = id;
        Summary = summary.Trim();
        Description = description;
        CreatedAt = createdAt == default ? DateTime.UtcNow : createdAt;
        IsComplete = isComplete;
    }

    public void Update(string summary, string? description, bool isComplete = false)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(summary);
        Summary = summary.Trim();
        Description = description;
        IsComplete = isComplete;
    }

    public void Update(WorkItem source)
    {
        Update(source.Summary, source.Description, source.IsComplete);
    }

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public string? Description { get; set; }

    public Guid Id { get; private set; }

    public bool IsComplete { get; set; }

    public required string Summary { get; set; }
}