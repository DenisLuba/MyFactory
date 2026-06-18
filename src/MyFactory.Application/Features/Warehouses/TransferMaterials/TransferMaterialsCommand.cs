using MediatR;
using MyFactory.Application.DTOs.Warehouses;

namespace MyFactory.Application.Features.Warehouses.TransferMaterials;

public sealed record TransferMaterialsCommand(
    Guid FromWarehouseId,
    Guid ToWarehouseId,
    IReadOnlyCollection<TransferMaterialItemDto> Items
) : IRequest;