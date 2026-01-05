namespace MyFactory.WebApi.Contracts.Finance;

public record CalculateDailyPayrollAccrualRequest(Guid EmployeeId, DateOnly Date);
