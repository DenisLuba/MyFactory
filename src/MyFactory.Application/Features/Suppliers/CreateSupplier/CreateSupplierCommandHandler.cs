using MediatR;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Materials;

namespace MyFactory.Application.Features.Suppliers.CreateSupplier;

public sealed class CreateSupplierCommandHandler
    : IRequestHandler<CreateSupplierCommand, Guid>
{
    private readonly IApplicationDbContext _db;

    public CreateSupplierCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(
        CreateSupplierCommand request,
        CancellationToken cancellationToken)
    {
        var supplier = new SupplierEntity(
            request.Name,
            request.Description);

        _db.Suppliers.Add(supplier);

        await _db.SaveChangesAsync(cancellationToken);

        return supplier.Id;
    }
}
