using MyFactory.MauiClient.Models.Products;

namespace MyFactory.MauiClient.Services.Products;

public interface IProductsService
{
    Task<IReadOnlyList<ProductListItemResponse>?> GetListAsync(string? search = null, string? sortBy = null, bool sortDesc = false);
    Task<ProductDetailsResponse?> GetDetailsAsync(Guid id);
    Task<CreateProductResponse?> CreateAsync(CreateProductRequest request);
    Task UpdateAsync(Guid id, UpdateProductRequest request);

    Task<AddProductMaterialResponse?> AddMaterialAsync(Guid productId, AddProductMaterialRequest request);
    Task UpdateMaterialAsync(Guid productMaterialId, UpdateProductMaterialRequest request);
    Task RemoveMaterialAsync(Guid productMaterialId);

    Task SetProductionCostsAsync(Guid productId, SetProductProductionCostsRequest request);

    Task<IReadOnlyList<ProductImageFileResponse>?> GetImagesAsync(Guid productId);
    Task<byte[]?> GetImageContentAsync(Guid imageId, CancellationToken cancellationToken = default);
    Task<Guid?> UploadImageAsync(Guid productId, Stream content, string fileName, string contentType, CancellationToken cancellationToken = default);
    Task DeleteImageAsync(Guid imageId);
}
