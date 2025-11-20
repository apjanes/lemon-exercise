using System.Diagnostics.CodeAnalysis;

namespace TaskList.Domain.Entities;

public class WorkItem
{
    public WorkItem()
    {

    }

    [SetsRequiredMembers]
    public WorkItem(Guid id, string title, string? description, DateTime? createdAt = null, DateTime? completedAt = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);

        if (id == Guid.Empty)
        {
            throw new ArgumentException("The id cannot be empty.", nameof(id));
        }

        if (createdAt > DateTime.UtcNow)
        {
            throw new ArgumentException("The creation date cannot be in the future.", nameof(createdAt));
        }

        Id = id;
        Title = title.Trim();
        Description = description;
        CompletedAt = completedAt;
        CreatedAt = createdAt ?? DateTime.UtcNow;
    }

    public void Update(string title, string? description, DateTime? completedAt = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        Title = title.Trim();
        Description = description;
    }

    public void Update(WorkItem source)
    {
        Update(source.Title, source.Description, source.CompletedAt);
    }
    public DateTime? CompletedAt { get; set; }

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public string? Description { get; set; }

    public Guid Id { get; private set; }

    public bool IsComplete => CompletedAt != null;

    public required string Title { get; set; }
}