using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.ProductTypes;

namespace MyFactory.Application.Features.ProductTypes.GetProductTypeById;

public sealed class GetProductTypeByIdQueryHandler
    : IRequestHandler<GetProductTypeByIdQuery, ProductTypeDto?>
{
    private readonly IApplicationDbContext _db;

    public GetProductTypeByIdQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<ProductTypeDto?> Handle(GetProductTypeByIdQuery request, CancellationToken cancellationToken)
    {
        var productType = await _db.ProductTypes
            .AsNoTracking()
            .Where(pt => pt.Id == request.ProductTypeId)
            .Select(pt => new ProductTypeDto(pt.Id, pt.Type, pt.Description))
            .FirstOrDefaultAsync(cancellationToken);

        return productType;
    }
}
