using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Auth;
using MyFactory.WebApi.SwaggerExamples.Auth;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/auth")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    // -------------------------
    //  LOGIN
    // -------------------------
    [HttpPost("login")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [SwaggerRequestExample(typeof(LoginRequest), typeof(LoginRequestExample))]
    [SwaggerResponseExample(200, typeof(LoginResponseExample))]
    public IActionResult Login([FromBody] LoginRequest req)
        => Ok(new LoginResponse(
            AccessToken: "sample.jwt.token",
            RefreshToken: "sample.refresh",
            ExpiresIn: 3600
        ));

    // -------------------------
    //  REFRESH
    // -------------------------
    [HttpPost("refresh")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(RefreshResponse), StatusCodes.Status200OK)]
    [SwaggerRequestExample(typeof(RefreshRequest), typeof(RefreshRequestExample))]
    [SwaggerResponseExample(200, typeof(RefreshResponseExample))]
    public IActionResult Refresh([FromBody] RefreshRequest req)
        => Ok(new RefreshResponse(
            AccessToken: "sample.new.jwt",
            ExpiresIn: 3600
        ));

    // -------------------------
    //  REGISTER
    // -------------------------
    [HttpPost("register")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status201Created)]
    [SwaggerRequestExample(typeof(RegisterRequest), typeof(RegisterRequestExample))]
    [SwaggerResponseExample(201, typeof(RegisterResponseExample))]
    public IActionResult Register([FromBody] RegisterRequest req)
        => Created(
            uri: "",
            value: new RegisterResponse(
                Id: Guid.NewGuid(),
                Status: RegisterStatus.Created
            )
        );
}

