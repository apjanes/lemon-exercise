using Microsoft.AspNetCore.Identity;

namespace TaskList.Infrastructure;

public class PasswordHasher : IPasswordHasher
{
    private static readonly object Noop = new();
    private readonly PasswordHasher<object> _hasher = new();

    public string Hash(string password)
    {
        return _hasher.HashPassword(Noop, password);
    }

    public bool Verify(string hashed, string password)
    {
        return _hasher.VerifyHashedPassword(Noop, hashed, password)
            is PasswordVerificationResult.Success
            or PasswordVerificationResult.SuccessRehashNeeded;
    }
}