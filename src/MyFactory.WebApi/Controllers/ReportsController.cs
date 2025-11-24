using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Reports;
using MyFactory.WebApi.SwaggerExamples.Reports;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/reports")]
[Produces("application/json")]
public class ReportsController : ControllerBase
{
    // -------------------------
    //  MONTHLY PROFIT
    // -------------------------
    [HttpGet("monthly-profit")]
    [SwaggerResponseExample(200, typeof(ReportsMonthlyProfitResponseExample))]
    [ProducesResponseType(typeof(ReportsMonthlyProfitResponse), StatusCodes.Status200OK)]
    public IActionResult MonthlyProfit([FromQuery] int month = 11, int year = 2025)
        => Ok(
            new ReportsMonthlyProfitResponse(
                Period: $"{month:D2}.{year}",
                Revenue: 122300m,
                ProductionCost: 78500m,
                Overhead: 18200m,
                Wages: 15000m,
                Profit: 10600m
            )
        );

    // -------------------------
    //  REVENUE BY PRODUCT
    // -------------------------
    [HttpGet("revenue")]
    [SwaggerResponseExample(200, typeof(ReportsRevenueResponseExample))]
    [ProducesResponseType(typeof(IEnumerable<ReportsRevenueResponse>), StatusCodes.Status200OK)]
    public IActionResult Revenue([FromQuery] int month = 11, int year = 2025)
        => Ok(
            new[]
            {
                new ReportsRevenueResponse(
                    SpecificationId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    SpecificationName: "Пижама женская",
                    Revenue: 55000m
                )
            }
        );

    // -------------------------
    //  PRODUCTION COST BY BATCH
    // -------------------------
    [HttpGet("production-cost")]
    [SwaggerResponseExample(200, typeof(ReportsProductionCostResponseExample))]
    [ProducesResponseType(typeof(IEnumerable<ReportsProductionCostResponse>), StatusCodes.Status200OK)]
    public IActionResult ProductionCost([FromQuery] int month = 11, int year = 2025)
        => Ok(
            new[]
            {
                new ReportsProductionCostResponse(
                    ProductionBatchId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    Cost: 78500m
                )
            }
        );
}
