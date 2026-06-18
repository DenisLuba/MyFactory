using MediatR;

namespace MyFactory.Application.Features.ProductTypes.DeleteProductType;

public sealed record DeleteProductTypeCommand(Guid ProductTypeId) : IRequest;
