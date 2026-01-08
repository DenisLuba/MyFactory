using MyFactory.MauiClient.Models.Units;

namespace MyFactory.MauiClient.Services.Units;

public interface IUnitsService
{
    Task<IReadOnlyList<UnitResponse>?> GetListAsync(CancellationToken ct = default);
    Task<Guid> CreateAsync(AddUnitRequest request, CancellationToken ct = default);
    Task UpdateAsync(Guid id, UpdateUnitRequest request, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
