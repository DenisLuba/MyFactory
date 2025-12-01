using MyFactory.MauiClient.Models.Production.MaterialTransfers;
using MyFactory.MauiClient.Models.Production.ProductionOrders;

namespace MyFactory.MauiClient.Services.ProductionServices;

public interface IProductionOrdersService
{
    Task<IReadOnlyList<ProductionOrderListResponse>?> GetListAsync();
    Task<ProductionOrderCardResponse?> GetByIdAsync(Guid orderId);
    Task<IReadOnlyList<MaterialTransferItemDto>?> GetMaterialTransfersAsync(Guid orderId);
    Task<IReadOnlyList<StageDistributionItem>?> GetStageDistributionAsync(Guid orderId);
}
