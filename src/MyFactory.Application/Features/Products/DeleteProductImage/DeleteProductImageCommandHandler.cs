using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Features.Products.DeleteProductImage;

public sealed class DeleteProductImageCommandHandler : IRequestHandler<DeleteProductImageCommand>
{
    private readonly IApplicationDbContext _db;
    private readonly IFileStorage _fileStorage;

    public DeleteProductImageCommandHandler(IApplicationDbContext db, IFileStorage fileStorage)
    {
        _db = db;
        _fileStorage = fileStorage;
    }

    public async Task Handle(DeleteProductImageCommand request, CancellationToken cancellationToken)
    {
        var image = await _db.ProductImages
            .FirstOrDefaultAsync(pi => pi.Id == request.ImageId, cancellationToken);

        if (image is null)
        {
            throw new NotFoundException($"Image {request.ImageId} not found");
        }

        var path = image.Path;

        _db.ProductImages.Remove(image);
        await _db.SaveChangesAsync(cancellationToken);

        await _fileStorage.DeleteAsync(path, cancellationToken);
    }
}
