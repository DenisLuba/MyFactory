using MyFactory.MauiClient.Models.Suppliers;

namespace MyFactory.MauiClient.Services.Suppliers;

public interface ISuppliersService
{
    Task<IReadOnlyList<SupplierListItemResponse>?> GetListAsync(string? search = null);
    Task<SupplierDetailsResponse?> GetDetailsAsync(Guid id);
    Task<CreateSupplierResponse?> CreateAsync(CreateSupplierRequest request);
    Task UpdateAsync(Guid id, UpdateSupplierRequest request);
    Task DeleteAsync(Guid id);
}
