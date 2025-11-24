using Microsoft.AspNetCore.Mvc;
using MyFactory.WebApi.Contracts.Payroll;
using MyFactory.WebApi.SwaggerExamples.Payroll;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/payroll")]
[Produces("application/json")]
public class PayrollController : ControllerBase
{
    // GET /api/payroll
    [HttpGet]
    [SwaggerResponseExample(200, typeof(PayrollGetResponseExample))]
    [ProducesResponseType(typeof(IEnumerable<PayrollGetResponse>), StatusCodes.Status200OK)]
    public IActionResult Get([FromQuery] int periodMonth = 11, int periodYear = 2025)
        => Ok(new PayrollGetResponse[]
        {
            new(
                EmployeeId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Period: $"{periodMonth:D2}.{periodYear}",
                Accrued: 32500,
                Paid: 15000,
                Outstanding: 17500
            )
        });

    // POST /api/payroll/calc
    [HttpPost("calc")]
    [SwaggerResponseExample(200, typeof(PayrollCalculateResponseExample))]
    [ProducesResponseType(typeof(PayrollCalculateResponse), StatusCodes.Status200OK)]
    public IActionResult Calculate([FromQuery] DateTime from, [FromQuery] DateTime to)
        => Ok(new PayrollCalculateResponse(
            Status: PayrollCalculatingStatus.CalculationStarted,
            From: from,
            To: to
        ));

    // POST /api/payroll/pay
    [HttpPost("pay")]
    [SwaggerRequestExample(typeof(PayrollPayRequest), typeof(PayrollPayRequestExample))]
    [SwaggerResponseExample(200, typeof(PayrollPayResponseExample))]
    [ProducesResponseType(typeof(PayrollPayResponse), StatusCodes.Status200OK)]
    public IActionResult Pay([FromBody] PayrollPayRequest dto)
        => Ok(new PayrollPayResponse(PayrollPaymentStatus.Paid));
}

