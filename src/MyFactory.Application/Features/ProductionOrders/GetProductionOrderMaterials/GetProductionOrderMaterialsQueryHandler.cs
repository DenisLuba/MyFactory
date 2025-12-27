using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.ProductionOrders;

namespace MyFactory.Application.Features.ProductionOrders.GetProductionOrderMaterials;

public sealed class GetProductionOrderMaterialsQueryHandler : IRequestHandler<GetProductionOrderMaterialsQuery, IReadOnlyList<ProductionOrderMaterialDto>>
{
    private readonly IApplicationDbContext _db;

    public GetProductionOrderMaterialsQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<ProductionOrderMaterialDto>> Handle(GetProductionOrderMaterialsQuery request, CancellationToken cancellationToken)
    {
        // Производственный заказ
        var po = await _db.ProductionOrders
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.ProductionOrderId, cancellationToken)
            ?? throw new NotFoundException("Production order not found");

        // Пункт заказа от заказчика, содержащий товар , который нужно произвести по данному производственному заказу
        var soi = await _db.SalesOrderItems
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == po.SalesOrderItemId, cancellationToken)
            ?? throw new NotFoundException("Sales order item not found");

        // Материалы, необходимые для производства запланированного количества продукции, и их наличие на складе
        var query =
            from pm in _db.ProductMaterials.AsNoTracking() // ProductMaterials - связь товаров и материалов с указанием количества материала на единицу товара
            join m in _db.Materials.AsNoTracking() on pm.MaterialId equals m.Id // Materials - справочник материалов
            join wm in _db.WarehouseMaterials.AsNoTracking() on pm.MaterialId equals wm.MaterialId // WarehouseMaterials - наличие материалов на складе
            into wmGroup // Группировка по материалам для подсчета доступного количества
            where pm.ProductId == soi.ProductId
            select new
            {
                m.Id,
                m.Name,
                RequiredQty = pm.QtyPerUnit * po.QtyPlanned,
                AvailableQty = wmGroup.Sum(x => (decimal?)x.Qty) ?? 0
            };

        return await query
            .Select(x => new ProductionOrderMaterialDto
            {
                MaterialId = x.Id,
                MaterialName = x.Name,
                RequiredQty = x.RequiredQty,
                AvailableQty = x.AvailableQty,
                MissingQty = Math.Max(0, x.RequiredQty - x.AvailableQty)
            })
            .ToListAsync(cancellationToken);
    }
}
