using MediatR;

namespace MyFactory.Application.Features.Finance.CreatePayrollPayment;

public sealed record CreatePayrollPaymentCommand(
    Guid EmployeeId,
    DateOnly PaymentDate,
    decimal Amount
) : IRequest<Guid>;

