using Microsoft.AspNetCore.Mvc;
using MyFactory.WebApi.Contracts.Auth;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest req)
        => Ok(new LoginResponse("sample.jwt.token", "sample.refresh", 3600));

    [HttpPost("refresh")]
    public IActionResult Refresh([FromBody] RefreshRequest req)
        => Ok(new RefreshResponse("sample.new.jwt", 3600));

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequest req)
        => CreatedAtAction(nameof(Register), new RegisterResponse(Guid.NewGuid(), "created"));
}