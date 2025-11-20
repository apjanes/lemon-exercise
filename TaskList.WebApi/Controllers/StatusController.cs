using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TaskList.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class StatusController : ControllerBase
{
    private readonly IConfiguration _config;

    public StatusController(IConfiguration config)
    {
        _config = config;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var status = new
        {
            EnvironmentAspNetCore = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
            EnvironmentConfig = _config.GetValue<string>("Environment")
        };

        return Ok(status);
    }
}