using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Features.ProductTypes.UpdateProductType;

public sealed class UpdateProductTypeCommandHandler
    : IRequestHandler<UpdateProductTypeCommand, Guid>
{
    private readonly IApplicationDbContext _db;

    public UpdateProductTypeCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(UpdateProductTypeCommand request, CancellationToken cancellationToken)
    {
        var productType = await _db.ProductTypes
            .FirstOrDefaultAsync(pt => pt.Id == request.ProductTypeId, cancellationToken)
            ?? throw new NotFoundException("Product type not found");

        productType.Update(request.Type, request.Description);

        await _db.SaveChangesAsync(cancellationToken);
        return productType.Id;
    }
}
