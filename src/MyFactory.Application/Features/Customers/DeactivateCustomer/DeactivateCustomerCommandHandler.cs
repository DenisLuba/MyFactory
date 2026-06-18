using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Features.Customers.DeactivateCustomer;

public sealed class DeactivateCustomerCommandHandler : IRequestHandler<DeactivateCustomerCommand>
{
    private readonly IApplicationDbContext _db;

    public DeactivateCustomerCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(DeactivateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _db.Customers.FirstOrDefaultAsync(x => x.Id == request.CustomerId, cancellationToken)
            ?? throw new NotFoundException("Customer not found");
        if (!customer.IsActive)
            return;
        customer.Deactivate();
        await _db.SaveChangesAsync(cancellationToken);
    }
}
