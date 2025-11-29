using MyFactory.MauiClient.Models.Materials;

namespace MyFactory.MauiClient.Services.MaterialsServices
{
    public interface IMaterialsService
    {
        Task<List<MaterialResponse>?> ListAsync(string? type = null);
        Task<MaterialResponse?> GetAsync(string id);
        Task<CreateMaterialResponse?> CreateAsync(CreateMaterialRequest request);
        Task<UpdateMaterialResponse?> UpdateAsync(string id, UpdateMaterialRequest request);
        Task<List<MaterialPriceHistoryResponse>?> PriceHistoryAsync(string id);
        Task<AddMaterialPriceResponse?> AddPriceAsync(string id, AddMaterialPriceRequest request);
        Task<MaterialTypeResponse?> GetMaterialTypeByIdAsync(Guid id);
    }
}