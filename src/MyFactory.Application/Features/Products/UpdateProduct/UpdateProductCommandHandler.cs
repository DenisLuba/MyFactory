using MediatR;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore; 

namespace MyFactory.Application.Features.Products.UpdateProduct;

public sealed class UpdateProductCommandHandler
    : IRequestHandler<UpdateProductCommand, Guid>
{
    private readonly IApplicationDbContext _db;

    public UpdateProductCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(
        UpdateProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _db.Products
            .FirstOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken)
            ?? throw new NotFoundException("Product not found");

        product.Update(
            name: request.Name,
            planPerHour: request.PlanPerHour,
            status: request.Status,
            description: request.Description,
            version: request.Version
        );

        await _db.SaveChangesAsync(cancellationToken);
        return product.Id;
    }
}
