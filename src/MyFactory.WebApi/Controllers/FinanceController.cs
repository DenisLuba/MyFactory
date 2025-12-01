using System;
using Microsoft.AspNetCore.Http;
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
        [ProducesResponseType(typeof(RecordOverheadResponse), 200)]
    public IActionResult AddOverhead([FromBody] RecordOverheadRequest request)
        => Ok(new RecordOverheadResponse(Guid.Parse("11111111-1111-1111-1111-111111111111"), OverheadStatus.Draft));

    // PUT /api/finance/overheads/{id}
    [HttpPut("overheads/{id}")]
    [Consumes("application/json")]
    [SwaggerRequestExample(typeof(RecordOverheadRequest), typeof(RecordOverheadRequestExample))]
    [SwaggerResponseExample(200, typeof(RecordOverheadResponseExample))]
        [ProducesResponseType(typeof(RecordOverheadResponse), 200)]
    public IActionResult UpdateOverhead(Guid id, [FromBody] RecordOverheadRequest request)
        => Ok(new RecordOverheadResponse(id, OverheadStatus.Draft));

    // PUT /api/finance/overheads/{id}/post
    [HttpPut("overheads/{id}/post")]
    [SwaggerResponseExample(200, typeof(RecordOverheadResponseExample))]
        [ProducesResponseType(typeof(RecordOverheadResponse), 200)]
    public IActionResult PostOverhead(Guid id)
        => Ok(new RecordOverheadResponse(id, OverheadStatus.Posted));

    // DELETE /api/finance/overheads/{id}
    [HttpDelete("overheads/{id}")]
    [SwaggerResponseExample(200, typeof(RecordOverheadResponseExample))]
        [ProducesResponseType(typeof(RecordOverheadResponse), 200)]
    public IActionResult DeleteOverhead(Guid id)
        => Ok(new RecordOverheadResponse(id, OverheadStatus.Draft));

    // GET /api/finance/overheads
    [HttpGet("overheads")]
    [SwaggerResponseExample(200, typeof(OverheadResponseExample))]
        [ProducesResponseType(typeof(IEnumerable<OverheadItemDto>), 200)]
    public IActionResult GetOverheads([FromQuery] int month = 11, int year = 2025, string? article = null, OverheadStatus? status = null)
        => Ok(new[]
        {
            new OverheadItemDto(
                Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                new DateTime(year, month, 1),
                article ?? "Аренда",
                25000m,
                "Аренда офиса",
                status ?? OverheadStatus.Posted
            ),
            new OverheadItemDto(
                Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                new DateTime(year, month, 2),
                "Коммуналка",
                3200m,
                "Свет + вода",
                OverheadStatus.Draft
            )
        });

    // GET /api/finance/overheads/articles
    [HttpGet("overheads/articles")]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
    public IActionResult GetOverheadArticles()
        => Ok(new[] { "Аренда", "Коммуналка", "Связь", "Прочие расходы" });

    // POST /api/finance/advances
    [HttpPost("advances")]
    [Consumes("application/json")]
    [SwaggerRequestExample(typeof(CreateAdvanceRequest), typeof(CreateAdvanceRequestExample))]
    [SwaggerResponseExample(201, typeof(AdvanceStatusResponseExample))]
        [ProducesResponseType(typeof(AdvanceStatusResponse), 201)]
    public IActionResult CreateAdvance([FromBody] CreateAdvanceRequest request)
        => Created(
            "",
            new AdvanceStatusResponse(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), AdvanceStatus.AdvanceCreated)
        );

    // POST /api/finance/advances/{id}/report
    [HttpPost("advances/{id}/report")]
    [Consumes("application/json")]
    [SwaggerRequestExample(typeof(SubmitAdvanceReportRequest), typeof(SubmitAdvanceReportRequestExample))]
    [SwaggerResponseExample(200, typeof(AdvanceStatusResponseExample))]
        [ProducesResponseType(typeof(AdvanceStatusResponse), 200)]
    public IActionResult SubmitAdvanceReport(
        string id,
        [FromBody] SubmitAdvanceReportRequest request)
        => Ok(new AdvanceStatusResponse(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), AdvanceStatus.Reported));

    // GET /api/finance/advances
    [HttpGet("advances")]
    [SwaggerResponseExample(200, typeof(AdvanceItemDtoExample))]
        [ProducesResponseType(typeof(IEnumerable<AdvanceItemDto>), 200)]
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

    // DELETE /api/finance/advances/{advanceNumber}
    [HttpDelete("advances/{advanceNumber}")]
    [SwaggerResponseExample(200, typeof(AdvanceStatusResponseExample))]
        [ProducesResponseType(typeof(AdvanceStatusResponse), 200)]
    public IActionResult DeleteAdvance(string advanceNumber)
        => Ok(new AdvanceStatusResponse(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), AdvanceStatus.AdvanceDeleted));

    // PUT /api/finance/advances/{advanceNumber}/close
    [HttpPut("advances/{advanceNumber}/close")]
    [SwaggerResponseExample(200, typeof(AdvanceStatusResponseExample))]
        [ProducesResponseType(typeof(AdvanceStatusResponse), 200)]
    public IActionResult CloseAdvance(string advanceNumber)
        => Ok(new AdvanceStatusResponse(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), AdvanceStatus.AdvanceClosed));
}



