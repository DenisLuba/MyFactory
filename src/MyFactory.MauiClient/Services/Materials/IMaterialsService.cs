using MyFactory.MauiClient.Models.Common;
using MyFactory.MauiClient.Models.Materials;
using MyFactory.MauiClient.Services.Common;

namespace MyFactory.MauiClient.Services.Materials;

public interface IMaterialsService : IGetListService<MaterialListItemResponse>
{
    Task<MaterialDetailsResponse?> GetDetailsAsync(Guid id);
    Task<Guid> CreateAsync(CreateMaterialRequest request, CancellationToken ct = default);
    Task UpdateAsync(Guid id, UpdateMaterialRequest request);
    Task DeleteAsync(Guid id, CancellationToken ct = default);

    Task<IReadOnlyList<MaterialImageFileResponse>?> GetImagesAsync(Guid materialId);
    Task<byte[]?> GetImageContentAsync(Guid imageId, CancellationToken cancellationToken = default);
    Task<Guid?> UploadImageAsync(Guid materialId, Stream content, string fileName, string contentType, CancellationToken cancellationToken = default);
    Task DeleteImageAsync(Guid imageId);
}
