using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Products;

namespace MyFactory.Application.Features.Products.SetProductProductionCosts;

public sealed class SetProductProductionCostsCommandHandler
    : IRequestHandler<SetProductProductionCostsCommand>
{
    private readonly IApplicationDbContext _db;

    public SetProductProductionCostsCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(
        SetProductProductionCostsCommand request,
        CancellationToken cancellationToken)
    {
        var existing = await _db.ProductDepartmentCosts
            .Where(x => x.ProductId == request.ProductId)
            .ToListAsync(cancellationToken);

        foreach (var dto in request.Costs)
        {
            var entity = existing.FirstOrDefault(x =>
                x.DepartmentId == dto.DepartmentId);

            if (entity is null)
            {
                entity = new ProductDepartmentCostEntity(
                    request.ProductId,
                    dto.DepartmentId,
                    expensesPerUnit: dto.Expenses,
                    cutCostPerUnit: dto.CutCost,
                    sewingCostPerUnit: dto.SewingCost,
                    packCostPerUnit: dto.PackCost
                );

                _db.ProductDepartmentCosts.Add(entity);
            }
            else
            {
                entity.Update(
                    expenses: dto.Expenses,
                    cut: dto.CutCost,
                    sewing: dto.SewingCost,
                    pack: dto.PackCost
                );
            }
        }

        await _db.SaveChangesAsync(cancellationToken);
    }
}
