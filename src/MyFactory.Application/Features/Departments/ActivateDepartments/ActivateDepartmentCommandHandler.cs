using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Organization;

namespace MyFactory.Application.Features.Departments.ActivateDepartments;

public sealed class ActivateDepartmentCommandHandler
    : IRequestHandler<ActivateDepartmentCommand>
{
    private readonly IApplicationDbContext _db;

    public ActivateDepartmentCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(
        ActivateDepartmentCommand request,
        CancellationToken cancellationToken)
    {
        var department = await _db.Departments
            .FirstOrDefaultAsync(x => x.Id == request.DepartmentId, cancellationToken)
            ?? throw new NotFoundException("Department not found");

        if (department.IsActive)
            return;

        department.Activate();

        await _db.SaveChangesAsync(cancellationToken);
    }
}