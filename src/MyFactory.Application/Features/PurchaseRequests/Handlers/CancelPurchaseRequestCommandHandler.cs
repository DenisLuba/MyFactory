using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.PurchaseRequests;
using MyFactory.Application.Features.PurchaseRequests.Commands;
using MyFactory.Domain.Entities.Warehousing;

namespace MyFactory.Application.Features.PurchaseRequests.Handlers;

public sealed class CancelPurchaseRequestCommandHandler : IRequestHandler<CancelPurchaseRequestCommand, PurchaseRequestDto>
{
    private readonly IApplicationDbContext _context;

    public CancelPurchaseRequestCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PurchaseRequestDto> Handle(CancelPurchaseRequestCommand request, CancellationToken cancellationToken)
    {
        var purchaseRequest = await _context.PurchaseRequests
            .Include(pr => pr.Items)
            .FirstOrDefaultAsync(pr => pr.Id == request.PurchaseRequestId, cancellationToken)
            ?? throw new InvalidOperationException("Purchase request not found.");

        if (purchaseRequest.Status != PurchaseRequestStatuses.Draft && purchaseRequest.Status != PurchaseRequestStatuses.Submitted)
        {
            throw new InvalidOperationException("Only draft or submitted purchase requests can be cancelled.");
        }

        purchaseRequest.Cancel();
        await _context.SaveChangesAsync(cancellationToken);

        var materialIds = purchaseRequest.Items.Select(item => item.MaterialId).Distinct().ToList();
        var materials = await _context.Materials
            .Where(material => materialIds.Contains(material.Id))
            .ToDictionaryAsync(material => material.Id, cancellationToken);

        return PurchaseRequestDto.FromEntity(purchaseRequest, purchaseRequest.Items, materials);
    }
}
