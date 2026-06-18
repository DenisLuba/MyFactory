using MyFactory.Application.Common.ValueObjects;

namespace MyFactory.Application.DTOs.Finance;

public sealed class EmployeePayrollAccrualDetailsDto
{
    public Guid EmployeeId { get; init; }
    public string EmployeeName { get; init; } = null!;
    public string PositionName { get; init; } = null!;

    public YearMonth Period { get; init; }

    public decimal TotalBaseAmount { get; init; }
    public decimal TotalPremiumAmount { get; init; }
    public decimal TotalAmount { get; init; }

    public decimal PaidAmount { get; init; }
    public decimal RemainingAmount { get; init; }

    public IReadOnlyList<EmployeePayrollAccrualDailyDto> Days { get; init; } = [];
}
