using MyFactory.MauiClient.Models.MaterialTypes;

namespace MyFactory.MauiClient.Services.MaterialTypes;

public interface IMaterialTypesService
{
    Task<IReadOnlyList<MaterialTypeModel>> GetListAsync(CancellationToken ct = default);
    Task<MaterialTypeModel> GetDetailsAsync(Guid id, CancellationToken ct = default);
    Task<Guid> CreateAsync(CreateMaterialTypeRequest request, CancellationToken ct = default);
    Task UpdateAsync(Guid id, UpdateMaterialTypeRequest request, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}