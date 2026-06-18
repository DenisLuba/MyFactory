using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Features.Suppliers.DeleteSupplier;

public sealed class DeleteSupplierCommandHandler
    : IRequestHandler<DeleteSupplierCommand>
{
    private readonly IApplicationDbContext _db;

    public DeleteSupplierCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(
        DeleteSupplierCommand request,
        CancellationToken cancellationToken)
    {
        var supplier = await _db.Suppliers
            .FirstOrDefaultAsync(s => s.Id == request.SupplierId, cancellationToken);

        if (supplier is null)
            throw new NotFoundException("Supplier not found");

        supplier.Deactivate(); // метод из ActivatableEntity

        await _db.SaveChangesAsync(cancellationToken);
    }
}
