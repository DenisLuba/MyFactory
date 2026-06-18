using MyFactory.MauiClient.Models.MaterialPurchaseOrders;
using MyFactory.MauiClient.Models.Suppliers;
using MyFactory.MauiClient.Services.Common;

namespace MyFactory.MauiClient.Services.Suppliers;

public interface ISuppliersService : IGetListService<SupplierListItemResponse>
{
    Task<SupplierDetailsResponse?> GetDetailsAsync(Guid id);
    Task<CreateSupplierResponse?> CreateAsync(CreateSupplierRequest request);
    Task UpdateAsync(Guid id, UpdateSupplierRequest request);
    Task DeleteAsync(Guid id);
}
