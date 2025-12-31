using MediatR;
using MyFactory.Application.Common.ValueObjects;
using MyFactory.Application.DTOs.Finance;

namespace MyFactory.Application.Features.Finance.GetEmployeePayrollAccruals;

public sealed record GetEmployeePayrollAccrualsQuery(
    Guid EmployeeId,
    YearMonth Period
) : IRequest<EmployeePayrollAccrualDetailsDto>;
