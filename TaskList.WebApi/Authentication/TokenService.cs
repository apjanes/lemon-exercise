using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace TaskList.WebApi.Authentication;

public class TokenService
{
    private static readonly TimeSpan DefaultLifetime = TimeSpan.FromMinutes(20);
    private readonly IJwtConfiguration _configuration;

    public TokenService(IJwtConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string CreateAccessToken(string userId, TimeSpan lifetime = default)
    {
        lifetime = lifetime == TimeSpan.Zero ? DefaultLifetime : lifetime;

        var credentials = new SigningCredentials(_configuration.SigningKey, SecurityAlgorithms.HmacSha256);
        var now = DateTime.UtcNow;
        var expires = now.Add(lifetime);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(ClaimTypes.NameIdentifier, userId)
        };

        var token = new JwtSecurityToken(
            audience: _configuration.Audience,
            claims: claims,
            issuer: _configuration.Issuer,
            expires: expires,
            notBefore: now,
            signingCredentials: credentials);

        var result = new JwtSecurityTokenHandler().WriteToken(token);

        return result;
    }

    public string CreateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
}