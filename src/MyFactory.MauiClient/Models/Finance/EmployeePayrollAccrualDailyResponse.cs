namespace MyFactory.MauiClient.Models.Finance;

public record EmployeePayrollAccrualDailyResponse(
    Guid AccrualId,
    DateOnly Date,
    decimal HoursWorked,
    decimal QtyPlanned,
    decimal QtyProduced,
    decimal QtyExtra,
    decimal BaseAmount,
    decimal PremiumAmount,
    decimal TotalAmount);
