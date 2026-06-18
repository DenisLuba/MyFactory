using MyFactory.MauiClient.Models.ProductTypes;

namespace MyFactory.MauiClient.Services.ProductTypes;

public interface IProductTypesService
{
    Task<IReadOnlyList<ProductTypeModel>> GetListAsync(string? search = null, CancellationToken ct = default);
    Task<ProductTypeModel> GetDetailsAsync(Guid id, CancellationToken ct = default);
    Task<ProductTypeModel?> GetByProductIdAsync(Guid productId, CancellationToken ct = default);
    Task<Guid> CreateAsync(CreateProductTypeRequest request, CancellationToken ct = default);
    Task UpdateAsync(Guid id, UpdateProductTypeRequest request, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
