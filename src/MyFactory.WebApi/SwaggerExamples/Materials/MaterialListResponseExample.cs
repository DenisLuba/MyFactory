using MyFactory.WebApi.Contracts.Common;
using MyFactory.WebApi.Contracts.Materials;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Materials;

public sealed class MaterialListResponseExample : IExamplesProvider<ListResponse<MaterialListItemResponse>>
{
    public ListResponse<MaterialListItemResponse> GetExamples() => new (
        Items: 
        [
            new(
                Id: Guid.Parse("11111111-1111-1111-1111-111111111111"),
                MaterialType: "�����",
                Name: "�����",
                TotalQty: 150,
                UnitCode: "�"),
            new(
                Id: Guid.Parse("22222222-2222-2222-2222-222222222222"),
                MaterialType: "���������",
                Name: "������ 20 ��",
                TotalQty: 320,
                UnitCode: "��")
        ],
        TotalCount: 128,
        Take: 30,
        Skip: 0,
        HasMore: true);
}
