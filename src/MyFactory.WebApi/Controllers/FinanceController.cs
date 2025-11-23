using Microsoft.AspNetCore.Mvc;
using MyFactory.WebApi.Contracts.Finance;
using MyFactory.WebApi.SwaggerExamples.Finance;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/finance")]
public class FinanceController : ControllerBase
{
    [HttpPost("overheads")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [SwaggerRequestExample(typeof(RecordOverheadRequest), typeof(RecordOverheadRequestExample))]
    [SwaggerResponseExample(200, typeof(RecordOverheadResponseExample))]
    [ProducesResponseType(typeof(RecordOverheadResponse), StatusCodes.Status200OK)]
    public IActionResult AddOverhead([FromBody] RecordOverheadRequest request)
        => Ok(new RecordOverheadResponse(FinanceStatus.OverheadRecorded));

    [HttpGet("overheads")]
    [Produces("application/json")]
    [SwaggerResponseExample(200, typeof(OverheadResponseExample))]
    [ProducesResponseType(typeof(IEnumerable<OverheadResponse>), StatusCodes.Status200OK)]
    public IActionResult GetOverheads([FromQuery] int month = 11, int year = 2025)
        => Ok(new[]
        {
            new OverheadResponse(
                Guid.Parse("11111111-1111-1111-1111-111111111111"),
                120000,
                $"{month}.{year}"
            )
        });

    [HttpPost("advances")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [SwaggerRequestExample(typeof(CreateAdvanceRequest), typeof(CreateAdvanceRequestExample))]
    [SwaggerResponseExample(201, typeof(CreateAdvanceResponseExample))]
    [ProducesResponseType(typeof(CreateAdvanceResponse), StatusCodes.Status201Created)]
    public IActionResult CreateAdvance([FromBody] CreateAdvanceRequest request)
        => Created(
            "",
            new CreateAdvanceResponse(
                Guid.Parse("11111111-1111-1111-1111-111111111222")
            )
        );

    [HttpPost("advances/{id}/report")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [SwaggerRequestExample(typeof(SubmitAdvanceReportRequest), typeof(SubmitAdvanceReportRequestExample))]
    [SwaggerResponseExample(200, typeof(SubmitAdvanceReportResponseExample))]
    [ProducesResponseType(typeof(SubmitAdvanceReportResponse), StatusCodes.Status200OK)]
    public IActionResult SubmitAdvanceReport(
        string id,
        [FromBody] SubmitAdvanceReportRequest request)
        => Ok(new SubmitAdvanceReportResponse(Guid.Parse(id), FinanceStatus.ReportSubmitted));
}


