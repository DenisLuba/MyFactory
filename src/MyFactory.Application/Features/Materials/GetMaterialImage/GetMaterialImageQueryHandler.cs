using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Materials;

namespace MyFactory.Application.Features.Materials.GetMaterialImage;

public sealed class GetMaterialImageQueryHandler : IRequestHandler<GetMaterialImageQuery, MaterialImageDto?>
{
    private readonly IApplicationDbContext _db;
    private readonly IFileStorage _fileStorage;

    public GetMaterialImageQueryHandler(IApplicationDbContext db, IFileStorage fileStorage)
    {
        _db = db;
        _fileStorage = fileStorage;
    }

    public async Task<MaterialImageDto?> Handle(GetMaterialImageQuery request, CancellationToken cancellationToken)
    {
        var image = await _db.MaterialImages
            .AsNoTracking()
            .FirstOrDefaultAsync(mi => mi.Id == request.ImageId, cancellationToken);

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

        return new MaterialImageDto
        {
            Id = image.Id,
            MaterialId = image.MaterialId,
            FileName = image.FileName,
            Path = image.Path,
            ContentType = image.ContentType,
            SortOrder = image.SortOrder,
            Content = content
        };
    }
}
