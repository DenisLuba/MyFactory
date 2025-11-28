using MyFactory.WebApi.Contracts.Materials;

namespace MyFactory.MauiClient.Models.Specifications;

public record SpecificationsGetResponse(
    Guid Id,
    string Sku,
    string Name,
    double PlanPerHour,
    BomItemResponse[] Bom,
    OperationItemResponse[] Operations
);

public record BomItemResponse(
    Guid MaterialId,
    string MaterialName,
    double Quantity,
    Units Unit,
    decimal Price
);

public record OperationItemResponse(
    Guid OperationId,
    string Code,
    string Name,
    double Minutes,
    decimal Cost
);

