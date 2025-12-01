using MyFactory.WebApi.Contracts.MaterialTransfers;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.MaterialTransfers;

public class MaterialTransfersForOrderExample : IExamplesProvider<IEnumerable<MaterialTransferItemDto>>
{
    public IEnumerable<MaterialTransferItemDto> GetExamples() =>
    [
        new(Guid.Parse("11111111-1111-1111-1111-111111111111"), "Ткань Ситец", 50, "м", 180, 9000),
        new(Guid.Parse("22222222-2222-2222-2222-222222222222"), "Молния 20 см", 200, "шт", 12, 2400)
    ];
}
