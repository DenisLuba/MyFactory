using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Specifications;

namespace MyFactory.WebApi.SwaggerExamples.Specifications;

public class SpecificationsCostResponseExample : IExamplesProvider<SpecificationsCostResponse>
{
    public SpecificationsCostResponse GetExamples() =>
        new(
            SpecificationId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            AsOf: new DateTime(2025, 11, 20),
            MaterialsCost: 336,
            OperationsCost: 68,
            WorkshopExpenses: 40,
            TotalCost: 444
        );
}

