using Microsoft.IdentityModel.Tokens;

namespace TaskList.WebApi.Authentication;

public interface IJwtConfiguration
{
    public string Audience { get; }

    public string Issuer { get; }

    public string Key { get; }

    public SecurityKey SigningKey { get; }
}