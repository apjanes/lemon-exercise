using System.Collections.Concurrent;

namespace TaskList.Backend.Authentication;

// DEBUG: details of the unsuitablity for production
public class InMemoryRefreshStore : IRefreshStore
{
    private readonly ConcurrentDictionary<string, (string userId, DateTimeOffset expiry)> _store = new();

    public TimeSpan DefaultLifetime => TimeSpan.FromDays(7);

    public Task SaveAsync(string userId, string token, DateTimeOffset expiresAt)
    {
        // DEBUG: explain that async is needed for non-inmemory stores.
        _store[token] = (userId, expiresAt);
        return Task.CompletedTask;
    }

    public Task<RefreshToken?> GetAsync(string token)
    {
        if (_store.TryGetValue(token, out var value))
        {
            if (value.expiry > DateTimeOffset.UtcNow)
            {
                return Task.FromResult<RefreshToken?>(new RefreshToken(value.userId, value.expiry));
            }
            _store.TryRemove(token, out _);
        }
        return Task.FromResult<RefreshToken?>(null);
    }

    public Task DeleteAsync(string token)
    {
        _store.TryRemove(token, out _);
        return Task.CompletedTask;
    }
}