namespace MyFactory.MauiClient.Models.Finance;

public record CalculateDailyPayrollAccrualRequest(Guid EmployeeId, DateOnly Date);
