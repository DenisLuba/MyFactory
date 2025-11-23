using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Auth;
using MyFactory.WebApi.SwaggerExamples.Auth;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    [SwaggerRequestExample(typeof(LoginRequest), typeof(LoginRequestExample))]
    [SwaggerResponseExample(200, typeof(LoginResponseExample))]
    public IActionResult Login([FromBody] LoginRequest req)
        => Ok(new LoginResponse(
            AccessToken: "sample.jwt.token",
            RefreshToken: "sample.refresh",
            ExpiresIn: 3600
        ));

    [HttpPost("refresh")]
    [SwaggerRequestExample(typeof(RefreshRequest), typeof(RefreshRequestExample))]
    [SwaggerResponseExample(200, typeof(RefreshResponseExample))]
    public IActionResult Refresh([FromBody] RefreshRequest req)
        => Ok(new RefreshResponse(
            AccessToken: "sample.new.jwt",
            ExpiresIn: 3600
        ));

    [HttpPost("register")]
    [SwaggerRequestExample(typeof(RegisterRequest), typeof(RegisterRequestExample))]
    [SwaggerResponseExample(200, typeof(RegisterResponseExample))]
    public IActionResult Register([FromBody] RegisterRequest req)
        => Created("",
            new RegisterResponse(
                Id: Guid.NewGuid(),
                Status: RegisterStatus.Created
            )
        );
}
