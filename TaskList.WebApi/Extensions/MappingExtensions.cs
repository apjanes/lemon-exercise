using TaskList.Domain.Entities;
using TaskList.Infrastructure;
using TaskList.WebApi.Dtos;

namespace TaskList.WebApi.Extensions;

public static class MappingExtensions
{
    public static WorkItemDto ToDto(this WorkItem source)
    {
        return new WorkItemDto(source);
    }

    public static WorkItem ToEntity(this WorkItemDto dto)
    {
        // DEBUG: question this in terms of a dto for update
        return new WorkItem(dto.Id ?? CombGuid.NewGuid(), dto.Summary, dto.Description, dto.CreatedAt ?? DateTimeOffset.UtcNow);
    }

    public static void Apply(this WorkItem entity, WorkItemDto dto)
    {
        entity.Update(dto.Summary, dto.Description, dto.IsComplete);
    }
}