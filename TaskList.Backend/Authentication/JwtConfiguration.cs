using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace TaskList.Backend.Authentication;

public sealed class JwtConfiguration : IJwtConfiguration
{
    [SetsRequiredMembers]
    public JwtConfiguration(IConfiguration configuration, string sectionName = "Jwt")
    {
        ArgumentNullException.ThrowIfNull(configuration);

        Key = Require(configuration, sectionName, "Key"); //$"{sectionName}:Key");

        SigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
    }

    public string Key { get; }

    public SecurityKey SigningKey { get; }

    private static string Require(IConfiguration configuration, string sectionName, string valueName)
    {
        var section = configuration.GetRequiredSection(sectionName);
        var value = section[valueName];
        return !string.IsNullOrWhiteSpace(value)
            ? value
            : throw new InvalidOperationException($"Missing configuration for '{sectionName}:{valueName}'.");
    }
}