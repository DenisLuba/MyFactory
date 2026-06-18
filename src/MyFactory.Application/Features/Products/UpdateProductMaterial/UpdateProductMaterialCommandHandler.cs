using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Features.Products.UpdateProductMaterial;

public sealed class UpdateProductMaterialCommandHandler
    : IRequestHandler<UpdateProductMaterialCommand>
{
    private readonly IApplicationDbContext _db;

    public UpdateProductMaterialCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(
        UpdateProductMaterialCommand request,
        CancellationToken cancellationToken)
    {
        var material = await _db.ProductMaterials
            .FirstOrDefaultAsync(x => x.Id == request.ProductMaterialId, cancellationToken)
            ?? throw new NotFoundException("Product material not found");

        material.UpdateQty(request.QtyPerUnit);

        await _db.SaveChangesAsync(cancellationToken);
    }
}
