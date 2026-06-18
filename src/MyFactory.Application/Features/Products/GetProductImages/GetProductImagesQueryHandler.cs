using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Products;

namespace MyFactory.Application.Features.Products.GetProductImages;

public sealed class GetProductImagesQueryHandler : IRequestHandler<GetProductImagesQuery, IReadOnlyList<ProductImageDto>>
{
    private readonly IApplicationDbContext _db;

    public GetProductImagesQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<ProductImageDto>> Handle(GetProductImagesQuery request, CancellationToken cancellationToken)
    {
        return await _db.ProductImages
            .AsNoTracking()
            .Where(pi => pi.ProductId == request.ProductId)
            .OrderBy(pi => pi.SortOrder)
            .ThenBy(pi => pi.CreatedAt)
            .Select(pi => new ProductImageDto
            {
                Id = pi.Id,
                ProductId = pi.ProductId,
                FileName = pi.FileName,
                Path = pi.Path,
                ContentType = pi.ContentType,
                SortOrder = pi.SortOrder,
                Content = null
            })
            .ToListAsync(cancellationToken);
    }
}
