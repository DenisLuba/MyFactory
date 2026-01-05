using MyFactory.WebApi.Contracts.Finance;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Finance;

public sealed class PayrollAccrualListResponseExample : IExamplesProvider<IReadOnlyList<PayrollAccrualListItemResponse>>
{
    public IReadOnlyList<PayrollAccrualListItemResponse> GetExamples() => new List<PayrollAccrualListItemResponse>
    {
        new(
            EmployeeId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
            EmployeeName: "Иванов Иван",
            TotalHours: 160,
            QtyPlanned: 320,
            QtyProduced: 340,
            QtyExtra: 20,
            BaseAmount: 32000,
            PremiumAmount: 4800,
            TotalAmount: 36800,
            PaidAmount: 15000,
            RemainingAmount: 21800),
        new(
            EmployeeId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"),
            EmployeeName: "Петров Петр",
            TotalHours: 152,
            QtyPlanned: 300,
            QtyProduced: 295,
            QtyExtra: 0,
            BaseAmount: 30400,
            PremiumAmount: 0,
            TotalAmount: 30400,
            PaidAmount: 10000,
            RemainingAmount: 20400)
    };
}
