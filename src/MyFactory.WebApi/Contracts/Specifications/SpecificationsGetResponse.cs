namespace MyFactory.WebApi.Contracts.Specifications;

public record SpecificationsGetResponse(
    Guid Id,
    string Sku,
    string Name,
    double PlanPerHour,
    BomItemResponse[] Bom,
    OperationResponse[] Operations
);

public record BomItemResponse(
    Guid MaterialId,
    string Material,
    double Qty,
    string Unit,
    decimal Price
);

public record OperationResponse(
    string Code,
    string Name,
    double Minutes,
    decimal Cost
);

