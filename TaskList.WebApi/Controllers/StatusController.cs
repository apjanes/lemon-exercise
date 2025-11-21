using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TaskList.WebApi.Controllers;

[ApiController]
[AllowAnonymous]
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
        return Ok("Alive");
    }
}