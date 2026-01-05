using MyFactory.WebApi.Contracts.Materials;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Materials;

public sealed class MaterialListResponseExample : IExamplesProvider<IReadOnlyList<MaterialListItemResponse>>
{
    public IReadOnlyList<MaterialListItemResponse> GetExamples() => new List<MaterialListItemResponse>
    {
        new(
            Id: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            MaterialType: "Ткань",
            Name: "Ситец",
            TotalQty: 150,
            UnitCode: "м"),
        new(
            Id: Guid.Parse("22222222-2222-2222-2222-222222222222"),
            MaterialType: "Фурнитура",
            Name: "Молния 20 см",
            TotalQty: 320,
            UnitCode: "шт")
    };
}
