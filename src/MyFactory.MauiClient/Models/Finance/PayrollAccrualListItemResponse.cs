using MyFactory.MauiClient.Models.Common;

namespace MyFactory.MauiClient.Models.Finance;

public record PayrollAccrualListItemResponse(
    Guid EmployeeId,
    string EmployeeName,
    decimal TotalHours,
    decimal QtyPlanned,
    decimal QtyProduced,
    decimal QtyExtra,
    decimal BaseAmount,
    decimal PremiumAmount,
    decimal TotalAmount,
    decimal PaidAmount,
    decimal RemainingAmount) : ListItemResponse(EmployeeId, EmployeeName);
