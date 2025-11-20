using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskList.Domain.Repositories;
using TaskList.WebApi.Authentication;
using TaskList.WebApi.Dtos;

namespace TaskList.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : Controller
{
    private readonly TokenService _tokenService;
    private readonly IRefreshStore _refreshStore;
    private readonly IUserRepository _userRepository;

    public AuthController(TokenService tokenService, IRefreshStore refreshStore, IUserRepository userRepository)
    {
        _tokenService = tokenService;
        _refreshStore = refreshStore;
        _userRepository = userRepository;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequestDto request)
    {
        // DEBUG:
        var user = await _userRepository.FindAsync(request.Username, request.Password);
        if (user == null)
        {
            return Unauthorized();
        }

        var userId = user.Username;
        var accessToken = _tokenService.CreateAccessToken(userId);
        var refreshToken = _tokenService.CreateRefreshToken();
        var now = DateTimeOffset.UtcNow;
        var expiresAt = now.Add(_refreshStore.DefaultLifetime);

        await _refreshStore.SaveAsync(userId, refreshToken, expiresAt);

        SetRefreshCookie(refreshToken, expiresAt);

        return Ok(new { accessToken });
    }

    [HttpPost("logout")]
    [AllowAnonymous]
    public async Task<IActionResult> LogoutAsync()
    {
        var cookie = Request.Cookies["rt"];
        if (!string.IsNullOrWhiteSpace(cookie))
            await _refreshStore.DeleteAsync(cookie);

        SetRefreshCookie(string.Empty, DateTimeOffset.UnixEpoch);

        return Ok();
    }

    // DEBUG: remove if unnecessary
    [HttpGet("me")]
    public IActionResult GetMeAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Ok();
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshAsync()
    {
        var cookie = Request.Cookies["rt"];
        var cookie2 = Request.Cookies["test"];
        if (string.IsNullOrWhiteSpace(cookie))
            return Unauthorized();

        var record = await _refreshStore.GetAsync(cookie);
        if (record == null)
            return Unauthorized();

        var newAccess = _tokenService.CreateAccessToken(record.UserId);
        var newRefresh = _tokenService.CreateRefreshToken();
        var now = DateTimeOffset.UtcNow;
        var expiresAt = now.Add(_refreshStore.DefaultLifetime);

        await _refreshStore.DeleteAsync(cookie);
        await _refreshStore.SaveAsync(record.UserId, newRefresh, expiresAt);

        SetRefreshCookie(newRefresh, expiresAt);

        return Ok(new { accessToken = newAccess });
    }

    private void SetRefreshCookie(string refreshToken, DateTimeOffset expiresAt)
    {
        Response.Cookies.Append("test", "test");

        Response.Cookies.Append("rt", refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Path = "/auth",
            Expires = expiresAt
        });
    }
}