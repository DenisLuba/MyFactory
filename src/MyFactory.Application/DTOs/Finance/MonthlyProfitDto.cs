using System;
using System.Collections.Generic;
using System.Linq;
using MyFactory.Domain.Entities.Reports;

namespace MyFactory.Application.DTOs.Finance;

public sealed record MonthlyProfitDto(
    Guid Id,
    int PeriodMonth,
    int PeriodYear,
    decimal Revenue,
    decimal ProductionCost,
    decimal Overhead,
    decimal Profit,
    IReadOnlyCollection<Guid> RevenueReportIds)
{
    public static MonthlyProfitDto FromEntity(MonthlyProfit profit)
    {
        var revenueReportIds = profit.RevenueReports.Select(report => report.Id).ToArray();

        return new MonthlyProfitDto(
            profit.Id,
            profit.PeriodMonth,
            profit.PeriodYear,
            profit.Revenue,
            profit.ProductionCost,
            profit.Overhead,
            profit.Profit,
            revenueReportIds);
    }
}
