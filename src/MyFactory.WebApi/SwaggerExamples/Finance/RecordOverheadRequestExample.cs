using MyFactory.WebApi.Contracts.Finance;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Finance;

public class RecordOverheadRequestExample : IExamplesProvider<RecordOverheadRequest>
{
    public RecordOverheadRequest GetExamples() =>
        new(
            PeriodMonth: 11,
            PeriodYear: 2025,
            ExpenseTypeId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Amount: 120000.00m
        );
}

