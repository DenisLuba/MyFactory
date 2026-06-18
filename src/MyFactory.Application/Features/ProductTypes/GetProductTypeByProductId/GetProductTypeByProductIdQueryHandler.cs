using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.ProductTypes;

namespace MyFactory.Application.Features.ProductTypes.GetProductTypeByProductId;

public sealed class GetProductTypeByProductIdQueryHandler
    : IRequestHandler<GetProductTypeByProductIdQuery, ProductTypeDto?>
{
    private readonly IApplicationDbContext _db;

    public GetProductTypeByProductIdQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<ProductTypeDto?> Handle(GetProductTypeByProductIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _db.Products
            .AsNoTracking()
            .Include(p => p.ProductType)
            .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken)
            ?? throw new NotFoundException("Product not found");

        if (product.ProductType is null)
        {
            return null;
        }

        return new ProductTypeDto(product.ProductType.Id, product.ProductType.Type, product.ProductType.Description);
    }
}
