using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Features.Products.RemoveProductMaterial;

public sealed class RemoveProductMaterialCommandHandler
    : IRequestHandler<RemoveProductMaterialCommand>
{
    private readonly IApplicationDbContext _db;

    public RemoveProductMaterialCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(
        RemoveProductMaterialCommand request,
        CancellationToken cancellationToken)
    {
        var productMaterial = await _db.ProductMaterials
            .FirstOrDefaultAsync(
                x => x.Id == request.ProductMaterialId,
                cancellationToken)
            ?? throw new NotFoundException("Product material not found");

        _db.ProductMaterials.Remove(productMaterial);

        await _db.SaveChangesAsync(cancellationToken);
    }
}
