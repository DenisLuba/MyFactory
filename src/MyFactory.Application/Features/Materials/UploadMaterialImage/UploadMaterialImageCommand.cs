using MediatR;

namespace MyFactory.Application.Features.Materials.UploadMaterialImage;

public sealed record UploadMaterialImageCommand(
    Guid MaterialId,
    string FileName,
    string? ContentType,
    byte[] Content
) : IRequest<Guid>;
