using MediatR;
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
        var product = new ProductEntity(
            request.Sku,
            request.Name,
            request.Status,
            planPerHour: request.PlanPerHour
        );

        _db.Products.Add(product);
        await _db.SaveChangesAsync(cancellationToken);

        return product.Id;
    }
}
