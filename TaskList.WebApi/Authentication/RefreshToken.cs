namespace TaskList.WebApi.Authentication;

public record RefreshToken(string UserId, DateTimeOffset ExpiresAt);
