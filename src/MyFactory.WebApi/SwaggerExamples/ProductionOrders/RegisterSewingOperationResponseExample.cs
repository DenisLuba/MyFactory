using MyFactory.WebApi.Contracts.ProductionOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.ProductionOrders;

public sealed class RegisterSewingOperationResponseExample : IExamplesProvider<RegisterSewingOperationResponse>
{
    public RegisterSewingOperationResponse GetExamples() => new(
        Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0001"));
}
