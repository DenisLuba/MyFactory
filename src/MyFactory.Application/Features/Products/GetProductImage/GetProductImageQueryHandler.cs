using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Products;

namespace MyFactory.Application.Features.Products.GetProductImage;

public sealed class GetProductImageQueryHandler : IRequestHandler<GetProductImageQuery, ProductImageDto?>
{
    private readonly IApplicationDbContext _db;
    private readonly IFileStorage _fileStorage;

    public GetProductImageQueryHandler(IApplicationDbContext db, IFileStorage fileStorage)
    {
        _db = db;
        _fileStorage = fileStorage;
    }

    public async Task<ProductImageDto?> Handle(GetProductImageQuery request, CancellationToken cancellationToken)
    {
        var image = await _db.ProductImages
            .AsNoTracking()
            .FirstOrDefaultAsync(pi => pi.Id == request.ImageId, cancellationToken);

        if (image is null)
        {
            return null;
        }

        byte[]? content = null;
        var stream = await _fileStorage.GetAsync(image.Path, cancellationToken);
        if (stream is not null)
        {
            await using (stream)
            {
                using var ms = new MemoryStream();
                await stream.CopyToAsync(ms, cancellationToken);
                content = ms.ToArray();
            }
        }

        return new ProductImageDto
        {
            Id = image.Id,
            ProductId = image.ProductId,
            FileName = image.FileName,
            Path = image.Path,
            ContentType = image.ContentType,
            SortOrder = image.SortOrder,
            Content = content
        };
    }
}
