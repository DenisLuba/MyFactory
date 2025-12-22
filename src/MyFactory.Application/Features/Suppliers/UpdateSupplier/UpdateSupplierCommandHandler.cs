using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Features.Suppliers.UpdateSupplier;

public sealed class UpdateSupplierCommandHandler
    : IRequestHandler<UpdateSupplierCommand>
{
    private readonly IApplicationDbContext _db;

    public UpdateSupplierCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(
        UpdateSupplierCommand request,
        CancellationToken cancellationToken)
    {
        var supplier = await _db.Suppliers
            .FirstOrDefaultAsync(
                s => s.Id == request.SupplierId,
                cancellationToken);

        if (supplier is null)
            throw new NotFoundException(
                $"Supplier with Id {request.SupplierId} not found");

        if (!supplier.IsActive)
            throw new BusinessRuleViolationException(
                "Cannot edit inactive supplier");

        supplier.Update(
            name: request.Name,
            description: request.Description
        );

        await _db.SaveChangesAsync(cancellationToken);
    }
}
