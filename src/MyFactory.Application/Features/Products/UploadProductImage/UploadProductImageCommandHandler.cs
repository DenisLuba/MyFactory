using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Products;

namespace MyFactory.Application.Features.Products.UploadProductImage;

public sealed class UploadProductImageCommandHandler : IRequestHandler<UploadProductImageCommand, Guid>
{
    private readonly IApplicationDbContext _db;
    private readonly IFileStorage _fileStorage;

    public UploadProductImageCommandHandler(IApplicationDbContext db, IFileStorage fileStorage)
    {
        _db = db;
        _fileStorage = fileStorage;
    }

    public async Task<Guid> Handle(UploadProductImageCommand request, CancellationToken cancellationToken)
    {
        var productExists = await _db.Products.AnyAsync(p => p.Id == request.ProductId, cancellationToken);
        if (!productExists)
        {
            throw new NotFoundException($"Product {request.ProductId} not found");
        }

        await using var contentStream = new MemoryStream(request.Content);
        var path = await _fileStorage.SaveAsync(request.FileName, contentStream, cancellationToken);

        var nextSort = await _db.ProductImages
            .Where(pi => pi.ProductId == request.ProductId)
            .Select(pi => (int?)pi.SortOrder)
            .MaxAsync(cancellationToken) ?? 0;

        var image = new ProductImageEntity(request.ProductId, request.FileName, path, request.ContentType, nextSort + 1);

        _db.ProductImages.Add(image);
        await _db.SaveChangesAsync(cancellationToken);

        return image.Id;
    }
}
