using System.Diagnostics.CodeAnalysis;
using TaskList.Domain.Entities;

namespace TaskList.WebApi.Dtos;

public class WorkItemDto
{
    public WorkItemDto() { }

    [SetsRequiredMembers]
    internal WorkItemDto(WorkItem workItem)
    {
        CompletedAt = workItem.CompletedAt;
        CreatedAt = workItem.CreatedAt;
        Description = workItem.Description;
        Id = workItem.Id;
        Title = workItem.Title;
        CreatedBy = UserDto.Create(workItem.CreatedBy);
    }

    public DateTime? CompletedAt { get; set; }

    public DateTime? CreatedAt { get; set; }
    public UserDto? CreatedBy { get; set; }

    public string? Description { get; set; }

    public Guid? Id { get; set; }

    public bool IsComplete => CompletedAt != null;

    public bool IsNew => Id == null;

    public required string Title { get; set; }

}