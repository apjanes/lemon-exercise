using Microsoft.IdentityModel.Tokens;

namespace TaskList.WebApi.Authentication;

public interface IJwtConfiguration
{
    public string Key { get; }

    public SecurityKey SigningKey { get; }
}