namespace MyFactory.MauiClient.Models.Finance;

public record CreatePayrollPaymentRequest(
    Guid EmployeeId,
    DateOnly PaymentDate,
    decimal Amount);
