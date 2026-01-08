using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Features.Units.RemoveUnit;

public sealed class RemoveUnitCommandHandler : IRequestHandler<RemoveUnitCommand>
{
    private readonly IApplicationDbContext _db;

    public RemoveUnitCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(RemoveUnitCommand request, CancellationToken cancellationToken)
    {
        var unit = await _db.Units.FirstOrDefaultAsync(u => u.Id == request.UnitId, cancellationToken);

        if (unit is null)
            throw new NotFoundException($"Unit with Id {request.UnitId} not found");

        _db.Units.Remove(unit);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
