using Microsoft.AspNetCore.Mvc;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/settings")]
public class SettingsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
        => Ok(new[] 
        { 
            new { key = "StandardShiftHours", value = "8", description = "..." }, 
            new { key = "Currency", value = "Р", description = "..." } 
        });

    [HttpGet("{key}")]
    public IActionResult Get(string key)
        => Ok(new { key, value = key == "StandardShiftHours" ? "8" : "Р", description = "..." });

    [HttpPut("{key}")]
    public IActionResult Update(string key, [FromBody] object dto)
        => Ok(new { key, status = "updated" });
}
