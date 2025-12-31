using MediatR;
using MyFactory.Application.DTOs.Finance;

namespace MyFactory.Application.Features.Finance.GetPayrollAccruals;

public sealed record GetPayrollAccrualsQuery(
    DateOnly From,
    DateOnly To,
    Guid? EmployeeId,
    Guid? DepartmentId
) : IRequest<IReadOnlyList<PayrollAccrualListItemDto>>;
