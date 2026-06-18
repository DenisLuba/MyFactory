using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Features.Materials.DeleteMaterialImage;

public sealed class DeleteMaterialImageCommandHandler : IRequestHandler<DeleteMaterialImageCommand>
{
    private readonly IApplicationDbContext _db;
    private readonly IFileStorage _fileStorage;

    public DeleteMaterialImageCommandHandler(IApplicationDbContext db, IFileStorage fileStorage)
    {
        _db = db;
        _fileStorage = fileStorage;
    }

    public async Task Handle(DeleteMaterialImageCommand request, CancellationToken cancellationToken)
    {
        var image = await _db.MaterialImages
            .FirstOrDefaultAsync(mi => mi.Id == request.ImageId, cancellationToken) ?? throw new NotFoundException($"Image {request.ImageId} not found");
        
        var path = image.Path;

        _db.MaterialImages.Remove(image);
        await _db.SaveChangesAsync(cancellationToken);

        await _fileStorage.DeleteAsync(path, cancellationToken);
    }
}
