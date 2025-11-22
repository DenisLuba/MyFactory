using Microsoft.AspNetCore.Mvc;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    [HttpGet]
    public IActionResult Get([FromQuery] string? role = null)
        => Ok(new[] {
            new { id = "usr-1", userName = "admin", email = "admin@acme", role = "Director", isActive = true },
            new { id = "usr-2", userName = "keeper", email = "keeper@acme", role = "Storekeeper", isActive = true }
        });

    [HttpGet("{id:guid}")]
    public IActionResult GetById(Guid id)
        => Ok(new { id, userName = "admin", email = "admin@acme", role = "Director", isActive = true });

    [HttpPost]
    public IActionResult Create([FromBody] object payload) // replace with DTO
        => Created("", new { status = "created" });

    [HttpPut("{id:guid}")]
    public IActionResult Update(Guid id, [FromBody] object payload)
        => Ok(new { status = "updated", id });

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
        => Ok(new { status = "deleted", id });
}
