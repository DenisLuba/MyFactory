using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.ProductTypes;

namespace MyFactory.Application.Features.ProductTypes.GetProductTypes;

public sealed class GetProductTypesQueryHandler
    : IRequestHandler<GetProductTypesQuery, IReadOnlyList<ProductTypeDto>>
{
    private readonly IApplicationDbContext _db;

    public GetProductTypesQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<ProductTypeDto>> Handle(GetProductTypesQuery request, CancellationToken cancellationToken)
    {
        var query = _db.ProductTypes.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(pt => pt.Type.Contains(request.Search));
        }

        var items = await query
            .OrderBy(pt => pt.Type)
            .Select(pt => new ProductTypeDto(pt.Id, pt.Type, pt.Description))
            .ToListAsync(cancellationToken);

        return items;
    }
}
