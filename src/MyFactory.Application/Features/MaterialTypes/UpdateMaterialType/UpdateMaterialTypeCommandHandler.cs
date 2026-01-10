using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Features.MaterialTypes.UpdateMaterialType;

public sealed class UpdateMaterialTypeCommandHandler : IRequestHandler<UpdateMaterialTypeCommand>
{
    private readonly IApplicationDbContext _db;

    public UpdateMaterialTypeCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(UpdateMaterialTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _db.MaterialTypes
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException("Material type not found");

        var nameTaken = await _db.MaterialTypes
            .AsNoTracking()
            .AnyAsync(x => x.Id != request.Id && x.Name == request.Name, cancellationToken);

        if (nameTaken)
            throw new DomainApplicationException("Material type with the same name already exists.");

        entity.Update(request.Name, request.Description);

        await _db.SaveChangesAsync(cancellationToken);
    }
}
