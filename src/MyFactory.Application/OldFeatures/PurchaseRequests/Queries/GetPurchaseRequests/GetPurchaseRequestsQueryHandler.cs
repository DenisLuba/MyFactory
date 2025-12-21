using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.PurchaseRequests;
using MyFactory.Domain.Entities.Warehousing;

namespace MyFactory.Application.OldFeatures.PurchaseRequests.Queries.GetPurchaseRequests;

public sealed class GetPurchaseRequestsQueryHandler : IRequestHandler<GetPurchaseRequestsQuery, IReadOnlyCollection<PurchaseRequestDto>>
{
    private readonly IApplicationDbContext _context;

    public GetPurchaseRequestsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<PurchaseRequestDto>> Handle(GetPurchaseRequestsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.PurchaseRequests
            .AsNoTracking()
            .Include(pr => pr.Items)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            var status = request.Status.Trim();
            query = query.Where(pr => pr.Status == status);
        }

        var purchaseRequests = await query
            .OrderByDescending(pr => pr.CreatedAt)
            .ToListAsync(cancellationToken);

        var materialIds = purchaseRequests
            .SelectMany(pr => pr.Items)
            .Select(item => item.MaterialId)
            .Distinct()
            .ToList();

        var materials = await _context.Materials
            .Where(material => materialIds.Contains(material.Id))
            .ToDictionaryAsync(material => material.Id, cancellationToken);

        return purchaseRequests
            .Select(pr => PurchaseRequestDto.FromEntity(pr, pr.Items, materials))
            .ToList();
    }
}
