using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskList.Domain.Repositories;
using TaskList.WebApi.Dtos;
using TaskList.WebApi.Extensions;

namespace TaskList.WebApi.Controllers;

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
    public async Task<ActionResult<WorkItemDto>> CreateAsync(WorkItemDto dto)
    {
        var workItem = dto.ToEntity();
        var saved = await _repository.CreateAsync(workItem);
        return Ok(saved.ToDto());
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<WorkItemDto>> UpdateAsync(Guid id, WorkItemDto dto)
    {
        if (id == Guid.Empty) return NotFound();

        var workItem = dto.ToEntity();
        var saved = await _repository.UpdateAsync(id, workItem);
        return Ok(saved.ToDto());
    }
}