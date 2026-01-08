using MyFactory.WebApi.Contracts.Units;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Units;

public sealed class GetUnitsResponseExample : IExamplesProvider<IEnumerable<UnitResponse>>
{
    public IEnumerable<UnitResponse> GetExamples() => new[]
    {
        new UnitResponse(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"), "pcs", "Pieces"),
        new UnitResponse(Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"), "m", "Meters")
    };
}
