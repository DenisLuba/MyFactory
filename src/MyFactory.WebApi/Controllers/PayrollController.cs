using Microsoft.AspNetCore.Mvc;
using MyFactory.WebApi.Contracts.Finance;
using MyFactory.WebApi.Contracts.Payroll;
using MyFactory.WebApi.SwaggerExamples.Payroll;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/payroll")]
public class PayrollController : ControllerBase
{
    [HttpGet]
    [Produces("application/json")]
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

    [HttpPost("calc")]
    [SwaggerResponseExample(200, typeof(PayrollCalculateResponseExample))]
    public IActionResult Calculate([FromQuery] DateTime from, [FromQuery] DateTime to)
        => Ok(new PayrollCalculateResponse(
            Status: PayrollCalculatingStatus.CalculationStarted,
            From: from,
            To: to
        ));

    [HttpPost("pay")]
    [SwaggerRequestExample(typeof(PayrollPayRequest), typeof(PayrollPayRequestExample))]
    [SwaggerResponseExample(200, typeof(PayrollPayResponseExample))]
    public IActionResult Pay([FromBody] PayrollPayRequest dto)
        => Ok(new PayrollPayResponse(PayrollPaymentStatus.Paid));
}
