using MyFactory.WebApi.Contracts.Common;
using MyFactory.WebApi.Contracts.Employees;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Employees;

public sealed class EmployeeListResponseExample : IExamplesProvider<ListResponse<EmployeeListItemResponse>>
{
    public ListResponse<EmployeeListItemResponse> GetExamples() => new 
    (
        [
            new(
                Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
                FullName: "������ ���� ��������",
                DepartmentName: "������� ���",
                PositionName: "����",
                IsActive: true),
            new(
                Id: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"),
                FullName: "������ ���� ��������",
                DepartmentName: "�������",
                PositionName: "���������",
                IsActive: false)
        ],
        TotalCount: 128,
        Take: 30,
        Skip: 0,
        HasMore: true
    );
}
