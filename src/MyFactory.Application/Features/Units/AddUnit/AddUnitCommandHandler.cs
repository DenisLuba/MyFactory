using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Materials;

namespace MyFactory.Application.Features.Units.AddUnit;

public sealed class AddUnitCommandHandler : IRequestHandler<AddUnitCommand, Guid>
{
    private readonly IApplicationDbContext _db;

    public AddUnitCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(AddUnitCommand request, CancellationToken cancellationToken)
    {
        var exists = await _db.Units
            .AsNoTracking()
            .AnyAsync(u => u.Code == request.Code, cancellationToken);

        if (exists)
            throw new InvalidOperationException($"Unit with code '{request.Code}' already exists.");

        var unit = new UnitEntity(request.Code, request.Name);

        await _db.Units.AddAsync(unit, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        return unit.Id;
    }
}
