using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Organization;

namespace MyFactory.Application.Features.Departments.CreateDepartment;
public sealed class CreateDepartmentCommandHandler
    : IRequestHandler<CreateDepartmentCommand, Guid>
{
    private readonly IApplicationDbContext _db;

    public CreateDepartmentCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(
        CreateDepartmentCommand request,
        CancellationToken cancellationToken)
    {
        var codeExists = await _db.Departments
            .AnyAsync(x => x.Code == request.Code, cancellationToken);

        if (codeExists)
            throw new DomainException("Department with the same code already exists.");

        var department = new DepartmentEntity(
            request.Name,
            request.Type);

        department.SetCode(request.Code); 
        department.Activate();

        _db.Departments.Add(department);
        await _db.SaveChangesAsync(cancellationToken);

        return department.Id;
    }
}