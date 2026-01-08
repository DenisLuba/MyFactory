using MyFactory.MauiClient.Models.Materials;

namespace MyFactory.MauiClient.Services.Materials;

public interface IMaterialsService
{
    Task<IReadOnlyList<MaterialListItemResponse>?> GetListAsync(string? materialName = null, string? materialType = null, bool? isActive = null, Guid? warehouseId = null);
    Task<MaterialDetailsResponse?> GetDetailsAsync(Guid id);
    Task<Guid> CreateAsync(CreateMaterialRequest request, CancellationToken ct = default);
    Task UpdateAsync(Guid id, UpdateMaterialRequest request);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
