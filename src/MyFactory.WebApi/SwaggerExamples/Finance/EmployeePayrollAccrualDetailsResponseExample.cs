using MyFactory.WebApi.Contracts.Finance;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Finance;

public sealed class EmployeePayrollAccrualDetailsResponseExample : IExamplesProvider<EmployeePayrollAccrualDetailsResponse>
{
    public EmployeePayrollAccrualDetailsResponse GetExamples() => new(
        EmployeeId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
        EmployeeName: "Иванов Иван",
        PositionName: "Швея",
        Period: new YearMonthResponse(2025, 3),
        TotalBaseAmount: 32800,
        TotalPremiumAmount: 5200,
        TotalAmount: 38000,
        PaidAmount: 15000,
        RemainingAmount: 23000,
        Days: new List<EmployeePayrollAccrualDailyResponse>
        {
            new(
                AccrualId: Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccc0003"),
                Date: new DateOnly(2025, 3, 1),
                HoursWorked: 8,
                QtyPlanned: 16,
                QtyProduced: 18,
                QtyExtra: 2,
                BaseAmount: 1600,
                PremiumAmount: 320,
                TotalAmount: 1920),
            new(
                AccrualId: Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddd0004"),
                Date: new DateOnly(2025, 3, 2),
                HoursWorked: 8,
                QtyPlanned: 16,
                QtyProduced: 16,
                QtyExtra: 0,
                BaseAmount: 1600,
                PremiumAmount: 0,
                TotalAmount: 1600)
        });
}
