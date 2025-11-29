using MyFactory.MauiClient.Models.Suppliers;

namespace MyFactory.MauiClient.Services.SuppliersServices;

public interface ISuppliersService
{
    Task<List<SupplierResponse>?> ListAsync();
    Task<SupplierResponse?> GetAsync(Guid id);
    Task<SuppliersCreateUpdateDeleteResponse?> CreateAsync(SuppliersCreateUpdateRequest request);
    Task<SuppliersCreateUpdateDeleteResponse?> UpdateAsync(Guid id, SuppliersCreateUpdateRequest request);
    Task<SuppliersCreateUpdateDeleteResponse?> DeleteAsync(Guid id);
}
