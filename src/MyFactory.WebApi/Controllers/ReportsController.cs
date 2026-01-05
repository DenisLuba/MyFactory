using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFactory.Application.Features.Reports.ApproveMonthlyFinancialReport;
using MyFactory.Application.Features.Reports.CalculateMonthlyFinancialReport;
using MyFactory.Application.Features.Reports.CloseMonthlyFinancialReport;
using MyFactory.Application.Features.Reports.GetMonthlyFinancialReportDetails;
using MyFactory.Application.Features.Reports.GetMonthlyFinancialReports;
using MyFactory.Application.Features.Reports.RecalculateMonthlyFinancialReport;
using MyFactory.WebApi.Contracts.Reports;
using MyFactory.WebApi.SwaggerExamples.Reports;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/reports")]
[Produces("application/json")]
public class ReportsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // -------------------------
    //  MONTHLY LIST
    // -------------------------
    [HttpGet("monthly")]
    [ProducesResponseType(typeof(IReadOnlyList<MonthlyFinancialReportListItemResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(MonthlyFinancialReportsResponseExample))]
    public async Task<IActionResult> GetMonthly()
    {
        var dtos = await _mediator.Send(new GetMonthlyFinancialReportsQuery());
        var response = dtos
            .Select(x => new MonthlyFinancialReportListItemResponse(
                x.Year,
                x.Month,
                x.TotalRevenue,
                x.PayrollExpenses,
                x.MaterialExpenses,
                x.OtherExpenses,
                x.Profit,
                x.Status))
            .ToList();
        return Ok(response);
    }

    // -------------------------
    //  MONTHLY DETAILS
    // -------------------------
    [HttpGet("monthly/{year:int}/{month:int}")]
    [ProducesResponseType(typeof(MonthlyFinancialReportDetailsResponse), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(MonthlyFinancialReportDetailsResponseExample))]
    public async Task<IActionResult> GetMonthlyDetails(int year, int month)
    {
        var dto = await _mediator.Send(new GetMonthlyFinancialReportDetailsQuery(year, month));
        var response = new MonthlyFinancialReportDetailsResponse(
            dto.Id,
            dto.Year,
            dto.Month,
            dto.TotalRevenue,
            dto.PayrollExpenses,
            dto.MaterialExpenses,
            dto.OtherExpenses,
            dto.Profit,
            dto.Status,
            dto.CalculatedAt,
            dto.CreatedBy);
        return Ok(response);
    }

    // -------------------------
    //  CALCULATE
    // -------------------------
    [HttpPost("monthly/calculate")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(CalculateMonthlyFinancialReportResponse), StatusCodes.Status201Created)]
    [SwaggerRequestExample(typeof(CalculateMonthlyFinancialReportRequest), typeof(CalculateMonthlyFinancialReportRequestExample))]
    [SwaggerResponseExample(201, typeof(CalculateMonthlyFinancialReportResponseExample))]
    public async Task<IActionResult> Calculate([FromBody] CalculateMonthlyFinancialReportRequest req)
    {
        var id = await _mediator.Send(new CalculateMonthlyFinancialReportCommand(req.Year, req.Month));
        return Created(string.Empty, new CalculateMonthlyFinancialReportResponse(id));
    }

    // -------------------------
    //  RECALCULATE
    // -------------------------
    [HttpPost("monthly/recalculate")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerRequestExample(typeof(RecalculateMonthlyFinancialReportRequest), typeof(RecalculateMonthlyFinancialReportRequestExample))]
    public async Task<IActionResult> Recalculate([FromBody] RecalculateMonthlyFinancialReportRequest req)
    {
        await _mediator.Send(new RecalculateMonthlyFinancialReportCommand(req.Year, req.Month));
        return NoContent();
    }

    // -------------------------
    //  APPROVE
    // -------------------------
    [HttpPost("monthly/approve")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerRequestExample(typeof(ApproveMonthlyFinancialReportRequest), typeof(ApproveMonthlyFinancialReportRequestExample))]
    public async Task<IActionResult> Approve([FromBody] ApproveMonthlyFinancialReportRequest req)
    {
        await _mediator.Send(new ApproveMonthlyFinancialReportCommand(req.Year, req.Month));
        return NoContent();
    }

    // -------------------------
    //  CLOSE
    // -------------------------
    [HttpPost("monthly/close")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerRequestExample(typeof(CloseMonthlyFinancialReportRequest), typeof(CloseMonthlyFinancialReportRequestExample))]
    public async Task<IActionResult> Close([FromBody] CloseMonthlyFinancialReportRequest req)
    {
        await _mediator.Send(new CloseMonthlyFinancialReportCommand(req.Year, req.Month));
        return NoContent();
    }
}
