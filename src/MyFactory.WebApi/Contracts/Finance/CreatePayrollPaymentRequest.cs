namespace MyFactory.WebApi.Contracts.Finance;

public record CreatePayrollPaymentRequest(
    Guid EmployeeId,
    DateOnly PaymentDate,
    decimal Amount);
