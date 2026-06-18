using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Materials;

namespace MyFactory.Application.Features.Materials.UploadMaterialImage;

public sealed class UploadMaterialImageCommandHandler : IRequestHandler<UploadMaterialImageCommand, Guid>
{
    private readonly IApplicationDbContext _db;
    private readonly IFileStorage _fileStorage;

    public UploadMaterialImageCommandHandler(IApplicationDbContext db, IFileStorage fileStorage)
    {
        _db = db;
        _fileStorage = fileStorage;
    }

    public async Task<Guid> Handle(UploadMaterialImageCommand request, CancellationToken cancellationToken)
    {
        var materialExists = await _db.Materials.AnyAsync(m => m.Id == request.MaterialId, cancellationToken);
        if (!materialExists)
        {
            throw new NotFoundException($"Material {request.MaterialId} not found");
        }

        await using var contentStream = new MemoryStream(request.Content);
        var path = await _fileStorage.SaveAsync(request.FileName, contentStream, cancellationToken);

        var nextSort = await _db.MaterialImages
            .Where(pi => pi.MaterialId == request.MaterialId)
            .Select(pi => (int?)pi.SortOrder)
            .MaxAsync(cancellationToken) ?? 0;

        var image = new MaterialImageEntity(request.MaterialId, request.FileName, path, request.ContentType, nextSort + 1);

        _db.MaterialImages.Add(image);
        await _db.SaveChangesAsync(cancellationToken);

        return image.Id;
    }
}
