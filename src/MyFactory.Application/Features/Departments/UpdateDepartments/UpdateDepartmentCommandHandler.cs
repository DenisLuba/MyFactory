using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Organization;

namespace MyFactory.Application.Features.Departments.UpdateDepartment;

public sealed class UpdateDepartmentCommandHandler
    : IRequestHandler<UpdateDepartmentCommand>
{
    private readonly IApplicationDbContext _db;

    public UpdateDepartmentCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(
        UpdateDepartmentCommand request,
        CancellationToken cancellationToken)
    {
        var department = await _db.Departments
            .FirstOrDefaultAsync(x => x.Id == request.DepartmentId, cancellationToken)
            ?? throw new NotFoundException("Department not found");

        if (!string.IsNullOrWhiteSpace(request.Code))
        {
            var codeExists = await _db.Departments
                .AnyAsync(x =>
                    x.Id != request.DepartmentId &&
                    x.Code == request.Code,
                    cancellationToken);

            if (codeExists)
                throw new DomainApplicationException("Department with the same code already exists.");
        }

        department.Update(
            request.Name,
            request.Type);

        department.SetCode(request.Code);

        if (request.IsActive)
            department.Activate();
        else
            department.Deactivate();

        await _db.SaveChangesAsync(cancellationToken);
    }
}