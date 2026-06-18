namespace MyFactory.Application.DTOs.Finance;

public sealed class EmployeePayrollAccrualDailyDto
{
    public Guid AccrualId { get; init; }
    public DateOnly Date { get; init; }

    public decimal HoursWorked { get; init; }

    public decimal QtyPlanned { get; init; }
    public decimal QtyProduced { get; init; }
    public decimal QtyExtra { get; init; }

    public decimal BaseAmount { get; init; }
    public decimal PremiumAmount { get; init; }
    public decimal TotalAmount { get; init; }
}
