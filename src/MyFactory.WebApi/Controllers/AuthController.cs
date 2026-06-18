using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFactory.Application.Features.Authentication.Login;
using MyFactory.Application.Features.Authentication.RefreshToken;
using MyFactory.Application.Features.Authentication.RegisterUser;
using MyFactory.WebApi.Contracts.Auth;
using MyFactory.WebApi.SwaggerExamples.Auth;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/auth")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // -------------------------
    //  LOGIN
    // -------------------------
    [HttpPost("login")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [SwaggerRequestExample(typeof(LoginRequest), typeof(LoginRequestExample))]
    [SwaggerResponseExample(200, typeof(LoginResponseExample))]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        var result = await _mediator.Send(new LoginCommand(req.Username, req.Password));
        var expiresIn = (int)Math.Max(0, (result.ExpiresAt - DateTime.UtcNow).TotalSeconds);

        return Ok(new LoginResponse(
            AccessToken: result.AccessToken,
            RefreshToken: result.RefreshToken,
            ExpiresIn: expiresIn));
    }

    // -------------------------
    //  REFRESH
    // -------------------------
    [HttpPost("refresh")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(RefreshResponse), StatusCodes.Status200OK)]
    [SwaggerRequestExample(typeof(RefreshRequest), typeof(RefreshRequestExample))]
    [SwaggerResponseExample(200, typeof(RefreshResponseExample))]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest req)
    {
        var result = await _mediator.Send(new RefreshTokenCommand(req.RefreshToken));
        var expiresIn = (int)Math.Max(0, (result.ExpiresAt - DateTime.UtcNow).TotalSeconds);

        return Ok(new RefreshResponse(
            AccessToken: result.AccessToken,
            ExpiresIn: expiresIn));
    }

    // -------------------------
    //  REGISTER
    // -------------------------
    [HttpPost("register")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status201Created)]
    [SwaggerRequestExample(typeof(RegisterRequest), typeof(RegisterRequestExample))]
    [SwaggerResponseExample(201, typeof(RegisterResponseExample))]
    public async Task<IActionResult> Register([FromBody] RegisterRequest req)
    {
        var result = await _mediator.Send(new RegisterUserCommand(req.UserName, req.Password, req.RoleId));

        return Created(
            uri: string.Empty,
            value: new RegisterResponse(
                Id: result.UserId,
                Status: RegisterStatus.Created));
    }
}

