namespace MyFactory.WebApi.Contracts.Specifications;

public record SpecificationsAddBomRequest(
    Guid MaterialId,
    double Qty,
    string Unit,
    decimal Price
);

