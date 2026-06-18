using MyFactory.MauiClient.Models.Positions;

namespace MyFactory.MauiClient.Services.Positions;

public interface IPositionsService
{
    Task<IReadOnlyList<PositionListItemResponse>?> GetListAsync(Guid? departmentId = null, bool includeInactive = false, PositionSortBy sortBy = PositionSortBy.Name, bool sortDesc = false);
    Task<PositionDetailsResponse?> GetDetailsAsync(Guid id);
    Task<CreatePositionResponse?> CreateAsync(CreatePositionRequest request);
    Task UpdateAsync(Guid id, UpdatePositionRequest request);
}
