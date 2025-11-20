using TaskList.Backend.Data.Entities;

namespace TaskList.Backend.Data.Repositories;

public interface IWorkItemRepository
{
    public Task<IEnumerable<WorkItem>> ListAsync(CancellationToken cancellationToken = default);

    public Task<WorkItem?> FindAsync(Guid id, CancellationToken cancellationToken = default);

    public Task<WorkItem> SaveAsync(WorkItem workItem, bool isNew, CancellationToken cancellationToken = default);
}