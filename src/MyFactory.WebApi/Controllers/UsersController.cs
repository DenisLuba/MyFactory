using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFactory.Application.Features.Users.CreateRole;
using MyFactory.Application.Features.Users.CreateUser;
using MyFactory.Application.Features.Users.DeactivateRole;
using MyFactory.Application.Features.Users.DeactivateUser;
using MyFactory.Application.Features.Users.GetRoles;
using MyFactory.Application.Features.Users.GetUserDetails;
using MyFactory.Application.Features.Users.GetUsers;
using MyFactory.Application.Features.Users.UpdateRole;
using MyFactory.Application.Features.Users.UpdateUser;
using MyFactory.WebApi.Contracts.Users;
using MyFactory.WebApi.SwaggerExamples.Users;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/users")]
[Produces("application/json")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // -------------------------
    //  ROLES
    // -------------------------
    [HttpGet("roles")]
    [ProducesResponseType(typeof(IReadOnlyList<RoleResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(RoleListResponseExample))]
    public async Task<IActionResult> GetRoles()
    {
        var dtos = await _mediator.Send(new GetRolesQuery());
        var response = dtos
            .Select(r => new RoleResponse(r.Id, r.Name))
            .ToList();
        return Ok(response);
    }

    [HttpPost("roles")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(CreateRoleResponse), StatusCodes.Status201Created)]
    [SwaggerRequestExample(typeof(CreateRoleRequest), typeof(CreateRoleRequestExample))]
    [SwaggerResponseExample(201, typeof(CreateRoleResponseExample))]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest req)
    {
        var id = await _mediator.Send(new CreateRoleCommand(req.Name));
        return Created(string.Empty, new CreateRoleResponse(id));
    }

    [HttpPut("roles/{roleId:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerRequestExample(typeof(UpdateRoleRequest), typeof(UpdateRoleRequestExample))]
    public async Task<IActionResult> UpdateRole(Guid roleId, [FromBody] UpdateRoleRequest req)
    {
        await _mediator.Send(new UpdateRoleCommand(roleId, req.Name));
        return Ok();
    }

    [HttpDelete("roles/{roleId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RemoveRole(Guid roleId)
    {
        await _mediator.Send(new RemoveRoleCommand(roleId));
        return Ok();
    }

    // -------------------------
    //  USERS
    // -------------------------
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<UserListItemResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(UserListResponseExample))]
    public async Task<IActionResult> GetUsers([FromQuery] Guid? roleId, [FromQuery] string? roleName)
    {
        var dtos = await _mediator.Send(new GetUsersQuery(roleId, roleName));
        var response = dtos
            .Select(u => new UserListItemResponse(
                u.Id,
                u.Username,
                u.RoleName,
                u.IsActive,
                u.CreatedAt))
            .ToList();
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserDetailsResponse), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(UserDetailsResponseExample))]
    public async Task<IActionResult> GetUser(Guid id)
    {
        var dto = await _mediator.Send(new GetUserDetailsQuery(id));
        var response = new UserDetailsResponse(
            dto.Id,
            dto.Username,
            dto.RoleId,
            dto.RoleName,
            dto.IsActive,
            dto.CreatedAt);
        return Ok(response);
    }

    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(CreateUserResponse), StatusCodes.Status201Created)]
    [SwaggerRequestExample(typeof(CreateUserRequest), typeof(CreateUserRequestExample))]
    [SwaggerResponseExample(201, typeof(CreateUserResponseExample))]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest req)
    {
        var id = await _mediator.Send(new CreateUserCommand(req.Username, req.Password, req.RoleId));
        return Created(string.Empty, new CreateUserResponse(id));
    }

    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerRequestExample(typeof(UpdateUserRequest), typeof(UpdateUserRequestExample))]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserRequest req)
    {
        await _mediator.Send(new UpdateUserCommand(id, req.RoleId, req.IsActive));
        return Ok();
    }

    [HttpPost("{id:guid}/deactivate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeactivateUser(Guid id)
    {
        await _mediator.Send(new DeactivateUserCommand(id));
        return Ok();
    }
}
