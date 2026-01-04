using MyFactory.WebApi.Contracts.Advances;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Advances;

public sealed class CreateCashAdvanceReturnRequestExample : IExamplesProvider<CreateCashAdvanceReturnRequest>
{
    public CreateCashAdvanceReturnRequest GetExamples() => new(
        ReturnDate: new DateOnly(2025, 3, 25),
        Amount: 1000m,
        Description: "Возврат остатка");
}
