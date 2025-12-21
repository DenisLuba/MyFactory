using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.PurchaseRequests;

namespace MyFactory.Application.OldFeatures.PurchaseRequests.Queries.GetPurchaseRequestById;

public sealed class GetPurchaseRequestByIdQueryHandler : IRequestHandler<GetPurchaseRequestByIdQuery, PurchaseRequestDto>
{
    private readonly IApplicationDbContext _context;

    public GetPurchaseRequestByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PurchaseRequestDto> Handle(GetPurchaseRequestByIdQuery request, CancellationToken cancellationToken)
    {
        var purchaseRequest = await _context.PurchaseRequests
            .AsNoTracking()
            .Include(pr => pr.Items)
            .FirstOrDefaultAsync(pr => pr.Id == request.Id, cancellationToken)
            ?? throw new InvalidOperationException("Purchase request not found.");

        var materialIds = purchaseRequest.Items.Select(item => item.MaterialId).Distinct().ToList();
        var materials = await _context.Materials
            .Where(material => materialIds.Contains(material.Id))
            .ToDictionaryAsync(material => material.Id, cancellationToken);

        return PurchaseRequestDto.FromEntity(purchaseRequest, purchaseRequest.Items, materials);
    }
}
