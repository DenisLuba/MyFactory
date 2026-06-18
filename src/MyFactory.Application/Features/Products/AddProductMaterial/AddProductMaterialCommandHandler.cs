using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Products;

namespace MyFactory.Application.Features.Products.AddProductMaterial;

public sealed class AddProductMaterialCommandHandler
    : IRequestHandler<AddProductMaterialCommand, Guid>
{
    private readonly IApplicationDbContext _db;

    public AddProductMaterialCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(
        AddProductMaterialCommand request,
        CancellationToken cancellationToken)
    {
        var exists = await _db.ProductMaterials.AnyAsync(
            x => x.ProductId == request.ProductId &&
                 x.MaterialId == request.MaterialId,
            cancellationToken);

        if (exists)
            throw new ValidationException("Material already added");

        var entity = new ProductMaterialEntity(
            request.ProductId,
            request.MaterialId,
            request.QtyPerUnit
        );

        _db.ProductMaterials.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
