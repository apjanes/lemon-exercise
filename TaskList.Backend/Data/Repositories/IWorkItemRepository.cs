using TaskList.Backend.Data.Entities;

namespace TaskList.Backend.Data.Repositories;

public interface IWorkItemRepository
{
    public Task<IEnumerable<WorkItem>> ListAsync(CancellationToken cancellationToken = default);

    public Task SaveAsync(WorkItem workItem, CancellationToken cancellationToken = default);
}