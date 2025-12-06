using System;
using System.Collections.Generic;
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

public sealed class AddPurchaseRequestItemCommandHandler : IRequestHandler<AddPurchaseRequestItemCommand, PurchaseRequestDto>
{
    private readonly IApplicationDbContext _context;

    public AddPurchaseRequestItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PurchaseRequestDto> Handle(AddPurchaseRequestItemCommand request, CancellationToken cancellationToken)
    {
        var purchaseRequest = await _context.PurchaseRequests
            .Include(pr => pr.Items)
            .FirstOrDefaultAsync(pr => pr.Id == request.PurchaseRequestId, cancellationToken)
            ?? throw new InvalidOperationException("Purchase request not found.");

        if (purchaseRequest.Status != PurchaseRequestStatus.Draft)
        {
            throw new InvalidOperationException("Only draft purchase requests can be modified.");
        }

        var material = await _context.Materials
            .FirstOrDefaultAsync(entity => entity.Id == request.MaterialId, cancellationToken)
            ?? throw new InvalidOperationException("Material not found.");

        purchaseRequest.AddItem(request.MaterialId, request.Quantity);
        await _context.SaveChangesAsync(cancellationToken);

        var materialIds = purchaseRequest.Items.Select(item => item.MaterialId).Distinct().ToList();
        var materials = await _context.Materials
            .Where(entity => materialIds.Contains(entity.Id))
            .ToDictionaryAsync(entity => entity.Id, cancellationToken);

        return PurchaseRequestDto.FromEntity(purchaseRequest, purchaseRequest.Items, materials);
    }
}
