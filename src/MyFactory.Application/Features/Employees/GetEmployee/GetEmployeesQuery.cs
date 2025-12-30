using MediatR;
using MyFactory.Application.DTOs.Employees;

namespace MyFactory.Application.Features.Organization.Employees.GetEmployee;

public sealed class GetEmployeesQuery : IRequest<IReadOnlyList<EmployeeListItemDto>>
{
    public string? Search { get; init; }
    public EmployeeSortBy SortBy { get; init; } = EmployeeSortBy.FullName;
    public bool SortDesc { get; init; } = false;
}

public enum EmployeeSortBy
{
    FullName,
    Department
}
