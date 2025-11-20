namespace TaskList.WebApi.Authentication;

public interface IRefreshStore
{
    TimeSpan DefaultLifetime { get; }
    Task SaveAsync(string userId, string token, DateTimeOffset expiresAt);
    Task<RefreshToken?> GetAsync(string token);
    Task DeleteAsync(string token);
}