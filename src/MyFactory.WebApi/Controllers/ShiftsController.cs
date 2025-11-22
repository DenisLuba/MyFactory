using Microsoft.AspNetCore.Mvc;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/shifts")]
public class ShiftsController : ControllerBase
{
    [HttpPost("plans")]
    public IActionResult CreatePlan([FromBody] object dto)
        => Created("", new
        {
            planId = "sp-001"
        });

    [HttpGet("plans")]
    public IActionResult GetPlans([FromQuery] DateTime? date = null)
        => Ok(new[]
        {
            new
            {
                shiftPlanId = "plan-001",
                employeeId = "emp-01",
                specificationId = "sp-001",
                plannedQuantity = 12,
                date = (date ?? DateTime.Parse("2025-12-12")).ToString("yyyy-MM-dd")
            }
        });

    [HttpPost("results")]
    public IActionResult RecordResult([FromBody] object dto)
        => Ok(new
        {
            status = "recorded"
        });

    [HttpGet("results")]
    public IActionResult GetResults(
        [FromQuery] string? employeeId = null,
        [FromQuery] DateTime? date = null)
        => Ok(new[]
        {
            new
            {
                shiftPlanId = "plan-001",
                actualQty = 14,
                hoursWorked = 7.5
            }
        });
}