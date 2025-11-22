using Microsoft.AspNetCore.Mvc;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/payroll")]
public class PayrollController : ControllerBase
{
    [HttpGet]
    public IActionResult Get([FromQuery] int periodMonth = 11, int periodYear = 2025)
        => Ok(new[] { 
            new 
            { 
                employeeId = "101", 
                period = $"{periodMonth}.{periodYear}", 
                accrued = 32500, 
                paid = 15000, 
                outstanding = 17500 
            } 
        });

    [HttpPost("calc")]
    public IActionResult Calculate([FromQuery] DateTime from, [FromQuery] DateTime to)
        => Ok(new { status = "calculation-started", from, to });

    [HttpPost("pay")]
    public IActionResult Pay([FromBody] object dto)
        => Ok(new { status = "paid" });
}