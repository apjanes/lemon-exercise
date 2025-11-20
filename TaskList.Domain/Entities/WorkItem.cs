using System.Diagnostics.CodeAnalysis;

namespace TaskList.Domain.Entities;

public class WorkItem
{
    [SetsRequiredMembers]
    public WorkItem(Guid id, string title, string? description, DateTime createdAt, bool isComplete = false)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);

        if (id == Guid.Empty)
        {
            throw new ArgumentException(nameof(id), "The id cannot be empty.");
        }

        Id = id;
        Title = title.Trim();
        Description = description;
        CreatedAt = createdAt == default ? DateTime.UtcNow : createdAt;
        IsComplete = isComplete;
    }

    public void Update(string title, string? description, bool isComplete = false)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        Title = title.Trim();
        Description = description;
        IsComplete = isComplete;
    }

    public void Update(WorkItem source)
    {
        Update(source.Title, source.Description, source.IsComplete);
    }

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public string? Description { get; set; }

    public Guid Id { get; private set; }

    public bool IsComplete { get; set; }

    public required string Title { get; set; }
}