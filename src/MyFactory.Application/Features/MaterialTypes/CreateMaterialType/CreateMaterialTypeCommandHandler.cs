using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Materials;

namespace MyFactory.Application.Features.MaterialTypes.CreateMaterialType;

public sealed class CreateMaterialTypeCommandHandler
    : IRequestHandler<CreateMaterialTypeCommand, Guid>
{
    private readonly IApplicationDbContext _db;

    public CreateMaterialTypeCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(CreateMaterialTypeCommand request, CancellationToken cancellationToken)
    {
        var exists = await _db.MaterialTypes
            .AsNoTracking()
            .AnyAsync(x => x.Name == request.Name, cancellationToken);

        if (exists)
            throw new DomainApplicationException("Material type with the same name already exists.");

        var entity = new MaterialTypeEntity(request.Name, request.Description);
        _db.MaterialTypes.Add(entity);

        await _db.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
