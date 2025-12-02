using MyFactory.MauiClient.Models.Specifications;

namespace MyFactory.MauiClient.Services.SpecificationsServices
{
    public interface ISpecificationsService
    {
        Task<IReadOnlyList<SpecificationsListResponse>?> ListAsync();
        Task<SpecificationsGetResponse?> GetAsync(Guid id);
        Task<SpecificationsCreateResponse?> CreateAsync(SpecificationsCreateRequest request);
        Task<SpecificationsUpdateResponse?> UpdateAsync(Guid id, SpecificationsUpdateRequest request);
        Task<IReadOnlyList<SpecificationBomItemResponse>?> GetBomAsync(Guid id);
        Task<SpecificationsAddBomResponse?> AddBomAsync(Guid id, SpecificationsAddBomRequest request);
        Task<SpecificationsDeleteBomItemResponse?> DeleteBomItemAsync(Guid id, Guid bomId);
        Task<IReadOnlyList<SpecificationOperationItemResponse>?> GetOperationsAsync(Guid id);
        Task<SpecificationsAddOperationResponse?> AddOperationAsync(Guid id, SpecificationsAddOperationRequest request);
        /*Task<SpecificationsUploadImageResponse?> UploadImageAsync(Guid id, Stream imageStream, string fileName, string? contentType);*/
        Task<SpecificationsCostResponse?> CostAsync(Guid id, DateTime? asOf = null);
    }
}