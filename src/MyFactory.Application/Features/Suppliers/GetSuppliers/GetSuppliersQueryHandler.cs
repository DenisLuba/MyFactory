using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Suppliers;

namespace MyFactory.Application.Features.Suppliers.GetSuppliers;

public sealed class GetSuppliersQueryHandler
    : IRequestHandler<GetSuppliersQuery, IReadOnlyList<SupplierListItemDto>>
{
    private readonly IApplicationDbContext _db;

    public GetSuppliersQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<SupplierListItemDto>> Handle(
        GetSuppliersQuery request,
        CancellationToken cancellationToken)
    {
        var query = _db.Suppliers
            .AsNoTracking()
            .Where(s => s.IsActive);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(s =>
                EF.Functions.Like(s.Name, $"%{request.Search}%"));
        }

        return await query
            .OrderBy(s => s.Name)
            .Select(s => new SupplierListItemDto
            {
                Id = s.Id,
                Name = s.Name,
                IsActive = s.IsActive
            })
            .ToListAsync(cancellationToken);
    }
}
