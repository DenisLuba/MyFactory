using Microsoft.AspNetCore.Mvc;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/reports")]
public class ReportsController : ControllerBase
{
    [HttpGet("monthly-profit")]
    public IActionResult MonthlyProfit([FromQuery] int month = 11, int year = 2025)
        => Ok(
            new 
            { 
                period = $"{month}.{year}", 
                revenue = 122300.0, 
                productionCost = 78500.0, 
                overhead = 18200.0, 
                wages = 15000.0,
                profit = 10600.0 
            });

    [HttpGet("revenue")]
    public IActionResult Revenue([FromQuery] int month = 11, int year = 2025)
        => Ok(new[] { new { spec = "Пижама", revenue = 55000.0 } });

    [HttpGet("production-cost")]
    public IActionResult ProductionCost([FromQuery] int month = 11, int year = 2025)
        => Ok(new[] { new { productionId = "pr-003", cost = 78500.0 } });
}
