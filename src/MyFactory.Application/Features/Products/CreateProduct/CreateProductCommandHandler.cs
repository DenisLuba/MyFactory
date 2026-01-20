using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Products;

namespace MyFactory.Application.Features.Products.CreateProduct;

public sealed class CreateProductCommandHandler
    : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IApplicationDbContext _db;

    public CreateProductCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        var sku = await GenerateSkuAsync(cancellationToken);

        var product = new ProductEntity(
            sku,
            request.Name,
            request.Status,
            planPerHour: request.PlanPerHour,
            description: request.Description,
            version: request.Version
        );

        _db.Products.Add(product);
        await _db.SaveChangesAsync(cancellationToken);

        return product.Id;
    }

    private async Task<string> GenerateSkuAsync(CancellationToken cancellationToken)
    {
        const int maxAttempts = 20;
        for (var i = 0; i < maxAttempts; i++)
        {
            var candidate = $"PRD-{Random.Shared.Next(1000, 9999)}";
            var exists = await _db.Products
                .AsNoTracking()
                .AnyAsync(p => p.Sku == candidate, cancellationToken);

            if (!exists)
                return candidate;
        }

        throw new InvalidOperationException("Failed to generate unique SKU.");
    }
}
