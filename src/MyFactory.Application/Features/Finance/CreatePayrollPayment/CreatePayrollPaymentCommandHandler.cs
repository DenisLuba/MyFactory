using MediatR;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Finance;

namespace MyFactory.Application.Features.Finance.CreatePayrollPayment;

public sealed class CreatePayrollPaymentCommandHandler
    : IRequestHandler<CreatePayrollPaymentCommand, Guid>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public CreatePayrollPaymentCommandHandler(
        IApplicationDbContext db,
        ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(
        CreatePayrollPaymentCommand request,
        CancellationToken cancellationToken)
    {
        var payment = new PayrollPaymentEntity(
            request.EmployeeId,
            request.PaymentDate,
            request.Amount,
            _currentUser.UserId);

        _db.PayrollPayments.Add(payment);
        await _db.SaveChangesAsync(cancellationToken);

        return payment.Id;
    }
}
