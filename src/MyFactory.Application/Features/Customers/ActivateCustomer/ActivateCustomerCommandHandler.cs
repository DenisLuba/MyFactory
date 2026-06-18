using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Features.Customers.ActivateCustomer;

public sealed class ActivateCustomerCommandHandler : IRequestHandler<ActivateCustomerCommand>
{
    private readonly IApplicationDbContext _db;

    public ActivateCustomerCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(ActivateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _db.Customers.FirstOrDefaultAsync(x => x.Id == request.CustomerId, cancellationToken)
            ?? throw new NotFoundException("Customer not found");
        if (customer.IsActive)
            return;
        customer.Activate();
        await _db.SaveChangesAsync(cancellationToken);
    }
}
