namespace MyFactory.WebApi.Contracts.Products;

public record ProductUpdateRequest(
    string Sku,
    string Name,
    double PlanPerHour,
    string Description
);
