using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Features.Units.UpdateUnit;

public sealed class UpdateUnitCommandHandler : IRequestHandler<UpdateUnitCommand>
{
    private readonly IApplicationDbContext _db;

    public UpdateUnitCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(UpdateUnitCommand request, CancellationToken cancellationToken)
    {
        var unit = await _db.Units.FirstOrDefaultAsync(u => u.Id == request.UnitId, cancellationToken);

        if (unit is null)
            throw new NotFoundException($"Unit with Id {request.UnitId} not found");

        var codeInUse = await _db.Units
            .AsNoTracking()
            .AnyAsync(u => u.Id != request.UnitId && u.Code == request.Code, cancellationToken);

        if (codeInUse)
            throw new InvalidOperationException($"Unit with code '{request.Code}' already exists.");

        unit.Update(request.Code, request.Name);

        await _db.SaveChangesAsync(cancellationToken);
    }
}
