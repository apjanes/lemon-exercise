using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskList.Backend.Data.Dtos;
using TaskList.Backend.Data.Entities;
using TaskList.Backend.Data.Repositories;

namespace TaskList.Backend.Controllers;

[ApiController]
[Authorize]
[Route("work-items")]
public class WorkItemsController : Controller
{
    private readonly IWorkItemRepository _repository;

    public WorkItemsController(IWorkItemRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WorkItemDto>>> ListAsync()
    {
        var workItems = await _repository.ListAsync();
        var result = workItems.Select(x => x.ToDto());

        return Ok(result);
    }

    [HttpPost]
    [HttpPut]
    public async Task<WorkItemDto> SaveAsync(WorkItemDto dto)
    {
        var workItem = WorkItem.FromDto(dto);
        var saved = await _repository.SaveAsync(workItem, dto.IsNew);
        return saved.ToDto();
    }
}