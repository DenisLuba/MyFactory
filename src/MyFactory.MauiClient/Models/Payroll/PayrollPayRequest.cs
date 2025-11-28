namespace MyFactory.MauiClient.Models.Payroll;

public record PayrollPayRequest(
    Guid EmployeeId,
    decimal Amount,
    DateTime Date);
