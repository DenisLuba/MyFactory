using MediatR;
using MyFactory.Application.DTOs.Warehouses;

namespace MyFactory.Application.Features.Warehouses.TransferProducts;

public sealed record TransferProductsCommand(
    Guid FromWarehouseId,
    Guid ToWarehouseId,
    IReadOnlyCollection<TransferProductItemDto> Items
) : IRequest;