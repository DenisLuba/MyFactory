using MediatR;

namespace MyFactory.Application.Features.Products.DeleteProduct;

public sealed record DeleteProductCommand(Guid ProductId) : IRequest;