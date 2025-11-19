namespace TaskList.Backend.Authentication;

public record RefreshToken(string UserId, DateTimeOffset ExpiresAt);
