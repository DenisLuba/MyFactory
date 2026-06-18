using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Features.Products.DeleteProduct;

public sealed class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly IApplicationDbContext _db;

    public DeleteProductCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _db.Products
            .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);

        var images = await _db.ProductImages
            .Where(i => i.ProductId == request.ProductId)
            .ToListAsync(cancellationToken);

        if (product is null)
        {
            throw new NotFoundException($"Product {request.ProductId} not found");
        }

        if (images != null && images.Count > 0)
        {
            _db.ProductImages.RemoveRange(images);
        }
        _db.Products.Remove(product);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
