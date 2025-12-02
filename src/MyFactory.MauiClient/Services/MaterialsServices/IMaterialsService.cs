using System;
using MyFactory.MauiClient.Models.Materials;

namespace MyFactory.MauiClient.Services.MaterialsServices
{
    public interface IMaterialsService
    {
        Task<List<MaterialListResponse>?> ListAsync(string? type = null);
        Task<MaterialCardResponse?> GetAsync(string id);
        // Task<CreateMaterialResponse?> CreateAsync(CreateMaterialRequest request);
        // Task<UpdateMaterialResponse?> UpdateAsync(string id, UpdateMaterialRequest request);
        // Task<List<MaterialPriceHistoryItem>?> PriceHistoryAsync(string id);
        Task<AddMaterialPriceResponse?> AddPriceAsync(string id, AddMaterialPriceRequest request);
        // Task<MaterialTypeResponse?> GetMaterialTypeByIdAsync(Guid id);
    }
}