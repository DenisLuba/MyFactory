using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Users;
using MyFactory.WebApi.SwaggerExamples.Users;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/users")]
[Produces("application/json")]
public class UsersController : ControllerBase
{
    [HttpGet]
    [SwaggerResponseExample(200, typeof(UsersGetByRoleResponseExample))]
    [ProducesResponseType(typeof(IEnumerable<UsersGetByRoleResponse>), StatusCodes.Status200OK)]
    public IActionResult GetByRole([FromQuery] string? role = null)
        => Ok(new[]
        {
            new UsersGetByRoleResponse(
                Id: Guid.Parse("11111111-1111-1111-1111-111111111111"),
                UserName: "admin",
                Email: "admin@acme",
                Role: "Director",
                IsActive: true
            )
        });

    [HttpGet("{id:guid}")]
    [SwaggerResponseExample(200, typeof(UsersGetByIdResponseExample))]
    [ProducesResponseType(typeof(UsersGetByIdResponse), StatusCodes.Status200OK)]
    public IActionResult GetById(Guid id)
        => Ok(
            new UsersGetByIdResponse(
                id,
                "admin",
                "admin@acme",
                "Director",
                true
            )
        );

    [HttpPost]
    [Consumes("application/json")]
    [SwaggerRequestExample(typeof(UsersCreateRequest), typeof(UsersCreateRequestExample))]
    [SwaggerResponseExample(201, typeof(UsersCreateResponseExample))]
    [ProducesResponseType(typeof(UsersCreateResponse), StatusCodes.Status201Created)]
    public IActionResult Create([FromBody] UsersCreateRequest payload)
        => Created("", new UsersCreateResponse(Guid.NewGuid(), UserStatus.Created));

    [HttpPut("{id:guid}")]
    [SwaggerRequestExample(typeof(UsersUpdateRequest), typeof(UsersUpdateRequestExample))]
    [SwaggerResponseExample(200, typeof(UsersUpdateResponseExample))]
    [ProducesResponseType(typeof(UsersUpdateResponse), StatusCodes.Status200OK)]
    public IActionResult Update(Guid id, [FromBody] UsersUpdateRequest payload)
        => Ok(new UsersUpdateResponse(id, UserStatus.Updated));

    [HttpDelete("{id:guid}")]
    [SwaggerResponseExample(200, typeof(UsersDeleteResponseExample))]
    [ProducesResponseType(typeof(UsersDeleteResponse), StatusCodes.Status200OK)]
    public IActionResult Delete(Guid id)
        => Ok(new UsersDeleteResponse(id, UserStatus.Deleted));
}

