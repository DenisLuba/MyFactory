using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyFactory.MauiClient.Models.Products;

namespace MyFactory.MauiClient.Services.ProductsServices;

public interface IProductsService
{
    Task<IReadOnlyList<ProductsListResponse>?> GetProductsAsync();
    Task<ProductCardResponse?> GetProductAsync(Guid id);
    Task<ProductUpdateResponse?> UpdateProductAsync(Guid id, ProductUpdateRequest request);
    Task<IReadOnlyList<ProductBomItemResponse>?> GetBomAsync(Guid productId);
    Task<IReadOnlyList<ProductOperationItemResponse>?> GetOperationsAsync(Guid productId);
	Task<ProductBomItemResponse?> AddBomItemAsync(Guid productId, ProductBomCreateRequest request);
	Task<ProductBomDeleteResponse?> DeleteBomItemAsync(Guid productId, Guid lineId);
	Task<ProductOperationItemResponse?> AddOperationAsync(Guid productId, ProductOperationCreateRequest request);
	Task<ProductOperationDeleteResponse?> DeleteOperationAsync(Guid productId, Guid lineId);
}
