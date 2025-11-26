using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Shifts;
using MyFactory.WebApi.SwaggerExamples.Shifts;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/shifts")]
public class ShiftsController : ControllerBase
{
    // POST /plans
    [HttpPost("plans")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [SwaggerRequestExample(typeof(ShiftsCreatePlanRequest), typeof(ShiftsCreatePlanRequestExample))]
    [SwaggerResponseExample(201, typeof(ShiftsCreatePlanResponseExample))]
    [ProducesResponseType(typeof(ShiftsCreatePlanResponse), StatusCodes.Status201Created)]
    public IActionResult CreatePlan([FromBody] ShiftsCreatePlanRequest dto)
        => Created(
            "",
            new ShiftsCreatePlanResponse(
                ShiftPlanId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Status: ShiftsStatus.Created
            )
        );

    // GET /plans
    [HttpGet("plans")]
    [Produces("application/json")]
    [SwaggerResponseExample(200, typeof(ShiftsGetPlansResponseExample))]
    [ProducesResponseType(typeof(IEnumerable<ShiftsGetPlansResponse>), StatusCodes.Status200OK)]
    public IActionResult GetPlans([FromQuery] DateTime? date = null)
        => Ok(new[]
        {
            new ShiftsGetPlansResponse(
                ShiftPlanId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
                EmployeeId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                SpecificationId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                PlannedQuantity: 12,
                Date: date ?? new DateTime(2025, 12, 12)
            )
        });

    // POST /results
    [HttpPost("results")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [SwaggerRequestExample(typeof(ShiftsRecordResultRequest), typeof(ShiftsRecordResultRequestExample))]
    [SwaggerResponseExample(200, typeof(ShiftsRecordResultResponseExample))]
    [ProducesResponseType(typeof(ShiftsRecordResultResponse), StatusCodes.Status200OK)]
    public IActionResult RecordResult([FromBody] ShiftsRecordResultRequest dto)
        => Ok(new ShiftsRecordResultResponse(
            ShiftPlanId: dto.ShiftPlanId,
            Status: ShiftsStatus.Recorded
        ));

    // GET /results
    [HttpGet("results")]
    [Produces("application/json")]
    [SwaggerResponseExample(200, typeof(ShiftsGetResultsResponseExample))]
    [ProducesResponseType(typeof(IEnumerable<ShiftsGetResultsResponse>), StatusCodes.Status200OK)]
    public IActionResult GetResults(
        [FromQuery] Guid? employeeId = null,
        [FromQuery] DateTime? date = null)
        => Ok(new[]
        {
            new ShiftsGetResultsResponse(
                ShiftPlanId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
                ActualQty: 14,
                HoursWorked: 7.5
            )
        });
}
