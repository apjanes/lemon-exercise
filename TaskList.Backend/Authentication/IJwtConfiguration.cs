using Microsoft.IdentityModel.Tokens;

namespace TaskList.Backend.Authentication;

public interface IJwtConfiguration
{
    public string Key { get; }

    public SecurityKey SigningKey { get; }
}