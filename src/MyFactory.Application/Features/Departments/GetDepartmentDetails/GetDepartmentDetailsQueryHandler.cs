using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Departments;

namespace MyFactory.Application.Features.Departments.GetDepartmentDetails;

public sealed class GetDepartmentDetailsQueryHandler
    : IRequestHandler<GetDepartmentDetailsQuery, DepartmentDetailsDto>
{
    private readonly IApplicationDbContext _db;

    public GetDepartmentDetailsQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<DepartmentDetailsDto> Handle(
        GetDepartmentDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var department = await _db.Departments
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.DepartmentId, cancellationToken)
            ?? throw new NotFoundException("Department not found");

        return new DepartmentDetailsDto
        {
            Id = department.Id,
            Name = department.Name,
            Code = department.Code ?? string.Empty,
            Type = department.Type,
            IsActive = department.IsActive
        };
    }
}
