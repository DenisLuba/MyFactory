using MediatR;

namespace MyFactory.Application.Features.Products.DeleteProductImage;

public sealed record DeleteProductImageCommand(Guid ImageId) : IRequest;
