using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.PurchaseRequests;
using MyFactory.Application.Features.PurchaseRequests.Commands;
using MyFactory.Domain.Entities.Warehousing;

namespace MyFactory.Application.Features.PurchaseRequests.Handlers;

public sealed class CreatePurchaseRequestCommandHandler : IRequestHandler<CreatePurchaseRequestCommand, PurchaseRequestDto>
{
    private readonly IApplicationDbContext _context;

    public CreatePurchaseRequestCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PurchaseRequestDto> Handle(CreatePurchaseRequestCommand request, CancellationToken cancellationToken)
    {
        var number = request.PrNumber.Trim();

        var exists = await _context.PurchaseRequests
            .AsNoTracking()
            .AnyAsync(pr => pr.PrNumber == number, cancellationToken);

        if (exists)
        {
            throw new InvalidOperationException("Purchase request number already exists.");
        }

        var purchaseRequest = new PurchaseRequest(number, request.CreatedAt);
        await _context.PurchaseRequests.AddAsync(purchaseRequest, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return PurchaseRequestDto.FromEntity(purchaseRequest, Array.Empty<PurchaseRequestItemDto>());
    }
}
