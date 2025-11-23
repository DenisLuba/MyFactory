namespace MyFactory.WebApi.Contracts.Payroll;

public record PayrollPayRequest(
    Guid EmployeeId,
    decimal Amount,
    DateTime Date
);
