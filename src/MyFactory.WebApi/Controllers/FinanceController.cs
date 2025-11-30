using Microsoft.AspNetCore.Mvc;
using MyFactory.WebApi.Contracts.Finance;
using MyFactory.WebApi.SwaggerExamples.Finance;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/finance")]
[Produces("application/json")]
public class FinanceController : ControllerBase
{
    // POST /api/finance/overheads
    [HttpPost("overheads")]
    [Consumes("application/json")]
    [SwaggerRequestExample(typeof(RecordOverheadRequest), typeof(RecordOverheadRequestExample))]
    [SwaggerResponseExample(200, typeof(RecordOverheadResponseExample))]
    [ProducesResponseType(typeof(RecordOverheadResponse), StatusCodes.Status200OK)]
    public IActionResult AddOverhead([FromBody] RecordOverheadRequest request)
        => Ok(new RecordOverheadResponse(FinanceStatus.OverheadRecorded));

    // GET /api/finance/overheads
    [HttpGet("overheads")]
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

    // POST /api/finance/advances
    [HttpPost("advances")]
    [Consumes("application/json")]
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

    // POST /api/finance/advances/{id}/report
    [HttpPost("advances/{id}/report")]
    [Consumes("application/json")]
    [SwaggerRequestExample(typeof(SubmitAdvanceReportRequest), typeof(SubmitAdvanceReportRequestExample))]
    [SwaggerResponseExample(200, typeof(SubmitAdvanceReportResponseExample))]
    [ProducesResponseType(typeof(SubmitAdvanceReportResponse), StatusCodes.Status200OK)]
    public IActionResult SubmitAdvanceReport(
        string id,
        [FromBody] SubmitAdvanceReportRequest request)
        => Ok(new SubmitAdvanceReportResponse(Guid.Parse(id), FinanceStatus.ReportSubmitted));

    // GET /api/finance/advances
    [HttpGet("advances")]
    [SwaggerResponseExample(200, typeof(AdvanceItemDtoExample))]
    [ProducesResponseType(typeof(IEnumerable<AdvanceItemDto>), StatusCodes.Status200OK)]
    public IActionResult GetAdvances()
        => Ok(new[]
        {
            new AdvanceItemDto(
                "ADV-2024-001",
                "Иванов И.И.",
                15000,
                "2024-06-01",
                AdvanceStatus.Issued
            ),
            new AdvanceItemDto(
                "ADV-2024-002",
                "Петров П.П.",
                20000,
                "2024-06-05",
                AdvanceStatus.Reported
            )
        });
}



