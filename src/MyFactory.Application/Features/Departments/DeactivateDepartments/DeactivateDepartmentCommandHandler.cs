using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Organization;

namespace MyFactory.Application.Features.Departments.DeactivateDepartments;

public sealed class DeactivateDepartmentCommandHandler
    : IRequestHandler<DeactivateDepartmentCommand>
{
    private readonly IApplicationDbContext _db;

    public DeactivateDepartmentCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(
        DeactivateDepartmentCommand request,
        CancellationToken cancellationToken)
    {
        var department = await _db.Departments
            .FirstOrDefaultAsync(x => x.Id == request.DepartmentId, cancellationToken)
            ?? throw new NotFoundException("Department not found");

        if (!department.IsActive)
            return;

        department.Deactivate();

        await _db.SaveChangesAsync(cancellationToken);
    }
}