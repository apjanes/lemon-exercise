using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskList.Domain.Entities;
using TaskList.Domain.Repositories;
using TaskList.WebApi.Dtos;
using TaskList.WebApi.Extensions;

namespace TaskList.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("work-items")]
public class WorkItemsController : Controller
{
    private readonly IWorkItemRepository _workItemRepository;
    private readonly IUserRepository _userRepository;

    public WorkItemsController(IWorkItemRepository workItemRepository, IUserRepository userRepository)
    {
        _workItemRepository = workItemRepository;
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WorkItemDto>>> ListAsync()
    {
        var user = await GetUserAsync(User.Identity?.Name);
        var workItems = await _workItemRepository.ListAsync(user);
        var result = workItems.Select(x => x.ToDto());

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<WorkItemDto>> CreateAsync(WorkItemDto dto)
    {
        var user = await GetUserAsync(User.Identity?.Name);
        var workItem = dto.ToEntity(user);
        var saved = await _workItemRepository.CreateAsync(workItem, user);
        return Ok(saved.ToDto());
    }

    private async Task<User?> GetUserAsync(string? identityName)
    {
        if (identityName == null) return null;
        var result = await _userRepository.FindAsync(identityName);

        return result;
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        var success = await _workItemRepository.DeleteAsync(id);

        if (success) return Ok();
        return NotFound();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<WorkItemDto>> UpdateAsync(Guid id, WorkItemDto dto)
    {
        if (id == Guid.Empty) return NotFound();

        var user = await GetUserAsync(User.Identity?.Name);
        var workItem = dto.ToEntity(user);
        var saved = await _workItemRepository.UpdateAsync(id, workItem);

        return saved == null ? NotFound() : Ok(saved.ToDto());
    }

    [HttpPut("{id}/complete/{isComplete}")]
    public async Task<ActionResult<WorkItemDto>> CompleteAsync(Guid id, bool isComplete)
    {
        if (id == Guid.Empty) return NotFound();

        var result = await _workItemRepository.SetComplete(id, isComplete);

        if (result == null) return NotFound();

        return Ok(result.ToDto());
    }
}