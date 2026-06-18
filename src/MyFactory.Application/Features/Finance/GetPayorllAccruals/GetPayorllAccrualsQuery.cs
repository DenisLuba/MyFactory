using MediatR;
using MyFactory.Application.DTOs.Common;
using MyFactory.Application.DTOs.Finance;

namespace MyFactory.Application.Features.Finance.GetPayrollAccruals;

public sealed record GetPayrollAccrualsQuery(
    DateOnly From,
    DateOnly To,
    Guid? EmployeeId = null,
    Guid? DepartmentId = null,
    string? SortBy = null,
    bool SortDesc = false,
    int Skip = 0,
    int Take = 30
) : IRequest<ListDto<PayrollAccrualListItemDto>>;
