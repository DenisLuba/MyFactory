using MyFactory.WebApi.Contracts.Positions;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Positions;

public sealed class PositionListResponseExample : IExamplesProvider<IReadOnlyList<PositionListItemResponse>>
{
    public IReadOnlyList<PositionListItemResponse> GetExamples() => new List<PositionListItemResponse>
    {
        new(
            Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
            Name: "Швея",
            Code: "SHW",
            DepartmentId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            DepartmentName: "Швейный цех",
            IsActive: true),
        new(
            Id: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"),
            Name: "Закройщик",
            Code: "CUT",
            DepartmentId: Guid.Parse("22222222-2222-2222-2222-222222222222"),
            DepartmentName: "Раскрой",
            IsActive: true)
    };
}
