using MediatR;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Products;

namespace MyFactory.Application.Features.ProductTypes.CreateProductType;

public sealed class CreateProductTypeCommandHandler
    : IRequestHandler<CreateProductTypeCommand, Guid>
{
    private readonly IApplicationDbContext _db;

    public CreateProductTypeCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(CreateProductTypeCommand request, CancellationToken cancellationToken)
    {
        var productType = new ProductTypeEntity(request.Type, request.Description);

        _db.ProductTypes.Add(productType);
        await _db.SaveChangesAsync(cancellationToken);

        return productType.Id;
    }
}
