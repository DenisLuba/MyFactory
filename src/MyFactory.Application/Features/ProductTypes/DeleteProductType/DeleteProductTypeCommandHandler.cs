using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Features.ProductTypes.DeleteProductType;

public sealed class DeleteProductTypeCommandHandler
    : IRequestHandler<DeleteProductTypeCommand>
{
    private readonly IApplicationDbContext _db;

    public DeleteProductTypeCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(DeleteProductTypeCommand request, CancellationToken cancellationToken)
    {
        var productType = await _db.ProductTypes
            .FirstOrDefaultAsync(pt => pt.Id == request.ProductTypeId, cancellationToken)
            ?? throw new NotFoundException("Product type not found");

        _db.ProductTypes.Remove(productType);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
