using MediatR;
using MyFactory.Application.DTOs.Common;
using MyFactory.Application.DTOs.Employees;

namespace MyFactory.Application.Features.Employees.GetEmployee;

public sealed record GetEmployeesQuery(
    string? FullName = null,
    string? SortBy = null,
    bool SortDesc = false,
    int Skip = 0,
    int Take = 30,
    bool? IsActive = null,
    Guid? DepartmentId = null,
    Guid? PositionId = null,
    int? Grade = null,
    DateTime? HiredFrom = null,
    DateTime? HiredTo = null,
    bool? CanCut = null,
    bool? CanSew = null,
    bool? CanPackage = null,
    ICollection<Guid>? ExceptEmployeeIds = null
) : IRequest<ListDto<EmployeeListItemDto>>;
