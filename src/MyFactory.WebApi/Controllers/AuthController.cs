using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
        => Ok(new
        {
            token = "sample-jwt-token",
            user = new { id = "USR-001", name = "Admin User", role = "Director" }
        });

    [HttpPost("refresh")]
    public IActionResult Refresh()
        => Ok(new { token = "sample-new-token" });
}

public record LoginRequest(string Username, string Password);

