using Microsoft.AspNetCore.Mvc;
using MyFactory.WebApi.Contracts.Finance;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/finance")]
public class FinanceController : ControllerBase
{
    [HttpPost("overheads")]
    public IActionResult AddOverhead([FromBody] RecordOverheadRequest request)
    => Ok(new RecordOverheadResponse(FinanceStatus.OverheadRecorded));

    [HttpGet("overheads")]
    public IActionResult GetOverheads([FromQuery] int month = 11, int year = 2025)
        => Ok(new[] {
        new OverheadResponse(Guid.Parse("11111111-1111-1111-1111-111111111111"), 120000, $"{month}.{year}")
        });

    [HttpPost("advances")]
    public IActionResult CreateAdvance([FromBody] CreateAdvanceRequest request)
        => Created("", new CreateAdvanceResponse(Guid.Parse("11111111-1111-1111-1111-111111111222")));

    [HttpPost("advances/{id}/report")]
    public IActionResult SubmitAdvanceReport(string id, [FromBody] SubmitAdvanceReportRequest request)
        => Ok(new SubmitAdvanceReportResponse(Guid.Parse(id), FinanceStatus.ReportSubmitted));
}
