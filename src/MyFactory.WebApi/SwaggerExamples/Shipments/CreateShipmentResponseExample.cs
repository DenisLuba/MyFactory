using MyFactory.WebApi.Contracts.Shipments;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Shipments;

public sealed class CreateShipmentResponseExample : IExamplesProvider<CreateShipmentResponse>
{
    public CreateShipmentResponse GetExamples() => new(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));
}
