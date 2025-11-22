using Microsoft.AspNetCore.Mvc;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/returns")]
public class ReturnsController : ControllerBase
{
    [HttpPost]
    public IActionResult CreateReturn([FromBody] object dto)
        => Created("", new
        {
            returnId = "ret-001",
            status = "accepted"
        });
}

