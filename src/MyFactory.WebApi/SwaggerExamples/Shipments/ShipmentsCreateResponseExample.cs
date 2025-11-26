using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Shipments;

namespace MyFactory.WebApi.SwaggerExamples.Shipments;

public class ShipmentsCreateResponseExample : IExamplesProvider<ShipmentsCreateResponse>
{
    public ShipmentsCreateResponse GetExamples() =>
        new(
            ShipmentId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Status: ShipmentsStatus.Created
        );
}

