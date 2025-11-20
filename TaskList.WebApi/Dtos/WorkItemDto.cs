using System.Diagnostics.CodeAnalysis;
using TaskList.Domain.Entities;

namespace TaskList.WebApi.Dtos;

public class WorkItemDto
{
    public WorkItemDto() { }

    [SetsRequiredMembers]
    internal WorkItemDto(WorkItem workItem)
    {
        CreatedAt = workItem.CreatedAt;
        Description = workItem.Description;
        Id = workItem.Id;
        IsComplete = workItem.IsComplete;
        Summary = workItem.Summary;
    }

    public DateTimeOffset? CreatedAt { get; set; }

    public string? Description { get; set; }

    public Guid? Id { get; set; }

    public bool IsComplete { get; set; }

    public bool IsNew => Id == null;

    public required string Summary { get; set; }
}