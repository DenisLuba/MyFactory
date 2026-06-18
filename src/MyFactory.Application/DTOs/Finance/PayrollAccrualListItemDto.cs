namespace MyFactory.Application.DTOs.Finance;

public sealed class PayrollAccrualListItemDto
{
    public Guid EmployeeId { get; init; }
    public string EmployeeName { get; init; } = null!;

    public decimal TotalHours { get; init; }

    public decimal QtyPlanned { get; init; }
    public decimal QtyProduced { get; init; }
    public decimal QtyExtra { get; init; }

    public decimal BaseAmount { get; init; }
    public decimal PremiumAmount { get; init; }
    public decimal TotalAmount { get; init; }

    public decimal PaidAmount { get; init; }
    public decimal RemainingAmount { get; init; }
}