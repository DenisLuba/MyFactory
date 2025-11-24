using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Contracts.Production;

public class ProductionCreateOrderRequestExample : IExamplesProvider<ProductionCreateOrderRequest>
{
    public ProductionCreateOrderRequest GetExamples() =>
        new ProductionCreateOrderRequest(
            SpecificationId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            Quantity: 10
        );
}

