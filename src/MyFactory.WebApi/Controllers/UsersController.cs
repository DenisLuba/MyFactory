using Microsoft.AspNetCore.Mvc;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    [HttpGet]
    public IActionResult GetUsers()
    {
        return Ok(new[] {
            new { id = "USR-001", name = "Администратор", role = "Director" },
            new { id = "USR-002", name = "Иванова О.Г.", role = "Sewer" }
        });
    }
}
