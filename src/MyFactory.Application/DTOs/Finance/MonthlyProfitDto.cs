using MyFactory.Domain.Entities.Finance;

namespace MyFactory.Application.DTOs.Finance;

public sealed record MonthlyProfitDto(
    int PeriodMonth,
    int PeriodYear,
    decimal Revenue,
    decimal ProductionCost,
    decimal Overhead,
    decimal Profit)
{
    public static MonthlyProfitDto FromEntity(MonthlyProfit profit)
    {
        return new MonthlyProfitDto(
            profit.PeriodMonth,
            profit.PeriodYear,
            profit.Revenue,
            profit.ProductionCost,
            profit.Overhead,
            profit.Profit);
    }
}
