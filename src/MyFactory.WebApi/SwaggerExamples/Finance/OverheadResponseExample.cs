using MyFactory.WebApi.Contracts.Finance;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Finance;

public class OverheadResponseExample : IExamplesProvider<OverheadResponse>
{
    public OverheadResponse GetExamples() =>
        new(
            ExpenseTypeId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Amount: 120000.00m,
            Period: "11.2025"
        );
}

