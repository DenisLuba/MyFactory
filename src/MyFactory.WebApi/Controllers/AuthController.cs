using Microsoft.AspNetCore.Mvc;
using MyFactory.WebApi.Contracts.Auth;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest req)
        => Ok(new LoginResponse(
            AccessToken: "sample.jwt.token",
            RefreshToken: "sample.refresh",
            ExpiresIn: 3600
        ));

    [HttpPost("refresh")]
    public IActionResult Refresh([FromBody] RefreshRequest req)
        => Ok(new RefreshResponse(
            AccessToken: "sample.new.jwt",
            ExpiresIn: 3600
        ));

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequest req)
        => Created("",
            new RegisterResponse(
                Id: Guid.NewGuid(),
                Status: RegisterStatus.Created
            )
        );
}
