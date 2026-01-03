using MediatR;

namespace MyFactory.Application.Features.Products.UploadProductImage;

public sealed record UploadProductImageCommand(
    Guid ProductId,
    string FileName,
    string? ContentType,
    byte[] Content
) : IRequest<Guid>;
