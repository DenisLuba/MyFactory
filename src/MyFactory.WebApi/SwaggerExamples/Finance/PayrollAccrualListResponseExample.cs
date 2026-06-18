using MyFactory.WebApi.Contracts.Common;
using MyFactory.WebApi.Contracts.Finance;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Finance;

public sealed class PayrollAccrualListResponseExample : IExamplesProvider<ListResponse<PayrollAccrualListItemResponse>>
{
    public ListResponse<PayrollAccrualListItemResponse> GetExamples() => new(
        Items:
        [
            new(
                EmployeeId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
                EmployeeName: "������ ����",
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
                EmployeeName: "������ ����",
                TotalHours: 152,
                QtyPlanned: 300,
                QtyProduced: 295,
                QtyExtra: 0,
                BaseAmount: 30400,
                PremiumAmount: 0,
                TotalAmount: 30400,
                PaidAmount: 10000,
                RemainingAmount: 20400)
        ],
        TotalCount: 2,
        Take: 30,
        Skip: 0,
        HasMore: false
    );
}
