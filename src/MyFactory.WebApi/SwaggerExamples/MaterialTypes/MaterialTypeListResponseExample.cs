using MyFactory.WebApi.Contracts.MaterialTypes;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.MaterialTypes;

public sealed class MaterialTypeListResponseExample : IExamplesProvider<IReadOnlyList<MaterialTypeResponse>>
{
    public IReadOnlyList<MaterialTypeResponse> GetExamples() => new List<MaterialTypeResponse>
    {
        new(
            Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
            Name: "Ткань",
            Description: "Ткани и полотна"),
        new(
            Id: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"),
            Name: "Фурнитура",
            Description: "Молнии, кнопки, пуговицы")
    };
}
