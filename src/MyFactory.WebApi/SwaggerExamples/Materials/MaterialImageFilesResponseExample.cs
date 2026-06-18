using MyFactory.WebApi.Contracts.Materials;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Materials;

public sealed class MaterialImageFilesResponseExample : IExamplesProvider<IReadOnlyList<MaterialImageFileResponse>>
{
    public IReadOnlyList<MaterialImageFileResponse> GetExamples() =>
    [
        new(
            Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
            MaterialId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"),
            FileName: "front.jpg",
            ContentType: "image/jpeg",
            Content: [255, 216, 255, 224]),
        new(
            Id: Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccc0003"),
            MaterialId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"),
            FileName: "back.jpg",
            ContentType: "image/jpeg",
            Content: [255, 216, 255, 225])
    ];
}
